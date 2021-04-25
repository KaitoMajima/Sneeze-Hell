using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace KaitoCo
{
        
    public class EnemyWaypointMovement : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        [SerializeField] private Transform enemyTransform;
        [SerializeField] private float pointInterval = 0;
        [SerializeField] private Transform[] movePoints;
        [SerializeField] private TweenSettings tweenSettings = TweenSettings.Default;
        private int currentWayPoint = 1;

        private void Start()
        {
            if(movePoints.Length == 0)
                return;
            
            enemyTransform.DOMove(movePoints[currentWayPoint].position, tweenSettings.duration).SetEase(tweenSettings.easeType).
            OnComplete(() => StartCoroutine(Wait(pointInterval, ChangeWayPoint)));

            if(pointInterval <= 0)
                return;
        }

        private void ChangeWayPoint()
        {
            currentWayPoint++;
            if(currentWayPoint >= movePoints.Length)
                currentWayPoint = 0;
            enemyTransform.DOMove(movePoints[currentWayPoint].position, tweenSettings.duration).SetEase(tweenSettings.easeType).
            OnComplete(() => StartCoroutine(Wait(pointInterval, ChangeWayPoint)));
        }

        private IEnumerator Wait(float seconds, Action callback)
        {
            while (seconds > 0)
            {
                seconds -= Time.deltaTime;
                yield return null;
            }
            callback?.Invoke();
        }
    }

}