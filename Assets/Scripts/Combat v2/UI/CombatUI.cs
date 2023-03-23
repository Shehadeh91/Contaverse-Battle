using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Contaquest.Metaverse.Combat2
{
    public class CombatUI : MonoBehaviour
    {
        [TabGroup("References")] [SerializeField] private Transform combatActionDisplayParent;
        [TabGroup("References")] [SerializeField] private CombatActionDisplay combatActionDisplayPrefab;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private CombatRobotController combatRobotController;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private CombatActionDisplay[] combatActionDisplays;
        public void InitializeUser(int userID)
        {
            // TODO:Get user

            // combatRobotController.
        }

        public void InitializeCombatActionsUI(List<CombatActionContext> combatActionContexts)
        {
            combatActionDisplays = new CombatActionDisplay[combatActionContexts.Count];

            for (var i = 0; i < combatActionContexts.Count; i++)
            {
                combatActionDisplays[i] = Instantiate(combatActionDisplayPrefab, combatActionDisplayParent);
                combatActionDisplays[i].InitializeUI(combatActionContexts[i], this);
            }
        }

        public void UseCombatAction(CombatActionContext combatActionContext)
        {
            combatRobotController.RegisterUserIpnut(combatActionContext);

            // combatActionDisplays[i] = Instantiate(combatActionDisplayPrefab);
            // combatActionDisplays[i].DisplayCombatAction(combatActionContext, this);
        }
    }
}