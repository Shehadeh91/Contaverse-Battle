using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Combat2
{
    public class CombatManager : MonoBehaviour
    {
        [TabGroup("References")] [ShowInInspector, ReadOnly] private List<CombatRobotController> robotControllers = new List<CombatRobotController>();
        [TabGroup("References")] [System.NonSerialized, ReadOnly] public List<CombatActionContext> combatActions;
        [TabGroup("State")] [SerializeField, ReadOnly] private CombatLobby combatLobby;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private List<Coroutine> runningCoroutines = new List<Coroutine>();

        public void Initialize(CombatLobby combatLobby)
        {
            this.combatLobby = combatLobby;
            robotControllers = combatLobby.combatUsers.Select((combatUser) => combatUser.combatRobotController).ToList();
            for (var i = 0; i < robotControllers.Count; i++)
            {
                robotControllers[i].turnPriorityBoost = i;
            }
        }

        public bool VerifyAllInputs(List<CombatActionContext> combatContexts)
        {
            foreach (var combatContext in combatContexts)
            {
                bool actionSuccess = combatContext.CombatAction.VerifyAction(combatContext);
                if(!actionSuccess)
                    return false;
            }
            return true;
        }

        public bool RecieveAndVerifyInput(CombatActionContext combatContext)
        {
            if (!combatContext.CombatAction.VerifyAction(combatContext))
                return false;
            combatActions.Add(combatContext);
            //check if all players have submited context and execute turn if so
            return true;
        }

        public void ExecuteTurn()
        {
            StartTurn();

            List<CombatActionContext> combatContexts = new List<CombatActionContext>();
            PerformCombatActions(combatContexts);

            EndTurn();
        }

        private void StartTurn()
        {
            
        }

        private void PerformCombatActions(List<CombatActionContext> combatContexts)
        {
            // start Performing visuals before changing any variables, otherwise the visuals may not work properly
#if !UNITY_SERVER
            PerformCombatActionVisuals(combatContexts);
#endif

            // Only perform actually gameplay changing effects on the server side, variables will get sent to clients separately
#if UNITY_SERVER
            PerformCombatActionGamePlay(combatContexts);

            UpdateClientVariables();
#endif
        }

        private void PerformCombatActionVisuals(List<CombatActionContext> combatContexts)
        {
            runningCoroutines = new List<Coroutine>();
            foreach (var combatContext in combatContexts)
            {
                combatContext.CombatAction.PlayVisuals(combatContext);
                List<IEnumerator> enumerators = combatContext.CombatAction.GetEnumerators(combatContext);
                for (int i = 0; i < enumerators.Count; i++)
                {
                    Coroutine coroutine = StartCoroutine(enumerators[i]);
                    runningCoroutines.Add(coroutine);
                }
            }
        }

        private void PerformCombatActionGamePlay(List<CombatActionContext> combatContexts)
        {
            List<ActionEffectHandler> actionEffectHandlers = new List<ActionEffectHandler>();

            foreach (var combatContext in combatContexts)
            {
                var actionEffects = combatContext.CombatAction.GetActionEffects().Where((actionEffect) => actionEffect.IsPerformingServerSideCode());
                var newHandlers = actionEffects.Select((actionEffect) => new ActionEffectHandler(combatContext, actionEffect));
                actionEffectHandlers.AddRange(newHandlers);
            }
            
            actionEffectHandlers.Sort();

            foreach (var actionEffectHandler in actionEffectHandlers)
            {
                actionEffectHandler.PerformPreTurnEffect();
            }
            foreach (var actionEffectHandler in actionEffectHandlers)
            {
                actionEffectHandler.PerformTurnEffect();
            }
            foreach (var actionEffectHandler in actionEffectHandlers)
            {
                actionEffectHandler.PerformPostTurnEffect();
            }
        }

        public void UpdateClientVariables()
        {
            // TODO: network stuff
            
        }

        public void EndTurn()
        {
            foreach(CombatRobotController robotController in robotControllers)
            {
                robotController.ResetTurnValues();

                robotController.turnPriorityBoost++;
                if(robotController.turnPriorityBoost >= robotControllers.Count)
                    robotController.turnPriorityBoost = 0;
            }
        }
    }
}