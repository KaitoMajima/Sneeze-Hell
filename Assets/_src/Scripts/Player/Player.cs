using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KaitoCo
{
    public class Player : MonoBehaviour, IActor
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Rigidbody2D playerRigidbody;
        private MovementState movementState;
        private MovementInput moveInput;
        [SerializeField] private MovementSettings movementSettings = MovementSettings.Default;
        private InputAction movementAction;
        private void Start()
        {
            var playerActionMap = playerInput.actions.FindActionMap("Player");
            playerActionMap.Enable();

            movementAction = playerActionMap.FindAction("Movement");
            movementAction.performed += InputMovement;
        }


        private void FixedUpdate()
        {
            Movement.Move(ref movementState, movementSettings, moveInput, Time.deltaTime);
            playerRigidbody.MovePosition((Vector2)transform.position + movementState.Velocity);
                     
            Movement.SetPosition(ref movementState, transform.position);
        }
        private void InputMovement(InputAction.CallbackContext context)
        {
            moveInput.MoveVector = context.ReadValue<Vector2>();  
        }
    }
}
