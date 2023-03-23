using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

namespace Contaquest.Mobile.Input.Drag
{
    [RequireComponent(typeof(Collider))]
    public class DraggableObject : TouchInteractable
    {
        [TabGroup("Properties")] [SerializeField] private bool useThreshhold = false;
        [TabGroup("Properties")] [SerializeField] [ShowIf("useThreshhold")] protected Vector2 dragThreshold;
        [TabGroup("Properties")] [SerializeField] protected float smoothDragFactor = 0.1f;
        [TabGroup("Properties")] [SerializeField] protected LayerMask dropLayerMask;


        [FormerlySerializedAs("targetTransform")]
        [TabGroup("References")] [SerializeField] protected Transform transformToMove;
        [TabGroup("References")] [SerializeField] protected Transform dropOffsetTransform;
        [TabGroup("References")] [SerializeField] private bool returnToStartPosition = false;
        [TabGroup("References")] [SerializeField] [ShowIf("returnToStartPosition")] protected Transform startPositionTransform;

        [TabGroup("Events")] [SerializeField] private TouchInputUnityEvent onStartDrag, onEndDrag;
        [TabGroup("Events")] [SerializeField] private UnityEvent onDropped, onDropAreaLeft;

        protected Camera activeCamera;
        protected TouchInputAction inputTouchAction;
        protected Vector3 offset = Vector3.zero;

        protected DropArea activeDropArea;

        protected virtual bool CanDrag()
        {
            // if (!useThreshhold)
            // {
                return true;
            // }
        }


        #region Draggable Methods

        public override void StartTouchInteraction(TouchInputAction touchInputAction)
        {
            base.StartTouchInteraction(touchInputAction);
            StopAllCoroutines();
            this.inputTouchAction = touchInputAction;
            touchInputAction.onEndTouch.AddListener(OnTouchReleased);

            activeCamera = ReferencesManager.Instance.MainCamera;
            offset = activeCamera.WorldToViewportPoint(transformToMove.position) - (Vector3)touchInputAction.currentPosition;
            DisableInteractability();
            StartCoroutine(DragUpdate());
            onStartDrag.Invoke(touchInputAction);
        }

        public virtual void OnTouchReleased(TouchInputAction inputTouchAction)
        {
            StopAllCoroutines();
            EnableInteractability();
            this.inputTouchAction = null;
            inputTouchAction.onEndTouch.RemoveListener(OnTouchReleased);

            if(activeDropArea != null)
            {
                activeDropArea.OnObjectReleased(this);
                activeDropArea = null;
                onDropAreaLeft?.Invoke();
            }

            if(!TryDropObject(inputTouchAction.currentPosition))
            {
                if(returnToStartPosition && startPositionTransform != null)
                {
                    StartCoroutine(MoveToNewTransform(startPositionTransform));
                }
            }

            onEndDrag.Invoke(inputTouchAction);
        }

        private bool TryDropObject(Vector2 position)
        {
            Ray ray = activeCamera.ViewportPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, dropLayerMask))
            {
                DropArea dropArea = hit.collider.GetComponent<DropArea>();

                if(dropArea == null)
                    return false;

                Debug.Log("Dropped onto " + dropArea.gameObject.name);
                
                if(!dropArea.CanObjectBeDropped())
                {
                    Debug.Log("Could not be released, droparea is full.");
                    return false;
                }

                dropArea.OnObjectDropped(this);
                onDropped?.Invoke();
                activeDropArea = dropArea;

                if(dropArea.disableObjectInteractability)
                    DisableInteractability();

                if(dropArea.setPosition)
                {
                    Vector3 targetPosition = dropArea.targetTransform.position;
                    if(dropOffsetTransform != null)
                        targetPosition = dropArea.targetTransform.position - transformToMove.TransformVector(dropOffsetTransform.localPosition);

                    StartCoroutine(MoveToPosition(targetPosition));
                }
                if(dropArea.setRotation)
                {
                    StartCoroutine(TurnToRotation(dropArea.targetTransform.rotation));
                }
                return true;
            }

            return false;
        }

        private IEnumerator MoveToNewTransform(Transform newTransform, bool setRotation = true)
        {
            bool hasArrived = false;
            while(!hasArrived)
            {
                yield return new WaitForFixedUpdate();
                transformToMove.position = Vector3.Lerp(transformToMove.position, newTransform.position, 0.2f);
                transformToMove.rotation = Quaternion.Slerp(transformToMove.rotation, newTransform.rotation, 0.2f);
                float distance = (transformToMove.position - newTransform.position).sqrMagnitude;
                float angle = Quaternion.Angle(transformToMove.rotation, newTransform.rotation);

                if (distance < 0.1f * 0.1f && angle < 2f)
                {
                    transformToMove.position = newTransform.position;
                    transformToMove.rotation = newTransform.rotation;
                    hasArrived = true;
                }
            }
        }
        private IEnumerator MoveToPosition(Vector3 newPosition)
        {
            bool hasArrived = false;
            while(!hasArrived)
            {
                yield return new WaitForFixedUpdate();
                transformToMove.position = Vector3.Lerp(transformToMove.position, newPosition, 0.2f);
                float distance = (transformToMove.position - newPosition).sqrMagnitude;

                if (distance < 0.1f * 0.1f)
                {
                    transformToMove.position = newPosition;
                    hasArrived = true;
                }
            }
        }
        private IEnumerator TurnToRotation(Quaternion newRotation)
        {
            bool hasArrived = false;
            while(!hasArrived)
            {
                yield return new WaitForFixedUpdate();
                transformToMove.rotation = Quaternion.Slerp(transformToMove.rotation, newRotation, 0.2f);
                float angle = Quaternion.Angle(transformToMove.rotation, newRotation);

                if (angle < 2f)
                {
                    transformToMove.rotation = newRotation;
                    hasArrived = true;
                }
            }
        }


        private IEnumerator DragUpdate()
        {
            while(true)
            {
                if (CanDrag())
                {
                    PerformDragUpdate();
                }
                yield return new WaitForFixedUpdate();
            }
        }

        protected virtual void PerformDragUpdate()
        {
            Vector3 viewportPosition = (Vector3)inputTouchAction.currentPosition + offset;

            Vector3 targetPosition = activeCamera.ViewportToWorldPoint(viewportPosition);
            transformToMove.position = Vector3.Lerp(transformToMove.position, targetPosition, smoothDragFactor);
        }
        #endregion
    }
}