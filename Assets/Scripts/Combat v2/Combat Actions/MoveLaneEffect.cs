using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Combat2.Effects
{
    [CreateAssetMenu(fileName = "Move Lane", menuName = "Combat2/Effects/Move Lane")]
    public class MoveLaneEffect : ActionEffect
    {
        public int laneDelta = 0;
        public float positionDelta = 0f;
        private bool useActionTime;
        [HideIf("useActionTime")] [SerializeField] private float moveTime = 1f;
        [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public override bool CheckEffectSuccess(CombatActionContext context)
        {
            ArenaPosition arenaPosition = context.Source.GetArenaPosition();
            arenaPosition.laneIndex += laneDelta * arenaPosition.LookDirection;
            arenaPosition.lanePosition += positionDelta * arenaPosition.LookDirection;
            return context.ArenaDefinition.IsPositionInArena(arenaPosition);
        }

        public override void PerformTurnEffect(CombatActionContext context)
        {
            ArenaPosition arenaPosition = context.Source.GetArenaPosition();

            context.Source.laneIndex.Value += laneDelta * arenaPosition.LookDirection;
            context.Source.lanePosition.Value += positionDelta * arenaPosition.LookDirection;
        }

        //Use this only for visual stuff or client side visuals. Any code in here should not affect the gameplay whatsoever 
        public override IEnumerator GetEnumerator(CombatActionContext context)
        {
            ArenaPosition arenaPosition = context.Source.GetArenaPosition();
            arenaPosition.laneIndex += laneDelta * arenaPosition.LookDirection;
            arenaPosition.lanePosition += positionDelta * arenaPosition.LookDirection;

            Transform myTransform = context.Source.transform;
            Vector3 startPosition = myTransform.position;
            Vector3 endPosition = context.ArenaDefinition.GetPositionInArena(arenaPosition);

            float timeElapsed = 0f;
            float maxTime = useActionTime ? context.CombatAction.targetClipLength : moveTime;
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
    }
}