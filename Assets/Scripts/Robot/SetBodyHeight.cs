using System;
using UnityEngine;

namespace Contaquest.Metaverse.Robot
{
    public class SetBodyHeight : MonoBehaviour
    {
        private RobotDefinition robotDefinition;

        public void Initialize(RobotDefinition robotDefinition)
        {
            this.robotDefinition = robotDefinition;
        }

        public void UpdatePosition()
        {
            Vector3 pos = transform.localPosition;
            pos.y = GetFloorDistance();
            transform.localPosition = pos;
        }

        public float GetFloorDistance()
        {
            if (robotDefinition == null)
                return 0f;
            float floorDistance = 0f;

            foreach (var robotPartBehaviour in robotDefinition.robotPartBehaviours)
            {
                floorDistance += robotPartBehaviour.Value.floorDistance;
            }
            return floorDistance;
        }
    }
}
