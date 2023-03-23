using UnityEngine;
using Contaquest.Metaverse.Behaviours;
using Unity.Netcode;

namespace Contaquest.Metaverse.Combat2
{
    public struct CombatActionContext : INetworkSerializable
    {
        public EquipSlot sourceEquipSlot;
        public int combatActionIndex;
        //public int sourceUserID;
        //public int targetUserID;
        public int lobbyID;
        public PlayerConnectionPayload sourceConnection;
        public PlayerConnectionPayload targetConnection;

        public CombatActionContext(EquipSlot sourceEquipSlot, int combatActionIndex, int lobbyID, PlayerConnectionPayload sourceConnection, PlayerConnectionPayload targetConnection) : this()
        {
            this.sourceEquipSlot = sourceEquipSlot;
            this.combatActionIndex = combatActionIndex;
            this.lobbyID = lobbyID;
            this.sourceConnection = sourceConnection;
            this.targetConnection = targetConnection;
        }

        public CombatActionContext(EquipSlot sourceEquipSlot, int combatActionIndex, CombatAction combatAction, CombatRobotController source, CombatRobotController target): this()
        {
            this.sourceEquipSlot = sourceEquipSlot;
            this.combatActionIndex = combatActionIndex;
            this.combatAction = combatAction;
            this.source = source;
            this.target = target;
        }
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref sourceConnection);
            serializer.SerializeValue(ref targetConnection);
            serializer.SerializeValue(ref sourceEquipSlot);
            serializer.SerializeValue(ref lobbyID);
            serializer.SerializeValue(ref combatActionIndex);
        }

        private CombatAction combatAction;
        public CombatAction CombatAction
        {
            get
            {
                if(combatAction == null)
                    combatAction = Source.GetCombatActionByIndex(combatActionIndex);
                return combatAction;
            }
        }
        private CombatRobotController source;
        public CombatRobotController Source
        {
            get
            {
                if(source == null)
                    source = Lobby.GetUserByNetworkInfo(sourceConnection).combatRobotController;;
                return source;
            }
        }
        private RobotPartBehaviour sourceRobotPart;
        public RobotPartBehaviour SourceRobotPart
        {
            get
            {
                if(sourceRobotPart == null)
                    sourceRobotPart = Source.robotDefinition.GetPartBehaviour(sourceEquipSlot);
                return sourceRobotPart;
            }
        }
        private CombatRobotController target;
        public CombatRobotController Target
        {
            get
            {
                if(target == null)
                    target = Lobby.GetUserByNetworkInfo(targetConnection).combatRobotController;
                return target;
            }
        }
        private CombatLobby lobby;
        public CombatLobby Lobby
        {
            get
            {
                if(lobby == null)
                    lobby = NetworkConnection.Singleton.FindLobby(lobbyID);

                return lobby;
            }
        }
        private ArenaDefinition arenaDefinition;

        

        public ArenaDefinition ArenaDefinition
        {
            get
            {
                if(arenaDefinition == null)
                    arenaDefinition = Lobby.arenaDefinition;

                return arenaDefinition;
            }
        }
    }
}