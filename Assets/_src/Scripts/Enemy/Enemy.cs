using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoCo
{
    public class Enemy : MonoBehaviour, IActor, IDamageable
    {
        private IDetectionBehaviour detectionBehaviour;
        private IShootBehaviour shootBehaviour;
        [SerializeField] private HealthState healthState = HealthState.Default;
        public Action<int, IActor> OnDamageTaken {get; set;}
        public Action<HealthState> OnHealthChanged;

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
           Destroy(gameObject); 
        }
    }
}
