using System.Runtime;
using Contaquest.Metaverse.Behaviours;
using UnityEngine;
using Contaquest.Metaverse.Data;

namespace Contaquest.Metaverse.Combat2.Effects
{
    [CreateAssetMenu(fileName = "Reduce Damage", menuName = "Combat2/Effects/Reduce Damage")]
    public class ReduceDamageEffect : ActionEffect
    {
        [SerializeField] private float receivedDamageReduction;
        [SerializeField] private float receivedDamageMultiplier;
        public override void PerformPreTurnEffect(CombatActionContext context)
        {
            context.Target.receivedDamageReduction += this.receivedDamageReduction;
            context.Target.receivedDamageMultiplier += this.receivedDamageMultiplier;
        }
    }
}