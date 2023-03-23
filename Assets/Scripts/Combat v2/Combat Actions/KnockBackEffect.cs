using System.Collections;
using UnityEngine;
using Contaquest.Metaverse.Behaviours;
using Contaquest.Metaverse.Data;

namespace Contaquest.Metaverse.Combat2.Effects
{
    [CreateAssetMenu(fileName = "KnockBack", menuName = "Combat2/Effects/KnockBack")]
    public class KnockBackEffect : ActionEffect
    {
        [Tooltip("Use this for piercing attacks")]
        [SerializeField] private bool ignoreBlock;
        [Tooltip("Use this for homing projectiles and lasers")]
        [SerializeField] private bool ignoreDodge;
        [SerializeField] private float knockbackAmount;
        [SerializeField] private float moveTime = 1f;
        [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public override void PerformPostTurnEffect(CombatActionContext context)
        {
            if (ignoreDodge || context.Target.hasDodged)
                return;
            if (ignoreBlock || context.Target.isBlocking)
                return;
            ArenaPosition arenaPosition = context.Source.GetArenaPosition();

            context.Source.lanePosition.Value -= GetTotalKnockback(context) * arenaPosition.LookDirection;
        }

        public override IEnumerator GetEnumerator(CombatActionContext context)
        {
            if (ignoreDodge || context.Target.hasDodged || ignoreBlock || context.Target.isBlocking)
                yield break;

            ArenaPosition arenaPosition = context.Source.GetArenaPosition();
            arenaPosition.lanePosition -= GetTotalKnockback(context) * arenaPosition.LookDirection;

            Transform myTransform = context.Source.transform;
            Vector3 startPosition = myTransform.position;
            Vector3 endPosition = context.ArenaDefinition.GetPositionInArena(arenaPosition);

            float timeElapsed = 0f;
            float maxTime = moveTime;
            bool isMoving = true;

            while (isMoving)
            {
                Vector3 currentFramePosition = Vector3.Lerp(startPosition, endPosition, animationCurve.Evaluate(timeElapsed / maxTime));

                timeElapsed += Time.deltaTime;
                if(timeElapsed >= maxTime)
                {
                    timeElapsed = maxTime;
                    isMoving = false;
                }

                Vector3 targetFramePosition = Vector3.Lerp(startPosition, endPosition, animationCurve.Evaluate(timeElapsed / maxTime));
                Vector3 frameDelta = targetFramePosition - currentFramePosition;

                context.Source.Move(frameDelta);
                yield return null;
            }
        }
        public override float GetTotalKnockback(CombatActionContext context)
        {
            return knockbackAmount;
        }
    }
}