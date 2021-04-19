using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace KaitoCo
{
    public class Player : MonoBehaviour, IActor, IDamageable
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Rigidbody2D playerRigidbody;
        [SerializeField] private Transform detectionTransform;

        [SerializeField] private float groundDetectionRadius;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float maxFallToleration = 0.4f;
        private bool isGrounded;
        private bool wasGrounded;
        private Coroutine fallingCoroutine;
        private MovementState movementState;
        private MovementInput moveInput;
        public PlayerState playerState;
        [SerializeField] private MovementSettings movementSettings = MovementSettings.Default;
        public Action onSneezeRecover;
        [SerializeField] private HealthState healthState = HealthState.Default;
        [SerializeField] private DashSettings sneezeDashSettings;
        [SerializeField] private TweenController sneezeAnimation;
        public enum PlayerState
        {
            Idle,
            Sneezing
        }
        private InputAction movementAction;
        private InputAction fireAction;
        private InputAction aimAction;
        public Action<DashSettings, Vector2> onInitiateSneeze;
        [HideInInspector] public Action<HealthState> OnHealthChanged;

        [HideInInspector] public Action<int, IActor> OnDamageTaken {get; set;}

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

        private void Update()
        {
            isGrounded = Physics2D.OverlapCircle(
            detectionTransform.position, 
            groundDetectionRadius,
            groundMask);

            bool gettingOffGround = wasGrounded && !isGrounded;
            if(gettingOffGround)
                fallingCoroutine = StartCoroutine(InitiateFall(maxFallToleration));
            
            bool gettingBackOnGround = !wasGrounded && isGrounded;
            if(gettingBackOnGround)
                CancelFall();
            wasGrounded = isGrounded;
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
            
            Movement.Pulse(ref movementState, sneezeDashSettings.movementSettings, input);
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

        private IEnumerator InitiateFall(float seconds)
        {
            while (seconds > 0)
            {
                seconds -= Time.deltaTime;
                yield return null;
            }
            Die();
            fallingCoroutine = null;
        }

        private void CancelFall()
        {
            if(fallingCoroutine == null)
                return;

            StopCoroutine(fallingCoroutine);
            Debug.Log("Back from the dead!");
        }
        public bool TryTakeDamage(int damage, IActor actor)
        {
            Health.Damage(ref healthState, damage);

            OnHealthChanged?.Invoke(healthState);
            OnDamageTaken?.Invoke(damage, actor); 
            if(healthState.Health <= 0)
            {
                healthState.Health = 0;
                Die();
            } 
            
            return true;
        }

        private void Die()
        {
            Debug.Log("ded");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(detectionTransform.position, groundDetectionRadius);
        }
    }
}
