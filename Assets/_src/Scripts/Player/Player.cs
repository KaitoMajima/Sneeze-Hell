using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
        private InputAction fireAction;
        private InputAction aimAction;
        public Action<DashSettings, Vector2> onInitiateSneeze;
        public Action onSneezeRecover;
        [SerializeField] private DashSettings sneezeDashSettings;
        [SerializeField] private TweenController sneezeAnimation;
        public enum PlayerState
        {
            Idle,
            Sneezing
        }
        public PlayerState playerState;
        private void Start()
        {
            var playerActionMap = playerInput.actions.FindActionMap("Player");
            playerActionMap.Enable();

            movementAction = playerActionMap.FindAction("Movement");
            movementAction.performed += InputMovement;

            fireAction = playerActionMap.FindAction("Fire");

            aimAction = playerActionMap.FindAction("Aim");
        
            onInitiateSneeze += InitiateSneeze;
            onSneezeRecover += SneezeRecover;
        }

        private void FixedUpdate()
        {
            switch (playerState)
            {
                case PlayerState.Idle:
                    Movement.Move(ref movementState, movementSettings, moveInput, Time.deltaTime);
                    playerRigidbody.MovePosition((Vector2)transform.position + movementState.Velocity);
                    break;
                case PlayerState.Sneezing:
                    Movement.Move(ref movementState, sneezeDashSettings.movementSettings, Vector2.zero, Time.deltaTime);
                    playerRigidbody.MovePosition((Vector2)transform.position + movementState.Velocity);
                    break;
            }
            
                     
            Movement.SetPosition(ref movementState, transform.position);
        }
        private void InputMovement(InputAction.CallbackContext context)
        {
            moveInput.MoveVector = context.ReadValue<Vector2>();  
        }

        private void InitiateSneeze(DashSettings sneezeDashSettings, Vector2 input)
        {
            
            Movement.Move(ref movementState, sneezeDashSettings.movementSettings, input.normalized, 1);
            playerRigidbody.velocity = movementState.Velocity;

            playerState = Player.PlayerState.Sneezing;
            this.sneezeDashSettings = sneezeDashSettings;
            sneezeAnimation.Activate();
            fireAction?.Disable();
            aimAction?.Disable();
        }

        private void SneezeRecover()
        {
            movementState.Velocity = Vector2.zero;

            playerState = Player.PlayerState.Idle;
            sneezeDashSettings = DashSettings.Default;
            fireAction?.Enable();
            aimAction?.Enable();
        }
    }
}
