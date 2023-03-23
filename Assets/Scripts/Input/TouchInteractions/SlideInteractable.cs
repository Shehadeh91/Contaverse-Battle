using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System.Collections;

namespace Contaquest.Mobile.Input
{
    [RequireComponent(typeof(Collider))]
    public class SlideInteractable : TouchInteractable
    {
        //[TabGroup("Properties")] [SerializeField] private bool screenSpace;
        [TabGroup("Properties")] [SerializeField] private bool slideX = true;
        [TabGroup("Properties")] [SerializeField] private float slideSpeedMultiplier;

        [TabGroup("Events")] [SerializeField] private TouchInputUnityEvent onStartSlide, onEndSlide;
        [TabGroup("Events")] [SerializeField] private FloatUnityEvent deltaSwipe;

        public override void StartTouchInteraction(TouchInputAction touchInputAction)
        {
            onStartSlide?.Invoke(touchInputAction);
            touchInputAction.OnPositionChanged += UpdateSlidePosition;
            touchInputAction.onEndTouch.AddListener((arg) => arg.OnPositionChanged -= UpdateSlidePosition);
        }

        private void UpdateSlidePosition(TouchInputAction touchInputAction)
        {
            Vector2 deltaPosition = touchInputAction.deltaPosition;
            float delta = 0f;
            if(slideX)
            {
                delta = deltaPosition.x;
            }
            else
            {
                delta = deltaPosition.y;
            }
            delta *= slideSpeedMultiplier;
            deltaSwipe?.Invoke(delta);
        }
    }
}