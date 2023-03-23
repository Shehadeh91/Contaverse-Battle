using UnityEngine;

namespace Contaquest.Metaverse.Combat2
{
    [System.Serializable]
    public struct ArenaPosition
    {
        public int laneIndex;
        public float lanePosition;
        [SerializeField] private bool flippedLookDirection;
        public int LookDirection
        {
            get
            {
                if(flippedLookDirection)
                    return -1;
                else
                    return 1;
            }
        }

        public ArenaPosition(int laneIndex, float lanePosition)
        {
            this.laneIndex = laneIndex;
            this.lanePosition = lanePosition;
            flippedLookDirection = false;
        }
        public ArenaPosition(int laneIndex, float lanePosition, bool lookDirection)
        {
            this.laneIndex = laneIndex;
            this.lanePosition = lanePosition;
            this.flippedLookDirection = lookDirection;
        }
    }
}