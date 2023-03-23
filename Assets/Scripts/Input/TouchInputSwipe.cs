using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Mobile.Input
{
    [System.Serializable]
    public class TouchInputSwipe
    {
#if UNITY_EDITOR
        public string directionName;
#endif
        [SerializeField] private bool useDistance = false;
        [SerializeField] private bool useDuration = false;

        [SerializeField] [ShowIf("useDistance")] private float minSwipeDistance, maxSwipeDistance;
        [SerializeField] [ShowIf("useDuration")] private float minSwipeDuration, maxSwipeDuration;

        public Direction direction;
        [SerializeField] private Vector2 swipeDirection;
        [SerializeField] [Range(0f, 1f)] private float matchPercent = 0.707f;

        public bool EvaluateSwipe(TouchInputAction touchInputAction)
        {
            Vector2 direction = ScreenUtility.FixViewportDistanceHeight(touchInputAction.Direction);

            if (useDuration)
            {
                if(touchInputAction.Duration < minSwipeDuration || touchInputAction.Duration > maxSwipeDuration)
                    return false;
            }
            if(useDistance)
            {
                float sqrDistance = direction.sqrMagnitude;
                if (sqrDistance < minSwipeDistance * minSwipeDistance || sqrDistance > maxSwipeDistance * maxSwipeDistance)
                    return false;
            }

            direction.Normalize();

            if(matchPercent <= 0.05f)
                return true;

            float dotProduct = Vector2.Dot(swipeDirection, direction);
            bool isDirectionMatch = dotProduct > matchPercent;

            return isDirectionMatch;
        }
    }
}
