using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

namespace KaitoCo
{
    public class Player : MonoBehaviour, IActor, IDamageable
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Rigidbody2D playerRigidbody;
        [SerializeField] private Transform detectionTransform;
        [SerializeField] private Transform checkpointTransform;
        [SerializeField] private PlayerCinematics playerCinematics;
        [SerializeField] private TweenController[] damageAnimation;
        [SerializeField] private SendAudio damageSound;
        [SerializeField] private SendAudio deathSound;
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
        public HealthState healthState = HealthState.Default;
        [SerializeField] private DashSettings sneezeDashSettings;
        [SerializeField] private TweenController sneezeAnimation;
        public enum PlayerState
        {
            Idle,
            Sneezing,
            Death
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
            movementState.Velocity = Vector2.zero;

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
            Fall();

            fallingCoroutine = null;
        }

        private void CancelFall()
        {
            if(fallingCoroutine == null)
                return;

            StopCoroutine(fallingCoroutine);
        }

        private void Fall()
        {
            movementState.Velocity = Vector2.zero;
            moveInput.MoveVector = Vector2.zero;
            playerCinematics.fallingCutscene.Play();
            TryTakeDamage(100, this);
        }
        public bool TryTakeDamage(int damage, IActor actor)
        {
            if(playerState == PlayerState.Death)
                return false;
            Health.Damage(ref healthState, damage);

            OnHealthChanged?.Invoke(healthState);
            OnDamageTaken?.Invoke(damage, actor); 
            if(healthState.Health <= 0)
            {
                healthState.Health = 0;
                Die();
            }
            else
            {
                foreach (var animation in damageAnimation)
                {
                    animation.Activate();
                }
                damageSound?.TriggerSound();
            }
            
            return true;
        }

        private void Die()
        {
            playerState = PlayerState.Death;
            moveInput.MoveVector = Vector2.zero;
            movementState.Velocity = Vector2.zero;
            playerRigidbody.simulated = false;
            playerCinematics.deathCutscene.Play();
            deathSound?.TriggerSound();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(detectionTransform.position, groundDetectionRadius);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(!collider.isTrigger)
                return;
            
            if(!collider.TryGetComponent(out IslandCheckpoint island))
                return;

            checkpointTransform = island.Checkpoint;
        }

        #region Signal Receiver Methods
        public void ReturnToCheckpoint()
        {
            transform.position = checkpointTransform.position;
        }

        public void ActivatePlayerControls()
        {
            movementAction?.Enable();
        }

        public void DeactivatePlayerControls()
        {
            movementAction?.Disable();
            movementState.Velocity = Vector2.zero;
            moveInput.MoveVector = Vector2.zero;
        }

        public void ActivateShootingControls()
        {
            fireAction?.Enable();
        }

        public void DeactivateShootingControls()
        {
            fireAction?.Disable();
        }
        #endregion
    }
}
