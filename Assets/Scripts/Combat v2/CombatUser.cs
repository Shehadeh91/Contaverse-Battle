using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Contaquest.Server;
using Unity.Netcode;
using Contaquest.Metaverse;

namespace Contaquest.Metaverse.Combat2
{
    public class CombatUser : MonoBehaviour
    {
        [TabGroup("Properties")] [SerializeField] private UserType userType;
        [TabGroup("Properties")] public NetworkUserInfo networkInfo;//only server and client owner needs to know this

        [TabGroup("References")] public RobotDataSourceNetworkUser robotDataSourceNetworkUser;
        [TabGroup("References")] public CombatRobotController combatRobotController;
        [TabGroup("References")] [ShowInInspector, ReadOnly] private CombatLobby combatLobby;

        [TabGroup("Events")] [SerializeField] private UnityEvent onUserConnected, onUserDisconnected;
        [TabGroup("Events")] [SerializeField] private UnityEvent onRoundWon, onRoundLost;
        [TabGroup("Events")] [SerializeField] private UnityEvent onFightWon, onFightLost;

        [TabGroup("State")] [System.NonSerialized, ShowInInspector, ReadOnly] public UserAccountData userAccountData;
        [TabGroup("State")] [System.NonSerialized, ShowInInspector, ReadOnly] public int userId;


        public void Initialize(CombatLobby combatLobby, int userId) 
        {
            this.combatLobby = combatLobby;
            this.userId = userId;
            ArenaPosition startArenaPosition = combatLobby.arenaDefinition.GetStartPosition(userId);
            robotDataSourceNetworkUser.LoadUserRobotData("Username", "", "", "", "", "", ()=> combatRobotController.InitializeRobot(combatLobby, startArenaPosition));

            if(userType == UserType.AI)
            {
                //Initialize AI Opponent

                return;
            }
        }

        public void InitializeNetworkPlayer(CombatLobby combatLobby, int userIndex, NetworkUserInfo networkUser)
        {
            this.combatLobby = combatLobby;
            this.userId = userIndex;
            ArenaPosition startArenaPosition = combatLobby.arenaDefinition.GetStartPosition(userIndex);
            combatRobotController.InitializeRobot(combatLobby, startArenaPosition);
            networkInfo = networkUser;
        }

        public void OnUserConnected()
        {
            onUserConnected?.Invoke();
        }

        public void OnUserDisconnected()
        {
            onUserDisconnected?.Invoke();
        }

        public void OnRoundWon()
        {
            onRoundWon?.Invoke();
        }
        public void OnRoundLost()
        {
            onRoundLost?.Invoke();
        }

        public void OnFightWon()
        {
            onFightWon?.Invoke();
        }
        public void OnFightLost()
        {
            onFightLost?.Invoke();
        }
    }

    public struct NetworkUserInfo : INetworkSerializable, System.IEquatable<NetworkUserInfo>
    {
        public int HeadPart, BodyPart, LeftArmPart, RightArmPart, LegsPart;
        public int RoundWins;
        public PlayerConnectionPayload ConnectionPayload;

        public ulong ClientID
        {
            get
            {
                return ConnectionPayload.ClientID;
            }
            set
            {
                ConnectionPayload.ClientID = value;
            }
        }

        public NetworkUserInfo(PlayerConnectionPayload connection)
        {
            HeadPart = -1;
            BodyPart = -1;
            LeftArmPart = -1;
            RightArmPart = -1;
            LegsPart = -1;
            ConnectionPayload = connection;
            RoundWins = 0;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref HeadPart);
            serializer.SerializeValue(ref BodyPart);
            serializer.SerializeValue(ref LeftArmPart);
            serializer.SerializeValue(ref RightArmPart);
            serializer.SerializeValue(ref LegsPart);
            serializer.SerializeValue(ref ConnectionPayload);
        }

        public bool Equals(NetworkUserInfo other)
        {
            return
            HeadPart == other.HeadPart &&
            BodyPart == other.BodyPart &&
            LeftArmPart == other.LeftArmPart &&
            RightArmPart == other.RightArmPart &&
            LegsPart == other.LegsPart &&
            ConnectionPayload.Equals(other.ConnectionPayload);
        }
    }

    public struct PlayerConnectionPayload : INetworkSerializable, System.IEquatable<PlayerConnectionPayload>
    {
        public ulong ClientID;
        public Unity.Collections.FixedString32Bytes Username;
        public int LobbyID;

        public PlayerConnectionPayload(ulong clientID, string username, int lobbyID)
        {
            ClientID = clientID;
            Username = username;
            LobbyID = lobbyID;
        }

        public bool Equals(PlayerConnectionPayload other)
        {
            return ClientID == other.ClientID &&
                    Username == other.Username &&
                    LobbyID == other.LobbyID;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientID);
            serializer.SerializeValue(ref Username);
            serializer.SerializeValue(ref LobbyID);
        }
    }
}