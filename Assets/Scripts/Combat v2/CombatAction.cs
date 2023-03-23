using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Contaquest.Metaverse.Combat2.Effects;

namespace Contaquest.Metaverse.Combat2
{
    [CreateAssetMenu(fileName = "Combat Action", menuName = "Combat2/Combat Action")]
    public class CombatAction : ScriptableObject
    {
        [TabGroup("Properties")] public ActionType actionType;
        [TabGroup("Properties")] public string actionName;
        [TabGroup("Properties")] public int energyCost = 0;
        [TabGroup("References")] public Sprite actionImage;
        [TabGroup("References")] public bool showDamageAmountUI;
        [TabGroup("References")] public bool showKnockBackAmountUI;

        [TabGroup("Animation")] [SerializeField] private string animatorStateName;
        [TabGroup("Animation")] [SerializeField, ReadOnly] internal int animationStateHash;
        [TabGroup("Animation")] [SerializeField, ReadOnly] internal bool mirrorAnimation;
        [TabGroup("Animation")] [SerializeField] private AnimationClip animationClip;
        [TabGroup("Animation")] [SerializeField, ReadOnly] public float clipLength = 0;
        [TabGroup("Animation")] public float targetClipLength = 1.0f;
        [TabGroup("Animation")] public float transitionDuration = 0.05f;

        [TabGroup("Effects")] [InlineEditor] public ActionEffect[] effects;

        public float CalculatedSpeed => clipLength / targetClipLength;

#if UNITY_EDITOR
        protected void OnValidate()
        {
            if (!string.IsNullOrEmpty(animatorStateName))
            {
                animationStateHash = Animator.StringToHash(animatorStateName);
            }
            
            if (animationClip)
                clipLength = animationClip.length;
        }
#endif
        public bool VerifyAction(CombatActionContext context)
        {
            if(context.Source.GetEnergy() < energyCost)
                return false;
                
            foreach (ActionEffect effect in effects)
            {
                bool effectSuccess = effect.CheckEffectSuccess(context);
                if(!effectSuccess)
                    return false;
            }
            return true;
        }

        public ActionEffect[] GetActionEffects()
        {
            return effects;
        }
        public void PlayVisuals(CombatActionContext context)
        {
            // Here animations will be triggered along with possible SFX etc
            context.Source.combatAnimator.Play(context);
        }
        public List<IEnumerator> GetEnumerators(CombatActionContext context)
        {
            if (effects == null)
                return new List<IEnumerator>();
            return effects.Select((arg) => arg.GetEnumerator(context)).ToList();
        }

        public float GetTotalDamage(CombatActionContext context)
        {
            return effects.Sum((actionEffect) => actionEffect.GetTotalDamage(context));
        }
        public float GetTotalKnockback(CombatActionContext context)
        {
            return effects.Sum((actionEffect) => actionEffect.GetTotalKnockback(context));
        }

        public virtual CombatAction GetCopy()
        {
            CombatAction returnCombatAction = ScriptableObject.CreateInstance<CombatAction>();

            returnCombatAction.energyCost = energyCost;
            returnCombatAction.animationStateHash = animationStateHash;
            returnCombatAction.clipLength = clipLength;
            returnCombatAction.targetClipLength = targetClipLength;
            returnCombatAction.transitionDuration = transitionDuration;
            returnCombatAction.effects = effects;
            returnCombatAction.actionImage = actionImage;
            returnCombatAction.actionName = actionName;

            return returnCombatAction;
        }
    }
}