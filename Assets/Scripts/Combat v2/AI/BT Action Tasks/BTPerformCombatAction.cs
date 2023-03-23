using UnityEngine;
using NodeCanvas.Framework;
using System.Linq;

namespace Contaquest.Metaverse.Combat2
{
    public class BTPerformCombatAction : ActionTask
    {
        public ActionType actionType;
        public float minEnergyCost, maxEnergyCost;
        public BBParameter<CombatRobotController> combatRobotController;
        private float timeToWait;
        private CombatAction combatAction;

        //Called only the first time the action is executed and before anything else.
        protected override string OnInit()
        {
            CombatAction[] combatActions = combatRobotController.value.GetCombatActionsByType(actionType);
            combatAction = combatActions.FirstOrDefault((combatAction) => combatAction.energyCost < maxEnergyCost && combatAction.energyCost > minEnergyCost);
            timeToWait = combatAction.targetClipLength;
            return null;
        }

        //Called once when the action is executed.
        protected override void OnExecute()
        {
            int currentEnergy = combatRobotController.value.energy.Value;

            float requiredCharge = combatAction.energyCost;
            if (requiredCharge < currentEnergy)
            {
                combatRobotController.value.RegisterUserIpnut(combatAction);
            }
            else
            {
                EndAction(false);
            }
        }

        //Called every frame while the action is running.
        protected override void OnUpdate()
        {
            if (elapsedTime > timeToWait)
                EndAction(true);
        }
    }
}