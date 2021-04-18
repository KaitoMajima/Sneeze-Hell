using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KaitoCo
{
    public class Shooter : Weapon
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BulletPatternBehaviour shootBehaviour;
        [SerializeField] private Transform bulletFirePoint;
        [SerializeField] private Transform shooterHolder;
        [SerializeField] private Transform reticleTarget;
        private InputAction fireAction;
        private InputAction aimAction;
        public Vector2 RawMousePosition { get; private set; }

        private void Start()
        {
            var playerActionMap = playerInput.actions.FindActionMap("Player");

            aimAction = playerActionMap.FindAction("Aim");
            aimAction.performed += InputMousePosition;

            fireAction = playerActionMap.FindAction("Fire");
            fireAction.performed += PullTrigger;
            fireAction.canceled += ReleaseTrigger;

            
        }  

        private void InputMousePosition(InputAction.CallbackContext context)
        {
            RawMousePosition = context.ReadValue<Vector2>();
        }
        private void PullTrigger(InputAction.CallbackContext context)
        {
            Use(reticleTarget);
        }

        private void ReleaseTrigger(InputAction.CallbackContext context)
        {
            Stop();
        }
        public override void Use(Transform target = null)
        {
            OnWeaponUse?.Invoke();
            
            shootBehaviour.ShootingEnabled = true;
            shootBehaviour.OnShootTrigger(bulletFirePoint, shooterHolder, target);
            IsFiring = true;

        }

        public override void Stop()
        {
            IsFiring = false;
            shootBehaviour.ShootingEnabled = false;
        }
        
        private void OnDestroy()
        {
            fireAction.performed -= PullTrigger;
            fireAction.canceled -= ReleaseTrigger;
        }
    }
}
