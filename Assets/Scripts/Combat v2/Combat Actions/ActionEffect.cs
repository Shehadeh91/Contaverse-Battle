using System.Collections;
using UnityEngine;
using Contaquest.Metaverse.Behaviours;

namespace Contaquest.Metaverse.Combat2.Effects
{
    public abstract class ActionEffect : ScriptableObject
    {
        public int effectPriority = 0;

        public virtual bool IsPerformingServerSideCode()
        {
            return false;
        }
        public virtual bool CheckEffectSuccess(CombatActionContext context)
        {
            return true;
        }
        public virtual void PerformPreTurnEffect(CombatActionContext context)
        {

        }
        public virtual void PerformTurnEffect(CombatActionContext context)
        {
            
        }
        public virtual void PerformPostTurnEffect(CombatActionContext context)
        {
            
        }

        //Use this only for visual stuff or client side visuals. Any code in here should not affect the gameplay whatsoever 
        public virtual IEnumerator GetEnumerator(CombatActionContext context)
        {
            yield return null;
        }

        public virtual float GetTotalDamage(CombatActionContext context)
        {
            return 0;
        }
        public virtual float GetTotalKnockback(CombatActionContext context)
        {
            return 0;
        }
    }
}