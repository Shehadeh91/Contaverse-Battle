using System;
using Contaquest.Metaverse.Combat2.Effects;

namespace Contaquest.Metaverse.Combat2
{
    public class ActionEffectHandler : IComparable<ActionEffectHandler>
    {
        public CombatActionContext context;
        public ActionEffect effect;
        public ActionEffectHandler(CombatActionContext context, ActionEffect effect)
        {
            this.context = context;
            this.effect = effect;
        }

        public virtual void PerformPreTurnEffect()
        {
            effect.PerformPreTurnEffect(context);
        }
        public virtual void PerformTurnEffect()
        {
            effect.PerformTurnEffect(context);
        }
        public virtual void PerformPostTurnEffect()
        {
            effect.PerformPostTurnEffect(context);
        }
        public int GetCurrentPriority()
        {
            return context.Source.turnPriorityBoost + effect.effectPriority;
        }
        public int CompareTo(ActionEffectHandler compareEffect)
        {
            // A null value means that this object is greater.
            if (compareEffect == null)
                return 1;
            else
                return this.GetCurrentPriority().CompareTo(compareEffect.GetCurrentPriority());
        }
    }
}