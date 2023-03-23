using UnityEngine;
using System.Collections.Generic;
using Contaquest.Metaverse.Robot;
using Contaquest.Metaverse.Data;
using Contaquest.Metaverse.Rooms;
using TMPro;
using Contaquest.Metaverse.Robot.Customization;
using Sirenix.OdinInspector;

namespace Contaquest.UI.Customization
{

    /// <summary>
    /// Oh my fucking god I hate this implementation
    /// </summary>
    public class RobotDefinitionStatsGetter : MonoBehaviour
    {
        [ShowInInspector] RobotDefinition robotDefinition;

        [SerializeField] private RobotStatsUIController robotStatsUIController;
        [SerializeField] private Camera cam1;
        [SerializeField] private Camera cam2;

        private void OnDestroy()
        {
            robotDefinition?.onDefinitionPartsChanged?.RemoveListener((equipSlot) => UpdateRobotDefinitionAfterChange());
        }

        public void Initialize()
        {
            robotDefinition = RoomManager.Instance.robotController.robotDefinition;
            //Debug.Log("Initializing");
            robotDefinition.onDefinitionPartsChanged?.AddListener((equipSlot) => UpdateRobotDefinitionAfterChange());
        }

        public void UpdateRobotDefinitionAfterChange()
        {
            if(robotDefinition == null)
            {
                Debug.LogWarning("RobotDefinition is null");
                return;
            }
            //Debug.LogWarning("Definition parts changed");
            RobotStats robotStats = robotDefinition.robotStatsCollection.robotStats;
            // robotStats.PrintDictionary();
            robotStatsUIController.DisplayRobotStats(robotStats);
            cam1?.Render();
            cam2?.Render();
        }
    }
}
