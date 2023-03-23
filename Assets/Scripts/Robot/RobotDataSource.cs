using System;
using System.Collections.Generic;
using System.Linq;
using Contaquest.Metaverse.Data;
using Contaquest.Metaverse.Robot;
using UnityEngine;

namespace Contaquest.Metaverse
{
    public class RobotDataSource : MonoBehaviour
    {
        protected RobotData robotData;
        protected bool isLoaded;

        public virtual RobotData GetRobotData()
        {
            if (robotData == null)
            {
                Debug.LogError("The robotData has not been assigned for some reason");
            }
            foreach (var equippableData in robotData.equippableDatas)
            {
                if(equippableData == null || equippableData.robotPartData == null)
                {
                    Debug.LogError("The robot equippables have not been assigned for some reason");
                    return null;
                }
            }
            return robotData;
        }

        public virtual void LoadRobotData()
        {
            // Debug.Log("Loading Robot data");
            if (isLoaded)
                return;
            isLoaded = true;
        }
    }
}
