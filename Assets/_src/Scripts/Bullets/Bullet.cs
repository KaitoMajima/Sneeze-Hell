using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;

namespace KaitoCo
{
    [Serializable] 
    public struct BulletSettings
    {
        public int bulletDamage;
        public float BulletLifeTime;

        [SerializeField]
        public MovementSettings MovementSettings;
    }

    [Serializable] 
    public struct BulletState
    {
        public MovementState MovementState;
    }

    [Serializable]
    public struct BulletData
    {
        public BulletState BulletState;
        public BulletSettings BulletSettings;
        public MovementInput MovementInput;
    }
    public class Bullet : MonoBehaviour
    {
        [Title("Dependencies")]
        [SerializeField] private GameObject bulletImpactPrefab;

        [Header("VFX")]
        [SerializeField] private SendImpulse bulletImpulse;

        [Header("SFX")]
        [SerializeField] private SendAudio bulletFireSound;

        [Space]

        [Title("Settings")]

        [SerializeField] private bool usePooling;
        public BulletData bulletData;
        private Transform bulletOwner;
        public Transform BulletOwner { get => bulletOwner; set => bulletOwner = value;}
        private Type bulletOwnerType;
        private Coroutine bulletLifetimeCoroutine;
        private bool wasEnabledLastFrame;
        private bool enabledOnce;
        private float lifetime;
        public Action<IActor, IDamageable, Bullet> OnBulletHitDamageable;
        private void Start()
        {
            lifetime = bulletData.BulletSettings.BulletLifeTime;
            if(!usePooling)
                bulletLifetimeCoroutine = StartCoroutine(LifetimeCountdown(lifetime, (seconds) => {lifetime = seconds;}));
            bulletOwnerType = bulletOwner.GetComponent<IActor>().GetType();

            if(!enabledOnce)
                bulletFireSound?.TriggerSound();
            
        }
        private void Explode()
        {
            enabledOnce = true;
            if(!usePooling)
                Destroy(gameObject);
            else
            {
                if(bulletImpactPrefab != null)
                    Instantiate(bulletImpactPrefab, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                
            }
                
        }

        public void MarkForDestruction()
        {
            if(!usePooling || !wasEnabledLastFrame)
                return;
            if(bulletImpactPrefab != null)
                Instantiate(bulletImpactPrefab, transform.position, Quaternion.identity);
        }
        private IEnumerator LifetimeCountdown(float seconds, Action<float> callback)
        {
            bulletFireSound?.TriggerSound();
            bulletImpulse?.TriggerImpulse();


            while(seconds > 0)
            {
                seconds -= Time.deltaTime;
                callback?.Invoke(seconds);
                yield return null;
            }
            
            Explode();
        }


        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.isTrigger)
                return;
            if(collider.TryGetComponent(out Bullet bullet))
                return;
            if(collider.transform == bulletOwner)
                return;

            if(collider.TryGetComponent(out IDamageable receiver))
            {
                if(bulletOwnerType == receiver.GetType())
                    return;
                if(bulletOwner == null)
                {
                    receiver.TryTakeDamage(bulletData.BulletSettings.bulletDamage, null);
                    OnBulletHitDamageable?.Invoke(null, receiver, this);
                }   
                else
                { 
                    var actor = bulletOwner.GetComponent<IActor>();

                    receiver.TryTakeDamage(bulletData.BulletSettings.bulletDamage, actor);
                    OnBulletHitDamageable?.Invoke(actor, receiver, this);
                }

            }
            if(bulletLifetimeCoroutine != null)
                StopCoroutine(bulletLifetimeCoroutine);
            Explode();
        }
        private void OnEnable()
        {
            if(usePooling)
            {
                if(bulletLifetimeCoroutine != null)
                    StopCoroutine(bulletLifetimeCoroutine);
                lifetime = bulletData.BulletSettings.BulletLifeTime;
                bulletLifetimeCoroutine = StartCoroutine(LifetimeCountdown(lifetime, (seconds) => {lifetime = seconds;}));
                
            }
            
            wasEnabledLastFrame = true;
        }

        private void OnDisable()
        {    
            wasEnabledLastFrame = false;
        }

        
    }

    [BurstCompile]
    public struct BulletJob : IJobParallelForTransform 
    {
        [ReadOnly] public float deltaTime;
        public NativeArray<BulletData> bulletData;
        public void Execute(int index, TransformAccess transform)
        {
            var data = bulletData[index];
            Movement.Move(ref data.BulletState.MovementState, data.BulletSettings.MovementSettings, data.MovementInput.MoveVector, deltaTime);
            transform.position += (Vector3)data.BulletState.MovementState.Velocity;
            Movement.SetPosition(ref data.BulletState.MovementState, transform.position);

        }
    }
}
