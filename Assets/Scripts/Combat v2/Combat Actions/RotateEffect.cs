using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Combat2.Effects
{
    [CreateAssetMenu(fileName = "Rotate", menuName = "Combat2/Effects/Rotate")]
    public class RotateEffect : ActionEffect
    {
        [SerializeField] private Vector3 axis = Vector3.up;
        private bool useActionTime;
        [Tooltip("The duration over which the rotation will be applied")]
        [HideIf("useActionTime")] [SerializeField] private float rotateTime = 1f;
        [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 180);


        public override IEnumerator GetEnumerator(CombatActionContext context)
        {
            float timeElapsed = 0f;
            float maxTime = useActionTime ? context.CombatAction.clipLength : rotateTime;
            bool isRotating = true;

            while (isRotating)
            {
                float startValue = animationCurve.Evaluate(timeElapsed / maxTime);

                timeElapsed += Time.deltaTime;
                if(timeElapsed >= maxTime)
                {
                    timeElapsed = maxTime;
                    isRotating = false;
                }

                float endValue = animationCurve.Evaluate(timeElapsed / maxTime);
                float deltaValue = endValue - startValue;

                Quaternion deltaRotation = Quaternion.AngleAxis(deltaValue, axis);
                context.Source.Rotate(deltaRotation);
                yield return null;
            }
        }
    }
}