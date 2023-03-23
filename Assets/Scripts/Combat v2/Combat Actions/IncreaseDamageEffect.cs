using System.Runtime;
using Contaquest.Metaverse.Behaviours;
using UnityEngine;
using Contaquest.Metaverse.Data;

namespace Contaquest.Metaverse.Combat2.Effects
{
    [CreateAssetMenu(fileName = "Increase Damage", menuName = "Combat2/Effects/Increase Damage")]
    public class IncreaseDamageEffect : ActionEffect
    {
        [SerializeField] private float dealtDamageAddition;
        [SerializeField] private float dealtDamageMultiplier;
        public override void PerformPreTurnEffect(CombatActionContext context)
        {
            context.Source.dealtDamageAddition += this.dealtDamageAddition;
            context.Source.dealtDamageMultiplier += this.dealtDamageMultiplier;
        }
    }
}