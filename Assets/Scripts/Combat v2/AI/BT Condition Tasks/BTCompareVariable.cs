using UnityEngine;
using NodeCanvas.Framework;
using System.Linq;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Combat2
{
    public class BTCompareVariable : ConditionTask
    {
        public VariableType variableType;
        public NumberComparison comparisonMode;

        public BBParameter<CombatRobotController> combatRobotController;

        [Header("Value 1")]
        public bool useConstValue1;
        public float constValue1;
        public ValueSource value1Source;

        [Header("Value 2")]
        public bool useConstValue2;
        public float constValue2;
        public ValueSource value2Source;


        // protected override string OnInit()
        // {
        //     CombatAction[] combatActions = combatRobotController.value.GetCombatActionsByType(actionType);
        //     combatAction = combatActions.FirstOrDefault((combatAction) => combatAction.energyCost < maxEnergyCost && combatAction.energyCost > minEnergyCost);
        //     skip = combatAction == null;
        //     return null;
        // }

        protected override bool OnCheck()
        {
            float value1 = 0f;
            if(useConstValue1)
                value1 = constValue1;
            else
                value1 = GetValueFromRobot(GetRobotController(value1Source));

            float value2 = 0f;
            if(useConstValue2)
                value2 = constValue2;
            else
                value2 = GetValueFromRobot(GetRobotController(value2Source));
            
            return CompareNumbers(value1, value2);
        }

        private CombatRobotController GetRobotController(ValueSource valueSource)
        {
            switch (valueSource)
            {
                case ValueSource.user:
                    return combatRobotController.value;
                case ValueSource.opponent:
                    int opponentUserID = combatRobotController.value.GetOpponentUserID();
                    return combatRobotController.value.combatLobby.GetUserByID(opponentUserID).combatRobotController;
                default:
                    return null;
            }
            
        }
        private float GetValueFromRobot(CombatRobotController robot)
        {
            switch (variableType)
            {
                case VariableType.health:
                    return robot.health.Value.Value;
                case VariableType.energy:
                    return (float)robot.energy.Value;
                case VariableType.laneIndex:
                    return (float)robot.laneIndex.Value;
                case VariableType.lanePosition:
                    return robot.lanePosition.Value;
                case VariableType.turnPriority:
                    return (float)robot.turnPriorityBoost;
                default:
                    return 0;
            }
        }

        protected bool CompareNumbers(float number1, float number2)
        {
            switch (comparisonMode)
            {
                case NumberComparison.Equal:
                    return number1 == number2;
                case NumberComparison.NotEqual:
                    return number1 != number2;
                case NumberComparison.Greater:
                    return number1 > number2;
                case NumberComparison.Less:
                    return number1 < number2;
                case NumberComparison.GEqual:
                    return number1 >= number2;
                case NumberComparison.LEqual:
                    return number1 <= number2;
                default:
                    return false;
            }
        }
        public enum ValueSource
        {
            user,
            opponent
        }
        public enum VariableType
        {
            health,
            energy,
            laneIndex,
            lanePosition,
            turnPriority
        }
    }
}