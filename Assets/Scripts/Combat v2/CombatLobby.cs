using System;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Contaquest.Server;
using Unity.Netcode;
using System.Collections.Generic;
using System.Linq;

namespace Contaquest.Metaverse.Combat2
{
    public class CombatLobby : NetworkBehaviour
    {
        [TabGroup("Properties")] [SerializeField] private bool loadArenaVisual;
        [TabGroup("Properties")] public string arenaName;

        [TabGroup("References")] public CombatUser combatUserPrefab;
        [TabGroup("References")] public CombatManager combatManager;
        [TabGroup("References")] [System.NonSerialized, ReadOnly] public ArenaDefinition arenaDefinition;
        [TabGroup("References")] [System.NonSerialized, ReadOnly] public List<CombatUser> combatUsers;
        [TabGroup("References")] [ReadOnly] public ClientRpcParams lobbyRPC;
        [TabGroup("References")] [ReadOnly] public int lobbyId;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private bool isArenaInitialized = false;
        [TabGroup("State")] [ShowInInspector, ReadOnly] [InlineEditor] private ArenaData arenaData;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private bool waitingForAnimation = false;

        public void InitializeInstance()
        {
            InitializeArena();
            combatManager.Initialize(this);
            lobbyId = GetInstanceID();
            combatUsers = new List<CombatUser>();
        }
        private void InitializeArena(Action callback = null)
        {
            arenaData = ArenaDataManager.Instance.GetArenaDataByName(arenaName);

            if(arenaData == null)
            {
                Debug.Log("Arena was not found");
                return;
            }
            // TODO: Idk, maybe change this implementation? works fine tho...
            arenaData.StartLoadArenaDefinition(()=>
            {
                arenaDefinition = Instantiate(arenaData.arenaDefinitionPrefab, transform).GetComponent<ArenaDefinition>();
                arenaDefinition.Initialize(this);
                isArenaInitialized = true;
                callback?.Invoke();
            });
            if(loadArenaVisual)
            {
                arenaData.StartLoadArenaVisual(()=>
                {
                    Instantiate(arenaData.arenaVisualPrefab, transform);
                });
            }
        }
        //private void InitializeUsers()
        //{
        //    combatUsers = new CombatUser[arenaData.maxUserCount];
        //    for (int i = 0; i < combatUsers.Length; i++)
        //    {
        //        combatUsers[i] = Instantiate(combatUserPrefab);
        //        combatUsers[i].Initialize(this, i);
        //    }
        //}
        
        //public CombatUser GetUserById(int userId)
        //{
        //    // TODO: implement getting by user ID
        //    return null;
        //}
        public CombatUser GetUserByNetworkInfo(PlayerConnectionPayload networkInfo)
        {
            foreach (CombatUser user in combatUsers)
            {
                if (networkInfo.Equals(user.networkInfo))
                    return user;
            }
            return null;
        }
        public CombatUser GetUserByID(int userId)
        {
            return combatUsers.FirstOrDefault((combatUser) => combatUser.userId == userId);
        }

        #region Server

        public void OnUserConnected(ulong clientId, string username)
        {
            //only on server
            CombatUser newCombatUser = Instantiate(combatUserPrefab);
            newCombatUser.InitializeNetworkPlayer(this, combatUsers.Count, new NetworkUserInfo(new PlayerConnectionPayload(clientId, username, lobbyId)));
            combatUsers.Add(newCombatUser);
            if(combatUsers.Count == arenaData.maxUserCount)
            {
                StartLobby();
            }
        }

        void StartLobby()
        {
            SetLobbyRPC();
            NetworkUserInfo[] networkUsers = new NetworkUserInfo[combatUsers.Count];
            for (int i = 0; i < combatUsers.Count; i++)
            {
                networkUsers[i] = combatUsers[i].networkInfo;
            }
            NetworkConnection.Singleton.InitializeUsersClientRpc(networkUsers, lobbyRPC);
            //phase 1
        }

        private void SetLobbyRPC()
        {
            lobbyRPC = new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = LobbyClientIds() } };
        }

        private ClientRpcParams ClientRPCParams(PlayerConnectionPayload connectionPayload)
        {
            return new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[1] { connectionPayload.ClientID } } };
        }

        private ulong[] LobbyClientIds()
        {
            ulong[] clinetIds = new ulong[arenaData.maxUserCount];
            for (int i = 0; i < clinetIds.Length; i++)
            {
                clinetIds[i] = combatUsers[i].networkInfo.ClientID;
            }
            return clinetIds;
        }

        public void RecieveInput(CombatActionContext combatContext)
        {
            if (!combatManager.RecieveAndVerifyInput(combatContext))//if verify failed
            {
                NetworkConnection.Singleton.ActionVerificationFailedClientRpc(ClientRPCParams(combatContext.sourceConnection));
            }
        }

        

        #endregion


        #region ClientLogic
        public void InitializeUsers(NetworkUserInfo[] networkUsers)
        {
            combatUsers = new List<CombatUser>();
            for (int i = 0; i < networkUsers.Length; i++)
            {
                combatUsers.Add( Instantiate(combatUserPrefab));
                combatUsers[i].InitializeNetworkPlayer(this, i, networkUsers[i]);
            }
        }
        #endregion

        
    }
}