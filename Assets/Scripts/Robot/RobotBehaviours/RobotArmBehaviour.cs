using System;
using Contaquest.Metaverse.Data;
using Contaquest.Metaverse.Robot;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse.Behaviours
{
    public class RobotArmBehaviour : RobotPartBehaviour
    {
        [SerializeField, ReadOnly] private string sideSuffix = "";

        public override void InitializeRobotDefinition(RobotDefinition robotDefinition)
        {
            //Debug.Log($"Initializing arm, equipslot = {equippableData.equipSlot}");
            UpdateSideSuffix();

            base.InitializeRobotDefinition(robotDefinition);
        }

        //Never call this function twice, it += the L or R indicator
        public void UpdateSideSuffix()
        {
            if (equippableData.equipSlot == EquipSlot.LeftArm)
                sideSuffix = "L";
            else if (equippableData.equipSlot == EquipSlot.RightArm)
                sideSuffix = "R";
            else
                sideSuffix = "L";

            foreach (var subPart in subParts)
            {
                subPart.InitializeSide(sideSuffix);
            }

            foreach (var attachmentPoint in attachmentPoints)
            {
                attachmentPoint.Initialize(sideSuffix);
            }
        }
    }
}
