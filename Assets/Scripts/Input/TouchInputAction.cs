using System.Runtime.InteropServices.WindowsRuntime;
using System;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Contaquest.Mobile.Input
{
    [System.Serializable]
    public class TouchInputAction
    {
        public bool pressed = false;

        public float startTime;
        public float Duration => Time.time - startTime;

        public Vector2 startPosition;
        public Vector2 currentPosition;

        public Vector2 deltaPosition;
        [ShowInInspector, ReadOnly] private Vector2 direction;
        public Vector2 Direction
        {
            get
            {
                direction = currentPosition - startPosition;
                return currentPosition - startPosition;
            }
        }


        [Header("Events")]
        public TouchInputUnityEvent onStartTouch = new TouchInputUnityEvent();
        public TouchInputUnityEvent onEndTouch = new TouchInputUnityEvent();
        public Action<TouchInputAction> OnPositionChanged;

        public void UpdatePosition(Vector2 newPosition)
        {
            deltaPosition = newPosition - currentPosition;
            currentPosition = newPosition;
            OnPositionChanged?.Invoke(this);
        }
    }
}

