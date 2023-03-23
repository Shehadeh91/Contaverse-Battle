using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GraphQlClient.EventCallbacks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;


namespace GraphQlClient.Core
{
    public static class ExtensionMethods
    {
        public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<object>();
            asyncOp.completed += obj => { tcs.SetResult(null); };
            return ((Task)tcs.Task).GetAwaiter();
        }
    }

    public class HttpHandler
    {
        public static async Task<UnityWebRequest> PostAsync(string url, string details, string authToken = null)
        {
            var jsonData = JsonConvert.SerializeObject(new { query = details });
            var postData = Encoding.UTF8.GetBytes(jsonData);
            var request = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.SetRequestHeader("Content-Type", "application/json");
            if (!string.IsNullOrEmpty(authToken))
                request.SetRequestHeader("Authorization", "Bearer " + authToken);

            var requestBegin = new OnRequestBegin();
            requestBegin.FireEvent();

            try
            {
                await request.SendWebRequest();
                if (request.error != null) throw new Exception(request.error);
            }
            catch (Exception e)
            {
                var requestFailed = new OnRequestEnded(e);
                requestFailed.FireEvent();
                if (request.error != null) throw new Exception(request.error);
                throw;
            }

            var requestSucceeded = new OnRequestEnded(request.downloadHandler.text);
            requestSucceeded.FireEvent();
            return request;
        }

        public static async Task<UnityWebRequest> PostAsync(UnityWebRequest request, string details)
        {
            var jsonData = JsonConvert.SerializeObject(new { query = details });
            var postData = Encoding.ASCII.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(postData);
            var requestBegin = new OnRequestBegin();
            requestBegin.FireEvent();

            try
            {
                await request.SendWebRequest();
            }
            catch (Exception e)
            {
                Debug.Log("Testing exceptions");
                var requestFailed = new OnRequestEnded(e);
                requestFailed.FireEvent();
            }

            Debug.Log(request.downloadHandler.text);

            var requestSucceeded = new OnRequestEnded(request.downloadHandler.text);
            requestSucceeded.FireEvent();
            return request;
        }


        public static async Task<UnityWebRequest> GetAsync(string url, string authToken = null)
        {
            var request = UnityWebRequest.Get(url);
            if (!string.IsNullOrEmpty(authToken))
                request.SetRequestHeader("Authorization", "Bearer " + authToken);
            var requestBegin = new OnRequestBegin();
            requestBegin.FireEvent();
            try
            {
                await request.SendWebRequest();
            }
            catch (Exception e)
            {
                Debug.Log("Testing exceptions");
                var requestEnded = new OnRequestEnded(e);
                requestEnded.FireEvent();
            }

            Debug.Log(request.downloadHandler.text);
            var requestSucceeded = new OnRequestEnded(request.downloadHandler.text);
            requestSucceeded.FireEvent();
            return request;
        }

        #region Websocket

        //Use this to subscribe to a graphql endpoint
        public static async Task<ClientWebSocket> WebsocketConnect(string subscriptionUrl, string details,
            string authToken = null, string socketId = "1", string protocol = "graphql-ws")
        {
            var subUrl = subscriptionUrl.Replace("http", "ws");
            var id = socketId;
            var cws = new ClientWebSocket();
            cws.Options.AddSubProtocol(protocol);
            if (!string.IsNullOrEmpty(authToken))
                cws.Options.SetRequestHeader("Authorization", "Bearer " + authToken);
            var u = new Uri(subUrl);
            try
            {
                await cws.ConnectAsync(u, CancellationToken.None);
                if (cws.State == WebSocketState.Open)
                    Debug.Log("connected");
                await WebsocketInit(cws);
                await WebsocketSend(cws, id, details);
            }
            catch (Exception e)
            {
                Debug.Log("woe " + e.Message);
            }

            return cws;
        }

        public static async Task<ClientWebSocket> WebsocketConnect(ClientWebSocket cws, string subscriptionUrl,
            string details, string socketId = "1")
        {
            var subUrl = subscriptionUrl.Replace("http", "ws");
            var id = socketId;
            var u = new Uri(subUrl);
            try
            {
                await cws.ConnectAsync(u, CancellationToken.None);
                if (cws.State == WebSocketState.Open)
                    Debug.Log("connected");
                await WebsocketInit(cws);
                await WebsocketSend(cws, id, details);
            }
            catch (Exception e)
            {
                Debug.Log("woe " + e.Message);
            }

            return cws;
        }

        static async Task WebsocketInit(ClientWebSocket cws)
        {
            var jsonData = "{\"type\":\"connection_init\"}";
            var b = new ArraySegment<byte>(Encoding.ASCII.GetBytes(jsonData));
            await cws.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
            GetWsReturn(cws);
        }

        static async Task WebsocketSend(ClientWebSocket cws, string id, string details)
        {
            var jsonData = JsonConvert.SerializeObject(new
                { id = $"{id}", type = "start", payload = new { query = details } });
            var b = new ArraySegment<byte>(Encoding.ASCII.GetBytes(jsonData));
            await cws.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        //Call GetWsReturn to wait for a message from a websocket. GetWsReturn has to be called for each message
        static async void GetWsReturn(ClientWebSocket cws)
        {
            var buf = new ArraySegment<byte>(new byte[1024]);
            buf = WebSocket.CreateClientBuffer(1024, 1024);
            WebSocketReceiveResult r;
            var result = "";
            do
            {
                r = await cws.ReceiveAsync(buf, CancellationToken.None);
                result += Encoding.UTF8.GetString(buf.Array ?? throw new ApplicationException("Buf = null"), buf.Offset,
                    r.Count);
            } while (!r.EndOfMessage);

            if (string.IsNullOrEmpty(result))
                return;
            var obj = new JObject();
            try
            {
                obj = JObject.Parse(result);
            }
            catch (JsonReaderException e)
            {
                throw new ApplicationException(e.Message);
            }

            var subType = (string)obj["type"];
            switch (subType)
            {
                case "connection_ack":
                {
                    Debug.Log("init_success, the handshake is complete");
                    var subscriptionHandshakeComplete =
                        new OnSubscriptionHandshakeComplete();
                    subscriptionHandshakeComplete.FireEvent();
                    GetWsReturn(cws);
                    break;
                }
                case "error":
                {
                    throw new ApplicationException("The handshake failed. Error: " + result);
                }
                case "connection_error":
                {
                    throw new ApplicationException("The handshake failed. Error: " + result);
                }
                case "data":
                {
                    var subscriptionDataReceived = new OnSubscriptionDataReceived(result);
                    subscriptionDataReceived.FireEvent();
                    GetWsReturn(cws);
                    break;
                }
                case "ka":
                {
                    GetWsReturn(cws);
                    break;
                }
                case "subscription_fail":
                {
                    throw new ApplicationException("The subscription data failed");
                }
            }
        }

        public static async Task WebsocketDisconnect(ClientWebSocket cws, string socketId = "1")
        {
            var jsonData = $"{{\"type\":\"stop\",\"id\":\"{socketId}\"}}";
            var b = new ArraySegment<byte>(Encoding.ASCII.GetBytes(jsonData));
            await cws.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
            await cws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
            var subscriptionCanceled = new OnSubscriptionCanceled();
            subscriptionCanceled.FireEvent();
        }

        #endregion

        #region Utility

        public static string FormatJson(string json)
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        #endregion
    }
}