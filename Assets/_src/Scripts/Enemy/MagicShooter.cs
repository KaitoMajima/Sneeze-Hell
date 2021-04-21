using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace KaitoCo
{
    public class MagicShooter : Weapon
    {
        [Title("Dependencies")]
        [SerializeField] private Rigidbody2D shooterHolder;

        [SerializeField] private Transform bulletFirePoint;

        [SerializeField] private BulletPatternBehaviour shootBehaviour;

        private Vector3 bulletRotation;

        public override void Use(Transform target = null)
        {
            OnWeaponUse?.Invoke();
            
            shootBehaviour.ShootingEnabled = true;
            shootBehaviour.OnShootTrigger(bulletFirePoint, shooterHolder.transform, target);
            IsFiring = true;

        }

        public override void Stop()
        {
            IsFiring = false;
            shootBehaviour.ShootingEnabled = false;
        }
        
    }
}
