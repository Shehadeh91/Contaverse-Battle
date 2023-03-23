using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

//All of this is shit and doesn't work yet, please excuse the mess...
//TODO: Proper events and cleanup (getting things to work!!)
//namespace Contaquest.Mobile.Input
//{
//    public class TouchSwipable : TouchInteractable
//    {
//        [TabGroup("References")] [SerializeField] private Collider col;
//        private Camera activeCamera;
//        private TouchInputAction touchInputAction;
//        [SerializeField] private Vector3 deltaSwipeAxis;
//        [TabGroup("Events")] [SerializeField] private UnityEvent onStartSwipe, onEndSwipe;
//        [TabGroup("Events")] [SerializeField] private FloatUnityEvent deltaSwipe;
//        [TabGroup("State")] [SerializeField, ReadOnly] private Vector3 lastPosition;
//        [TabGroup("State")] [SerializeField, ReadOnly] private Vector3 offset;

//        private void Update()
//        {
//            SwipeUpdate();
//        }

//        private void SwipeUpdate()
//        {
//            Vector3 viewportPosition = (Vector3)touchInputAction.currentPosition;
//            viewportPosition.z = offset.z;
//            Vector3 worldpoint = activeCamera.ViewportToWorldPoint(viewportPosition);

//            Vector3 deltaPosition = worldpoint - lastPosition;
//            lastPosition = worldpoint;

//            float deltaSwipeAmount = Vector3.Scale(deltaPosition, deltaSwipeAxis).magnitude;
//            deltaSwipe?.Invoke(deltaSwipeAmount);
//        }

//        public override void EnableInteractability()
//        {
//            col.enabled = true;
//        }

//        public override void DisableInteractability()
//        {
//            col.enabled = true;
//        }

//        public override void StartTouchInteraction(TouchInputAction touchInputAction)
//        {
//            StopAllCoroutines();
//            this.touchInputAction = touchInputAction;
//            touchInputAction.onEndTouch.AddListener(OnTouchReleased);

//            activeCamera = ReferencesManager.Instance.MainCamera;
//            offset = activeCamera.WorldToViewportPoint(transform.position);// - (Vector3)touchInputAction.currentPosition;
//            onStartSwipe.Invoke();
//        }

//        public void OnTouchReleased(TouchInputAction touchInputAction)
//        {
//            touchInputAction.onEndTouch.RemoveListener(OnTouchReleased);

//        }
//        public override void EndTouchInteraction(TouchInputAction touchInputAction)
//        {

//        }
//    }
//}
