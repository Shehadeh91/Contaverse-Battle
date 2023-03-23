using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Mobile.Input
{
    public class TouchInputSwipeDetector : MonoBehaviour
    {
        [SerializeField]
        [ListDrawerSettings(ListElementLabelName = "directionName", Expanded = true)]
        private TouchInputSwipe[] touchInputSwipes;

        public DirectionUnityEvent onHover = new DirectionUnityEvent();
        public DirectionUnityEvent onSwipeSuccess = new DirectionUnityEvent();

        public void OnStartTouch(TouchInputAction touchInputAction)
        {
            //Debug.Log("Started touch");
            touchInputAction.OnPositionChanged += OnPositionChanged;
        }
        public void OnPositionChanged(TouchInputAction touchInputAction)
        {
            foreach (var touchInputSwipe in touchInputSwipes)
            {
                bool evaluationSuccess = touchInputSwipe.EvaluateSwipe(touchInputAction);
                if(evaluationSuccess)
                {
                    onHover?.Invoke(touchInputSwipe.direction);
                    break;
                }
            }
        }
        public void OnEndTouch(TouchInputAction touchInputAction)
        {
            touchInputAction.OnPositionChanged -= OnPositionChanged;

            foreach (var touchInputSwipe in touchInputSwipes)
            {
                bool evaluationSuccess = touchInputSwipe.EvaluateSwipe(touchInputAction);
                if(evaluationSuccess)
                {
                    Debug.Log("Ended touch");
                    onSwipeSuccess?.Invoke(touchInputSwipe.direction);
                    break;
                }
            }
        }
    }
}
