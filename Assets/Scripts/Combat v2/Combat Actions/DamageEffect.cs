using Contaquest.Metaverse.Behaviours;
using UnityEngine;
using Contaquest.Metaverse.Data;

namespace Contaquest.Metaverse.Combat2.Effects
{
    [CreateAssetMenu(fileName = "Damage", menuName = "Combat2/Effects/Damage")]
    public class DamageEffect : TimedEffect
    {
        [Tooltip("Use this for piercing attacks")]
        [SerializeField] private bool ignoreBlock;
        [Tooltip("Use this for homing projectiles and lasers")]
        [SerializeField] private bool ignoreDodge;
        public override void PerformPostTurnEffect(CombatActionContext context)
        {
            if (!context.Target.isAlive)
                return;

            // Doddging and Blocking stop damage calculation. No damage will be applied
            if (ignoreDodge || context.Target.hasDodged)
                return;
            if (ignoreBlock || context.Target.isBlocking)
                return;

            // Stat Stat = robotStats.GetStat("HP");

            context.Target.ApplyDamage(GetTotalDamage(context));
        }

        protected override void EffectTrigger(CombatActionContext context)
        {
            if(context.Target.hasDodged)
            {
                context.Target.onDodged?.Invoke();
                return;
            }

            context.Target.dirtiness.Value.SetValueClamped(context.Target.dirtiness.Value.Value + 5);
            context.Target.dirtiness.Value.OnChanged();

            if (context.Target.isBlocking)
            {
                context.Target.onDamageBlocked?.Invoke();
                return;
            }
        }
        public override float GetTotalDamage(CombatActionContext context)
        {
            RobotStats robotStats = context.SourceRobotPart.robotStats;
            Stat damageStat = robotStats.GetStat("DMG");

            float damageAmount = damageStat.Value;
            damageAmount -= context.Target.receivedDamageReduction;
            damageAmount += context.Source.dealtDamageAddition;
            damageAmount *= context.Target.receivedDamageMultiplier;
            damageAmount *= context.Source.dealtDamageMultiplier;

            return damageAmount;
        }
    }
}