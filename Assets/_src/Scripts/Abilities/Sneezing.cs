using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoCo
{
    public class Sneezing : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float sneezingCountdown = 10;
        [SerializeField] private float sneezingCooldown = 1;
        [SerializeField] private DashSettings sneezeDashSettings = DashSettings.Default;
        [SerializeField] private BulletPatternBehaviour shootBehaviour;
        [SerializeField] private Transform bulletFirePoint;
        [SerializeField] private Transform shooterHolder;
        private void Start()
        {
            InitateCountdown();
        }

        private void InitateCountdown()
        {
            StartCoroutine(CountDown(sneezingCountdown, InitateCooldown));
            player.onSneezeRecover?.Invoke();
            shootBehaviour.ShootingEnabled = false;
            
        }
        private void InitateCooldown()
        {
            StartCoroutine(CountDown(sneezingCooldown, InitateCountdown));
            player.onInitiateSneeze?.Invoke(sneezeDashSettings);
            shootBehaviour.ShootingEnabled = true;
            shootBehaviour.OnShootTrigger(bulletFirePoint, shooterHolder);
        }
        private IEnumerator CountDown(float seconds, Action onFinishCallback)
        {
            while (seconds > float.Epsilon)
            {
                seconds -= Time.deltaTime;
                if(seconds < float.Epsilon)
                    seconds = 0;
                yield return null;
            }
            onFinishCallback?.Invoke();
        }

    }
}
