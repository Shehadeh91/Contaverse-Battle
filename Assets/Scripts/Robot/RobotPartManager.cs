using System;
using System.Collections.Generic;
using Contaquest.Metaverse.Behaviours;
using Contaquest.Metaverse.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse.Robot
{
    /// <summary>
    /// Robot part Object pooling solution??
    /// </summary>
    public class RobotPartManager : GenericSingleton<RobotPartManager>
    {
        public List<RobotPartBehaviour> robotPartBehaviours = new List<RobotPartBehaviour>();
        public List<RobotPart> robotParts = new List<RobotPart>();

        public void CreateRobotPart(RobotPart robotPart, Action callback = null)
        {
            //robotParts.Select()
            //if()
        }
    }
}
