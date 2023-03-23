using UnityEngine;
using NodeCanvas.Framework;
using System.Linq;

namespace Contaquest.Metaverse.Combat2
{
    public class BTCheckAction : ConditionTask
    {
        public ActionType actionType;
        public float minEnergyCost, maxEnergyCost;
        public BBParameter<CombatRobotController> combatRobotController;
        private CombatAction combatAction;
        private bool skip;

        protected override string OnInit()
        {
            CombatAction[] combatActions = combatRobotController.value.GetCombatActionsByType(actionType);
            combatAction = combatActions.FirstOrDefault((combatAction) => combatAction.energyCost < maxEnergyCost && combatAction.energyCost > minEnergyCost);
            skip = combatAction == null;
            return null;
        }

        //This is called when the condition is checked. Override and return true or false.
        protected override bool OnCheck()
        {
            if(skip)
                return false;
            int currentEnergy = combatRobotController.value.energy.Value;

            return combatAction.energyCost >= currentEnergy;
        }
    }
}