using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Combat2
{
    public class ArenaDefinition : MonoBehaviour
    {
        [TabGroup("Properties")] [SerializeField] private int middleLane = 3;
        [TabGroup("Properties")] [SerializeField] private float middlePosition = 5f;
        [TabGroup("Properties")] [SerializeField] private ArenaPosition[] startPositions;

        [TabGroup("References")] [SerializeField] private Lane[] lanes;
        [TabGroup("References")] [ShowInInspector, ReadOnly] private CombatLobby combatLobby;

        public void Initialize(CombatLobby combatLobby)
        {
            this.combatLobby = combatLobby;
        }

        public ArenaPosition GetStartPosition(int userIndex)
        {
            return startPositions[userIndex];
        }
        public float GetDistanceToMiddle(float lanePosition)
        {
            float relativePosition = lanePosition - middlePosition;
            return Mathf.Abs(relativePosition);
        }
        public bool IsPositionInArena(int laneIndex, float lanePosition)
        {
            if(laneIndex < 0 || laneIndex >= lanes.Length)
                return false;

            return lanes[laneIndex].IsProgressInsideLane(lanePosition);
        }
        public bool IsPositionInArena(ArenaPosition arenaPosition)
        {
            return IsPositionInArena(arenaPosition.laneIndex, arenaPosition.lanePosition);
        }

        public Vector3 GetPositionInArena(int laneIndex, float lanePosition)
        {
            return lanes[laneIndex].GetPositionOnLane(lanePosition);
        }
        public Vector3 GetPositionInArena(ArenaPosition arenaPosition)
        {
            return GetPositionInArena(arenaPosition.laneIndex, arenaPosition.lanePosition);
        }

        public Vector3 GetForwardInArena(int laneIndex, float lanePosition)
        {
            return lanes[laneIndex].GetForwardOnLane(lanePosition);
        }
        public Vector3 GetForwardInArena(ArenaPosition arenaPosition)
        {
            return GetForwardInArena(arenaPosition.laneIndex, arenaPosition.lanePosition);
        }
    }
}