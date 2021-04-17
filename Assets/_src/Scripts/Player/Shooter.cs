using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KaitoCo
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        [SerializeField] private Camera mainCamera;
        private InputAction fireAction;
        private InputAction aimAction;
        public Vector2 RawMousePosition { get; private set; }

        private void Start()
        {
            var playerActionMap = playerInput.actions.FindActionMap("Player");

            aimAction = playerActionMap.FindAction("Aim");
            aimAction.performed += InputMousePosition;

            fireAction = playerActionMap.FindAction("Fire");
            fireAction.performed += FireBullet;
        }

        private void InputMousePosition(InputAction.CallbackContext context)
        {
            RawMousePosition = context.ReadValue<Vector2>();
        }
        private void FireBullet(InputAction.CallbackContext context)
        {
            Debug.Log("<color=orange>Fire at </color>" + mainCamera.ScreenToWorldPoint(RawMousePosition));
        }

        
    }
}
