using System;
using System.Collections.Generic;
using Cinemachine;
using Contaquest.Metaverse.Robot;
using Contaquest.Metaverse.Rooms.UI;
using Contaquest.Mobile.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Rooms
{
    [RequireComponent(typeof(Collider))]
    public class RoomInteraction : MonoBehaviour, iTouchInteractable
    {
        [TabGroup("References")] [SerializeField] private Collider col;
        [TabGroup("References")] [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [TabGroup("References")] [SerializeField] private List<TouchInteractable> touchInteractables;
        [TabGroup("References")] public Transform robotTargetTransform;

        [TabGroup("Events")] [SerializeField] private UnityEvent onStartActivated, onActivated, onDeactivated;
        [TabGroup("Events")] [SerializeField] private RobotDefinitionUnityEvent onInitialized;

        [TabGroup("State")] [SerializeField, ReadOnly] private bool isRoomInteractionActive = false;
        [TabGroup("State")] [SerializeField, ReadOnly] private RoomController roomController;

        public void Initialize(RoomController roomController)
        {
            //Debug.Log("Initialized Interaction");
            this.roomController = roomController;
            virtualCamera.m_Priority = 0;
            DeactivateInteraction();
        }

        public void EnableInteractability()
        {
            col.enabled = true;
        }

        public void DisableInteractability()
        {
            col.enabled = false;
        }

        public void OnInteractionActivated()
        {
            ActivateInteraction();
        }

        public void OnInteractionDeactivated()
        {
            DeactivateInteraction();
            roomController.StopActiveInteraction();
        }

        private void ActivateInteraction()
        {
            if (isRoomInteractionActive)
                return;

            onActivated?.Invoke();
            EnableSubInteractions(true);
            isRoomInteractionActive = true;
        }
        private void DeactivateInteraction()
        {
            if (!isRoomInteractionActive)
                return;

            EnableSubInteractions(false);
            isRoomInteractionActive = false;

            virtualCamera.m_Priority = 0;
            onDeactivated?.Invoke();
        }

        public void StartTouchInteraction(TouchInputAction touchInputAction)
        {
        }

        public void EndTouchInteraction(TouchInputAction touchInputAction)
        {
            if (touchInputAction.Direction.sqrMagnitude > 0.15f * 0.15f)
                return;
            StartActivateInteraction();
        }

        [Button] [TabGroup("Buttons")] [HideInEditorMode]
        public void StartActivateInteraction()
        {
            virtualCamera.m_Priority = 101;

            if (roomController != null)
                roomController.SetActiveInteraction(this);

            RoomMovementController.Instance.StartMoveToTransform(robotTargetTransform, OnInteractionActivated);

            onStartActivated?.Invoke();
        }

        public void ForceStartActivateInteraction()
        {
            roomController.ForceStartActivateRoom();
            
            virtualCamera.m_Priority = 101;
            if (roomController != null)
                roomController.SetActiveInteraction(this);

            RoomMovementController.Instance.SetPosition(robotTargetTransform.position);
            RoomMovementController.Instance.SetIsOnElevator(false);

            OnInteractionActivated();

            onStartActivated?.Invoke();
        }

        [Button] [TabGroup("Buttons")] [HideInEditorMode]
        public void StartDeactivateInteraction()
        {
            OnInteractionDeactivated();
        }

        public void EnableSubInteractions(bool isEnabled)
        {
            foreach (var touchInteractable in touchInteractables)
            {
                if (isEnabled)
                    touchInteractable.EnableInteractability();
                else
                    touchInteractable.DisableInteractability();
            }
        }
    }
}
