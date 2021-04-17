using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KaitoCo
{
    public class FollowReticle : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        
        [SerializeField] private Transform reticleTransform;

        [SerializeField] private Camera mainCamera;
        private Vector2 mousePosition;
        private InputAction aimAction;

        public Vector2 RawMousePosition {get; private set;}

        private void Start()
        {
            var playerActionMap = playerInput.actions.FindActionMap("Player");

            aimAction = playerActionMap.FindAction("Aim");
            aimAction.performed += InputMousePosition;
        }

        private void InputMousePosition(InputAction.CallbackContext context)
        {
            RawMousePosition = context.ReadValue<Vector2>();
        }

        private void LateUpdate()
        {
            mousePosition = mainCamera.ScreenToWorldPoint(RawMousePosition);
            reticleTransform.position = mousePosition;
        }

    }
}
