using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using GraphQlClient.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Contaquest.Metaverse.Data;
using User_Registration.Serialization;

namespace Contaquest.Server
{
    public class CombatGraphQLManager : GenericSingleton<CombatGraphQLManager>
    {
        [TabGroup("Properties")] [SerializeField] private bool attemptLoginOnStart = true;

        [TabGroup("References")] [SerializeField] private GraphApi graphQlApi;
        [TabGroup("References")] [SerializeField] private BoolReference isDebugModeEnabled;

        [TabGroup("Events")] [SerializeField] private UnityEvent onFailedLogin;
        [TabGroup("Events")] [SerializeField] private UnityEvent onLoggedIn;

        private const string ConfigPath = "UserName";

        private void Start()
        {
            if(!attemptLoginOnStart)
                return;
                
            StartLogin();
        }
        public void StartLogin()
        {
            if(graphQlApi == null)
                return;
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                onFailedLogin?.Invoke();
                return;
            }
            // if (!PlayerPrefs.HasKey(TokenVariableName))
            // {
            //     Debug.Log("No Login Token available.");
            //     onFailedLogin?.Invoke();
            //     return;
            // }

            // SendLoginRequest(email, password);
        }

        public async void SendLoginRequest(string email, string password)
        {
            Debug.Log($"Sending login request with\nEmail: {email}\nPassword: {password}");

            var output = new { data = new { login = new { token = "", expiresIn = long.MaxValue }}, errors = new GraphqlError[0]};
            var args = new { email, password };

            var result = await graphQlApi.GetQueryResult("LoginQuery", args, output);

            if(result == null || result.data == null || result.data.login == null || string.IsNullOrEmpty(result.data.login.token))
            {
                string errorMessage = "Failed to log in User:";
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);
                onFailedLogin?.Invoke();
                return;
            }
            
            string token = result.data.login.token;
            graphQlApi.SetAuthToken(token);
        }

        public async Task GetUserAccountDataAsync(string userName, Action<UserAccountData> callback)
        {
            if(isDebugModeEnabled.Value)
            {
                callback?.Invoke(new UserAccountData("Debug User" + (int)UnityEngine.Random.Range(0, 1000), Faction.Droidz, null));
                return;
            }
            
            var output = new { data = new { getUserInfo = new UserAccountData() }, errors = new GraphqlError[0] };

            var result = await graphQlApi.GetQueryResult("GetAllUserInfoQuery", null, output);

            if(result == null || result.data == null || result.data.getUserInfo == null)
            {
                string errorMessage = "Failed to get User Data:";
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);

                callback?.Invoke(new UserAccountData("Debug User" + (int)UnityEngine.Random.Range(0, 1000), Faction.Droidz, null));
                return;
            }

            callback?.Invoke(result.data.getUserInfo);
            return;
        }

        public async void GetWalletItems(string userName, Action<WalletItem[]> callback = null)
        {
            // Debug.Log("Getting wallet items");
            WalletItem[] returnedWalletItems = new WalletItem[0];
            if(isDebugModeEnabled.Value)
            {
                callback?.Invoke(returnedWalletItems);
                return;
            }

            // var output = new { data = new { walletContents = new WalletContent[1]}, errors = new GraphqlError[0] };

            // var result = await graphQlApi.GetQueryResult("GetWalletContents", null, output);

            // if(result == null || result.data == null || result.data.walletContents == null)
            // {
            //     string errorMessage = "Failed to get Wallet Contents:";
            //     foreach (var error in result.errors)
            //         errorMessage += "\n" + error.message;
            //     ErrorManager.Instance.ShowError(errorMessage);
            //     return;
            // }
            // List<WalletItem> walletItems = new List<WalletItem>();
            // foreach(var walletContent in result.data.walletContents)
            // {
            //     walletItems.Add(new WalletItem(walletContent.token.itemURI, walletContent.token.id, walletContent.index, walletContent.value));
            // }
            // returnedWalletItems = result.data.walletContents.Select((content) => new WalletItem(content.token.itemURI, content.token.id, content.index, content.value)).ToArray();
            
            callback?.Invoke(returnedWalletItems);
        }
    }
}