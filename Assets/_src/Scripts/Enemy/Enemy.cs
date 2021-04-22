using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace KaitoCo
{
    public class Enemy : MonoBehaviour, IActor, IDamageable
    {
        private IDetectionBehaviour detectionBehaviour;
        private IShootBehaviour shootBehaviour;

        [SerializeField] private Transform enemyMainTransform;
        [SerializeField] private TweenController[] damageAnimation;
        [SerializeField] private SendAudio damageSound;
        [SerializeField] private SendAudio deathSound;
        [SerializeField] private HealthState healthState = HealthState.Default;
        [SerializeField] private PlayableDirector deathCutscene;
        public MovementState movementState;
        private MovementInput movementInput;
        public Action<int, IActor> OnDamageTaken {get; set;}
        public Action<HealthState> OnHealthChanged;
        private bool hasDied;

        private void Start()
        {
            detectionBehaviour = GetComponent<IDetectionBehaviour>();
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

        private void Update()
        {
            if(detectionBehaviour != null)
                detectionBehaviour.Detect(ref movementInput);
        }
        private void Die()
        {
            if(hasDied)
                return;
            
            hasDied = true;
            deathSound?.TriggerSound();
            if(deathCutscene != null)
            {
                deathCutscene.Play();
                return;
            }
            Destroy(enemyMainTransform.gameObject);
            
        }
    }
}
