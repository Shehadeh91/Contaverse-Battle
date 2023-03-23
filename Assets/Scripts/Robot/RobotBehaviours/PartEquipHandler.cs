using System;
using Contaquest.Metaverse.Behaviours;
using Contaquest.Mobile.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Robot
{
    public class PartEquipHandler : MonoBehaviour
    {
        [TabGroup("Properties")]
        [SerializeField]
        private LayerMask equipLayerMask;

        [TabGroup("References")]
        [SerializeField]
        private RobotPartBehaviour robotPartBehaviour;
        private EquipSlotCollider selectedEquipSlotCollider;

        [TabGroup("Events")]
        [SerializeField]
        private UnityEvent onEquipFailed;

        public bool TryEquipPart()
        {
            Debug.Log("Releasing part");
            Camera activeCamera = ReferencesManager.Instance.MainCamera;
            //Send out ray
            Ray ray = activeCamera.ViewportPointToRay(TouchInputManager.Instance.primaryInputTouchAction.currentPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, equipLayerMask))
            {
                Debug.Log("hit successful");

                selectedEquipSlotCollider = hit.collider.GetComponent<EquipSlotCollider>();

                if (selectedEquipSlotCollider != null)
                {
                    EquipRobotPart(selectedEquipSlotCollider);
                    return true;
                }
                return false;
            }
            else
            {
                Debug.Log("hit unsuccessful");
                return false;
            }
        }

        private void EquipRobotPart(EquipSlotCollider equipSlotCollider)
        {
            if (robotPartBehaviour is null)
            {
                Debug.LogError("Please assign a robotpart");
                return;
            }
            transform.parent = null;
            EquipSlot equipSlot = equipSlotCollider.GetTargetSlot(robotPartBehaviour);
            robotPartBehaviour.equippableData.equipSlot = equipSlot;
            Debug.LogWarning($" Part equipslot = {robotPartBehaviour.equippableData.equipSlot}");


            RobotDefinition robotDefinition = equipSlotCollider.robotDefinition;
            robotPartBehaviour.owner = robotDefinition;
            RobotPartBehaviour oldRobotPartBehaviour = robotDefinition.robotPartBehaviours[(int)equipSlot];

            robotDefinition.EquipPart(robotPartBehaviour);

            robotPartBehaviour.InitializeRobotDefinition(robotDefinition);
            //SaveManager.Instance.SaveRobotData(robotDefinition.robotData, 1);
            robotPartBehaviour.StartGravitateTowardsAttachpoints(oldRobotPartBehaviour.Disassemble);
        }
    }
}
