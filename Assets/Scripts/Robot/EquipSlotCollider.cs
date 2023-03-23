using Contaquest.Metaverse.Behaviours;
using Contaquest.Metaverse.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse.Robot
{
    public class EquipSlotCollider : MonoBehaviour
    {
        public RobotDefinition robotDefinition;

        [SerializeField]
        private bool overwriteEquipSlot = false;
        [SerializeField][ShowIf("overwriteEquipSlot")]
        private EquipSlot equipSlot;

        public void Initialize(RobotDefinition robotDefinition)
        {
            this.robotDefinition = robotDefinition;
        }

        public EquipSlot GetTargetSlot(RobotPartBehaviour robotPartBehaviour)
        {
            if(overwriteEquipSlot)
            {
                //Debug.Log($"{gameObject.name} Overwriting. returning: {equipSlot}");
                return equipSlot;
            }
            else
            {
                //Debug.Log($"{gameObject.name} using robotpartdata. returning: {robotPartBehaviour.equippableData.robotPartData.equipSlot}");

                return robotPartBehaviour.equippableData.robotPartData.equipSlot;
            }
        }
    }
}
