using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System;
using Cinemachine;
using Contaquest.Mobile.Input;

namespace Contaquest.Metaverse.Rooms
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField][InlineEditor]
        private RoomSO roomSO;

        [TabGroup("References")] [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [TabGroup("References")] public Transform startPoint, endpoint, elevatorTargetPoint;
        [TabGroup("References")] public RoomInteraction[] roomInteractions;
        [TabGroup("References")] [SerializeField] private TouchInteractable[] touchInteractions;

        [TabGroup("Events")] [SerializeField] private UnityEvent onActivated, onDeactivated;

        [TabGroup("State")] [SerializeField, ReadOnly] private RoomInteraction activeInteraction;

        public void Initialize()
        {
            //Debug.Log("Initialized Room");

            virtualCamera.m_Priority = 0;
            foreach (var roomInteraction in roomInteractions)
            {
                roomInteraction.Initialize(this);
            }
            DeactivateRoom();
        }

        private void OnEnable()
        {
            roomSO.onSelected += StartActivateRoom;
            roomSO.onDeselected += StartDeactivateRoom;
        }
        private void OnDisable()
        {
            roomSO.onSelected -= StartActivateRoom;
            roomSO.onDeselected -= StartDeactivateRoom;
        }

        [Button] [TabGroup("Buttons")] [HideInEditorMode]
        public void StartActivateRoom()
        {
            virtualCamera.m_Priority = 100;

            if (RoomMovementController.Instance != null)
                RoomMovementController.Instance.StartMoveToRoomController(this, ActivateRoom);
            else
                ActivateRoom();
        }

        public void ForceStartActivateRoom()
        {
            virtualCamera.m_Priority = 100;
            RoomMovementController.Instance.SetElevatorPosition(elevatorTargetPoint.position);
            RoomMovementController.Instance.SetIsOnElevator(true);
            RoomMovementController.Instance.SetPosition(elevatorTargetPoint.position);
            ActivateRoom();
        }

        [Button] [TabGroup("Buttons")] [HideInEditorMode]
        public void StartDeactivateRoom()
        {
            DeactivateRoom();
        }

        private void ActivateRoom()
        {
            // Debug.Log($"Activating Room {gameObject.name}", gameObject);
            
            foreach (var roomInteraction in roomInteractions)
            {
                roomInteraction.EnableInteractability();
            }
            foreach (var touchInteraction in touchInteractions)
            {
                touchInteraction.EnableInteractability();
            }
            onActivated.Invoke();
        }

        private void DeactivateRoom()
        {
            // Debug.Log($"Deactivating Room {gameObject.name}", gameObject);

            virtualCamera.m_Priority = 0;
            foreach (var roomInteraction in roomInteractions)
            {
                roomInteraction.DisableInteractability();
            }
            foreach (var touchInteraction in touchInteractions)
            {
                touchInteraction.DisableInteractability();
            }
            onDeactivated.Invoke();
        }

        public void SetActiveInteraction(RoomInteraction newRoomInteraction)
        {
            activeInteraction?.OnInteractionDeactivated();
            activeInteraction = newRoomInteraction;

            foreach (var roomInteraction in roomInteractions)
            {
                roomInteraction.DisableInteractability();
            }
        }
        public void StopActiveInteraction()
        {
            // Debug.Log("Stopping active interaction " + activeInteraction.gameObject.name);
            activeInteraction = null;
            RoomMovementController.Instance.StartMoveToElevator();
            foreach (var roomInteraction in roomInteractions)
            {
                roomInteraction.EnableInteractability();
            }
        }
    }
}
