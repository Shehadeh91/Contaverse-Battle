using System;
using System.Collections.Generic;
using Contaquest.Mobile.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Contaquest.Mobile.Input
{
    public class TouchInputManager : GenericSingleton<TouchInputManager>
    {
        public PlayerControls playerControls;

        public TouchInputAction primaryInputTouchAction = new TouchInputAction();
        public TouchInputAction secondaryInputTouchAction = new TouchInputAction();

        public override void Awake()
        {
            base.Awake();
            playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            TouchSimulation.Enable();
            playerControls.Enable();
            playerControls.CombatControls.Enable();

            playerControls.CombatControls.PrimaryContact.started += PrimaryTouchStarted;
            playerControls.CombatControls.PrimaryContact.canceled += PrimaryTouchEnded;
            playerControls.CombatControls.PrimaryPosition.performed+= UpdatePrimaryTouch;

            playerControls.CombatControls.SecondaryContact.started += SecondaryTouchStarted;
            playerControls.CombatControls.SecondaryContact.canceled += SecondaryTouchEnded;

            //Debug.Log("Enabling controls");
        }


        private void OnDisable()
        {
            playerControls.CombatControls.PrimaryContact.started -= PrimaryTouchStarted;
            playerControls.CombatControls.PrimaryContact.canceled -= PrimaryTouchEnded;

            playerControls.CombatControls.SecondaryContact.started -= SecondaryTouchStarted;
            playerControls.CombatControls.SecondaryContact.canceled -= SecondaryTouchEnded;
            playerControls.Disable();
        }
        private void Start()
        {
            // Debug.Log(playerControls.CombatControls.PrimaryPosition.ReadValue<Vector2>());
        }


        private void PrimaryTouchStarted(InputAction.CallbackContext context)
        {
            Vector2 value = playerControls.CombatControls.PrimaryPosition.ReadValue<Vector2>();
            // Debug.Log(playerControls.CombatControls.PrimaryPosition.ReadValue<Vector2>());
            primaryInputTouchAction.startPosition = ScreenToViewPort(value);
            primaryInputTouchAction.currentPosition = primaryInputTouchAction.startPosition;
            primaryInputTouchAction.startTime = Time.time;
            primaryInputTouchAction.pressed = true;
            // Debug.Log(primaryInputTouchAction.startPosition + "screen" + ScreenUtility.ViewportToScreen(primaryInputTouchAction.startPosition));
            primaryInputTouchAction.onStartTouch.Invoke(primaryInputTouchAction);
            primaryInputTouchAction.OnPositionChanged?.Invoke(primaryInputTouchAction);
        }

        private void UpdatePrimaryTouch(InputAction.CallbackContext context)
        {
            Vector2 newPosition = ScreenToViewPort(playerControls.CombatControls.PrimaryPosition.ReadValue<Vector2>());
            if(primaryInputTouchAction.startPosition == Vector2.zero)
                primaryInputTouchAction.startPosition = newPosition;
            primaryInputTouchAction.UpdatePosition(newPosition);
        }

        private void PrimaryTouchEnded(InputAction.CallbackContext context)
        {
            //Debug.Log("Ended Primary Touch");

            primaryInputTouchAction.currentPosition = ScreenToViewPort(playerControls.CombatControls.PrimaryPosition.ReadValue<Vector2>());
            primaryInputTouchAction.pressed = false;
            primaryInputTouchAction.onEndTouch.Invoke(primaryInputTouchAction);
        }

        private void SecondaryTouchStarted(InputAction.CallbackContext context)
        {
            Vector2 value = playerControls.CombatControls.SecondaryPosition.ReadValue<Vector2>();
            secondaryInputTouchAction.startPosition = ScreenToViewPort(value);
            secondaryInputTouchAction.currentPosition = secondaryInputTouchAction.startPosition;
            secondaryInputTouchAction.startTime = Time.time;
            secondaryInputTouchAction.pressed = true;
            secondaryInputTouchAction.onStartTouch.Invoke(secondaryInputTouchAction);
            secondaryInputTouchAction.OnPositionChanged?.Invoke(secondaryInputTouchAction);
        }

        private void UpdateSecondaryTouch(InputAction.CallbackContext context)
        {
            Vector2 newPosition = ScreenToViewPort(playerControls.CombatControls.SecondaryPosition.ReadValue<Vector2>());
            secondaryInputTouchAction.UpdatePosition(newPosition);
        }

        private void SecondaryTouchEnded(InputAction.CallbackContext context)
        {
            secondaryInputTouchAction.currentPosition = ScreenToViewPort(playerControls.CombatControls.SecondaryPosition.ReadValue<Vector2>());
            secondaryInputTouchAction.pressed = false;
            secondaryInputTouchAction.onEndTouch.Invoke(secondaryInputTouchAction);
        }

        public Vector2 ScreenToViewPort(Vector2 vector2)
        {
            vector2.x = vector2.x / Screen.width;
            vector2.y = vector2.y / Screen.height;
            return vector2;
        }
    }
}
