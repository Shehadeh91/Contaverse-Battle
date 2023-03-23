using System.Collections;
using Contaquest.Metaverse.Behaviours;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse.Combat2.Effects
{
    public abstract class TimedEffect : ActionEffect
    {
        [SerializeField] private bool useNormalizedTime;
        [SerializeField] private float time;

        public override IEnumerator GetEnumerator(CombatActionContext context)
        {
            float waitTime = time;

            if (useNormalizedTime)
            {
                waitTime = context.CombatAction.targetClipLength * time;
            }

            yield return new WaitForSeconds(waitTime);

            EffectTrigger(context);
        }

        protected abstract void EffectTrigger(CombatActionContext context);
    }
}
