using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using System;
using GraphQlClient.Core;
using Sirenix.OdinInspector;
using UnityEngine.Events;
namespace Contaquest.Metaverse.Combat2
{
    public class NetworkConnection : NetworkBehaviour
    {
        [TabGroup("References")] [SerializeField] private string[] serverArgs;
        [TabGroup("References")] [SerializeField] private bool isServer;
        [TabGroup("References")] [SerializeField] private int defaultMaxLobbies;
        [TabGroup("Events")] [SerializeField] private UnityEvent<string>[] argAction;
        [TabGroup("Events")] [SerializeField] private UnityEvent<ulong> clientConnected;
        private int maxLobbies;
        [TabGroup("References")] [SerializeField] private GraphApi graphQlApi;
        private PlayerConnectionPayload clientConnectionPayload;
        private const string UserNameVariableName = "UserName";
        private List<CombatLobby> lobbies;
        [TabGroup("References")] [SerializeField] private GameObject lobbyPrefab;

        public static NetworkConnection Singleton { get; private set; }

        private CombatLobby clientCombatLobby;

        void Start()
        {
            if(Singleton == null)
            {
                Singleton = this;
            }
            if (isServer)
                InitializeServer();
        }

        
        
        private void InitializeServer()
        {
            isServer = true;
            maxLobbies = defaultMaxLobbies;
            string[] args = System.Environment.GetCommandLineArgs();
            //if arg matches a serverArg, call argAction with same index. Passes action string after a :
            foreach (string item in args)
            {
                string[] seperated = item.Split(':');
                for (int i = 0; i < serverArgs.Length; i++)
                {
                    if (string.Equals(seperated[0], serverArgs[i], System.StringComparison.OrdinalIgnoreCase))
                    {
                        argAction[i].Invoke(seperated[1]);
                    }
                }
            }

            NetworkManager.Singleton.StartServer();
            NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
        }

        public void ClientConnected(ulong clientID)
        {
            if (isServer)
            {
                clientConnected?.Invoke(clientID);
            }
        }

        public void ChangePort(string port)
        {
            if (!int.TryParse(port, out int portNumber))
                return;
            UNetTransport networkTransport = (UNetTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            networkTransport.ConnectPort = portNumber;
            networkTransport.ServerListenPort = portNumber;
        }

        public void ChangeIP(string ip)
        {
            UNetTransport networkTransport = (UNetTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            networkTransport.ConnectAddress = ip;
        }

        public void ChangeLobbySize(string newSize)
        {
            if (!int.TryParse(newSize, out int newLobbySize))
                return;
            maxLobbies = newLobbySize;
        }

        private void ConnectToServer(string ip, int port, int lobbyID)
        {
            UNetTransport networkTransport = (UNetTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            networkTransport.ConnectAddress = ip;
            networkTransport.ConnectPort = port;

            NetworkManager.Singleton.StartClient();
            clientConnectionPayload = new PlayerConnectionPayload(NetworkManager.Singleton.LocalClientId, PlayerPrefs.GetString(UserNameVariableName), lobbyID);
        }

        public void OnPlayerJoin(ulong clientID, string username, int lobbyID)//send to open lobby, or new lobby
        {
            FindLobby(lobbyID)?.OnUserConnected(clientID, username);
        }

        public CombatLobby NewLobby()
        {
            CombatLobby newLobby = Instantiate(lobbyPrefab).GetComponent<CombatLobby>();
            lobbies.Add(newLobby);
            return newLobby;
        }

        public CombatLobby FindLobby(int lobbyId)
        {
            CombatLobby lobby = null;
            foreach (var item in lobbies)
            {
                if (item.lobbyId == lobbyId)
                {
                    lobby = item;
                    break;
                }
            }
            if (lobby == null)
                Debug.Log("Lobby does not exist");

            return lobby;
        }

        public bool FindClientsLobby(PlayerConnectionPayload connectionPayload, out CombatLobby lobby)
        {
            lobby = null;
            foreach (var item in lobbies)
            {
                if (item.lobbyId == connectionPayload.LobbyID && item.GetUserByNetworkInfo(connectionPayload) != null)
                {
                    lobby = item;
                    break;
                }
            }
            if (lobby == null)
                Debug.Log("Lobby does not exist");

            return lobby != null;
        }

        public ClientRpcParams LobbyRPC(PlayerConnectionPayload connectionPayload)
        {
            if (FindClientsLobby(connectionPayload, out CombatLobby lobby))
                return lobby.lobbyRPC;
            return new ClientRpcParams { };
        }

        #region ClientToLobby
        [ServerRpc(RequireOwnership = false)]
        public void SendInputServerRpc(EquipSlot sourceSlot, int actionIndex, int lobbyId, PlayerConnectionPayload userPayload, PlayerConnectionPayload targetPayload)
        {
            FindLobby(lobbyId).RecieveInput(new CombatActionContext(sourceSlot, actionIndex, lobbyId, userPayload, targetPayload));
        }
        #endregion

        #region ServerToLobby
        [ClientRpc]
        public void InitializeUsersClientRpc(NetworkUserInfo[] users, ClientRpcParams clientRpcParams = default)
        {
            clientCombatLobby.InitializeUsers(users);
        }
        #endregion

        #region ServerToClient
        [ClientRpc]
        public void ActionVerificationFailedClientRpc(ClientRpcParams clientRpcParams = default)
        {
            //player needs input again
        }
        #endregion
    }
}