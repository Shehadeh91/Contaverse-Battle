using UnityEngine;
using System.Collections.Generic;
using Contaquest.Metaverse.Robot;
using Contaquest.Metaverse.Data;
using Contaquest.Metaverse.Robot.Customization;
using TMPro;
using Contaquest.Metaverse.Behaviours;

namespace Contaquest.UI.Customization
{
    public class RobotPartStatsGetter : MonoBehaviour
    {
        [SerializeField] private RobotStatsUIController robotStatsUIController;
        [SerializeField] private CustomizationSelector customizationSelector;
        [SerializeField] private TextMeshProUGUI robotPartName;
        [SerializeField] private TextMeshProUGUI noPartsText;
        [SerializeField] private Camera cam;

        public void UpdateRobotStatsAfterChange()
        {
            RobotPartBehaviour robotPartBehaviour = customizationSelector.selectedPart;
            if(robotPartBehaviour == null)
            {
                NoParts();
                return;
            }

            robotStatsUIController.gameObject.SetActive(true);
            robotPartName.gameObject.SetActive(true);
            noPartsText.gameObject.SetActive(false);

            RobotPart robotPart = robotPartBehaviour.equippableData.robotPartData;

            robotPartName.text = robotPart.nftData.name;

            RobotStats robotStats = robotPartBehaviour.robotStats;
            robotStatsUIController.DisplayRobotStats(robotStats);
            cam?.Render();
        }

        public void NoParts()
        {
            robotStatsUIController.gameObject.SetActive(false);
            robotPartName.gameObject.SetActive(false);
            noPartsText.gameObject.SetActive(true);
        }
    }
}
