using System.Collections.Generic;
using System.Linq;
using Contaquest.Metaverse.Data;
using Contaquest.Metaverse.Robot;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse
{
    public class RobotDataSourceEntity : RobotDataSource
    {
        [SerializeField] [InlineEditor] private List<RobotPartCollection> robotPartCollections = new List<RobotPartCollection>();

        public override void LoadRobotData()
        {
            base.LoadRobotData();
            if (robotPartCollections is null || robotPartCollections.Count == 0)
            {
                Debug.LogError("Please assign a RobotPartCollection", gameObject);
                return;
            }

            int random = Random.Range(0, robotPartCollections.Count);
            robotData = robotPartCollections[random].GetRobotData();
        }
    }
}
