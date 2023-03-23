using System;
using System.Collections.Generic;
using Contaquest.Metaverse.Behaviours;
using Contaquest.Metaverse.Combat2;
using Contaquest.Metaverse.Data;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Robot
{
    [System.Serializable]
    public class RobotDefinition : iOwner
    {
        public RobotData robotData;

        public RobotStatsCollection robotStatsCollection;

        public Dictionary<int, RobotPartBehaviour> robotPartBehaviours = new Dictionary<int, RobotPartBehaviour>();
        public Dictionary<int, AttachmentPoint> attachmentPoints = new Dictionary<int, AttachmentPoint>();

        [Tooltip("this gets invoked when robot parts get swapped out")]
        public EquipSlotUnityEvent onDefinitionPartsChanged = new EquipSlotUnityEvent();

        public RobotDefinition(RobotData robotData)
        {
            this.robotData = robotData;
            RobotPart[] robotParts = robotData.equippableDatas.Select((equippableData) => equippableData.robotPartData).ToArray();
            //Debug.Log("amount of robotparts: " + robotParts.Length);
            robotStatsCollection = new RobotStatsCollection(robotParts);
        }

        public void AddAttachmentPoints(AttachmentPoint[] attachmentPoints)
        {
            foreach (var attachmentPoint in attachmentPoints)
            {
                int hashCode = attachmentPoint.attachmentPointName.GetHashCode();
                if (!this.attachmentPoints.ContainsKey(hashCode))
                    this.attachmentPoints.Add(hashCode, attachmentPoint);
            }
        }
        public void RemoveAttachmentPoints(AttachmentPoint[] attachmentPoints)
        {
            foreach (var attachmentPoint in attachmentPoints)
            {
                int hashCode = attachmentPoint.attachmentPointName.GetHashCode();
                this.attachmentPoints.Remove(hashCode);
            }
        }

        public void OnUpdate()
        {
            if (robotPartBehaviours == null)
                return;

            foreach (KeyValuePair<int, RobotPartBehaviour> robotPartBehaviour in robotPartBehaviours)
            {
                robotPartBehaviour.Value.OnUpdate();
            }
        }
        public void OnLateUpdate()
        {
            foreach (var robotPartBehaviour in robotPartBehaviours)
            {
                robotPartBehaviour.Value.OnLateUpdate();
            }
        }

        public void EquipPart(RobotPartBehaviour robotPartBehaviour)
        {
            EquipSlot equipSlot = robotPartBehaviour.equippableData.equipSlot;
            if(robotPartBehaviour == null)
            {
                Debug.LogError("Trying to equip null??");
                return;
            }

            if (robotPartBehaviours.TryGetValue((int)equipSlot, out RobotPartBehaviour oldRobotPartBehaviour))
            {
                robotStatsCollection.RemoveRobotStats(oldRobotPartBehaviour.robotStats);
                oldRobotPartBehaviour.Remove();
                robotPartBehaviours[(int)equipSlot] = robotPartBehaviour;
            }
            else
            {
                robotPartBehaviours.Add((int)equipSlot, robotPartBehaviour);
            }

            robotData.equippableDatas[(int)equipSlot] = robotPartBehaviour.equippableData;

            robotStatsCollection.AddRobotStats(robotPartBehaviour.robotStats);

            onDefinitionPartsChanged?.Invoke(equipSlot);
        }

        public void ReAssemble()
        {
            foreach (var robotPartBehaviour in robotPartBehaviours)
            {
                robotPartBehaviour.Value.ReAssemble();
            }
        }

        public CombatAction GetCombatAction(EquipSlot equipSlot)
        {
            if (robotPartBehaviours.TryGetValue((int)equipSlot, out RobotPartBehaviour robotPartBehaviour))
            {
                return robotPartBehaviour.GetCombatAction();
            }

            return null;
        }
        public List<CombatActionContext> GetAllCombatActions(CombatRobotController source, CombatRobotController target)
        {
            List<CombatActionContext> combatActionContexts = new List<CombatActionContext>();
            int combatActionIndex = 0;
            foreach (var robotPartBehaviour in robotPartBehaviours)
            {
                CombatAction combatAction = robotPartBehaviour.Value.GetCombatAction();
                if(combatAction == null)
                    continue;
                EquipSlot equipSlot = robotPartBehaviour.Value.equippableData.equipSlot;
                CombatActionContext combatActionContext = new CombatActionContext(equipSlot, combatActionIndex, combatAction, source, target);

                combatActionContexts.Add(combatActionContext);
                combatActionIndex++;
            }
            return combatActionContexts;
        }
        public RobotPartBehaviour GetPartBehaviour(EquipSlot equipSlot)
        {
            if (robotPartBehaviours.TryGetValue((int)equipSlot, out RobotPartBehaviour robotPartBehaviour))
            {
                return robotPartBehaviour;
            }
            return null;
        }

        public void SaveRobotDataToSlot(int saveSlot)
        {
            SaveManager.Instance.SaveRobotData(robotData, saveSlot);
        }

        public void SetDirtiness(float value)
        {
            // Debug.Log($"Setting Dirtiness to {value}");

            if(robotPartBehaviours == null)
                return;

            foreach (KeyValuePair<int, RobotPartBehaviour> robotPartBehaviour in robotPartBehaviours)
            {
                robotPartBehaviour.Value?.SetDirtiness(value);
            }
        }
    }
}
