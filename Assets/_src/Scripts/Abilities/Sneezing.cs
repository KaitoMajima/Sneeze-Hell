using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KaitoCo
{
    public class Sneezing : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Transform bulletFirePoint;
        [SerializeField] private Transform shooterHolder;
        [SerializeField] private BulletPatternBehaviour shootBehaviour;
        [SerializeField] private TweenController[] indicationsAnimation;
        [SerializeField] private RotateTowardsDirection[] indicationsRotator;
        [SerializeField] private TweenController sneezeDangerAnimation;
        [SerializeField] private SendAudio sneezingPrepare;
        [SerializeField] private SendAudio sneezingShoot;
        [SerializeField] private SendImpulse sneezingImpulse;
        [SerializeField] private float sneezingCountdown = 10;
        [SerializeField] private float sneezingIndication = 2;
        [SerializeField] private float sneezingCooldown = 1;
        [SerializeField] private DashSettings sneezeDashSettings = DashSettings.Default;
        private float xSneezeRandomValue;
        private float ySneezeRandomValue;
        private Vector2 sneezeRandomVector;
        public Action<int> OnCountdownChanged;

        private void Start()
        {
            InitateCountdown();
        }

        private void InitateCountdown()
        {
            StartCoroutine(CountDown(sneezingCountdown, InitateCooldown, sneezingIndication, ShowIndication));
            player.onSneezeRecover?.Invoke();
            shootBehaviour.ShootingEnabled = false;

            xSneezeRandomValue = Random.Range(-1f, 1f);
            if(xSneezeRandomValue < float.Epsilon && xSneezeRandomValue > -float.Epsilon)
                xSneezeRandomValue = 1;
            
            ySneezeRandomValue = Random.Range(-1f, 1f);
            if(ySneezeRandomValue < float.Epsilon && ySneezeRandomValue > -float.Epsilon)
                ySneezeRandomValue = 1;
            
            sneezeRandomVector = new Vector2(xSneezeRandomValue, ySneezeRandomValue).normalized;
            foreach (var indicationRotator in indicationsRotator)
            {
                indicationRotator.Rotate(sneezeRandomVector);
            }
        }
        private void InitateCooldown()
        {
            StartCoroutine(CountDown(sneezingCooldown, InitateCountdown));
            player.onInitiateSneeze?.Invoke(sneezeDashSettings, sneezeRandomVector);
            shootBehaviour.ShootingEnabled = true;
            shootBehaviour.OnShootTrigger(bulletFirePoint, shooterHolder);
            sneezingShoot?.TriggerSound();
            sneezingImpulse?.TriggerImpulse();
            
            HideIndication();
        }

        private void ShowIndication()
        {
            foreach (var indicationAnimation in indicationsAnimation)
            {
                indicationAnimation.Activate();
            }
            
        }

        private void HideIndication()
        {
            foreach (var indicationAnimation in indicationsAnimation)
            {
                indicationAnimation.Deactivate();
            }
        }
        private IEnumerator CountDown(float seconds, Action onFinishCallback, float? indicationTime = null, Action onIndicationPassedCallback = null)
        {
            bool indicationPassed = false;
            while (seconds > float.Epsilon)
            {
                seconds -= Time.deltaTime;
                if(seconds < float.Epsilon)
                    seconds = 0;
                
                if(indicationTime != null)
                {
                    if(seconds < indicationTime && !indicationPassed)
                    {
                        onIndicationPassedCallback?.Invoke();
                        sneezingPrepare?.TriggerSound();
                        sneezeDangerAnimation?.Activate();
                        indicationPassed = true;
                    }
                    OnCountdownChanged?.Invoke(Mathf.CeilToInt(seconds));    
                }
                
                yield return null;
            }
            onFinishCallback?.Invoke();
            sneezeDangerAnimation?.Deactivate();
        }

    }
}
