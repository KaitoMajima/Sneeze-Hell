using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

namespace KaitoCo
{
    [CreateAssetMenu(fileName = "New Bullet Pattern", menuName = "Scriptable Objects/Bullet Pattern")]
    public class BulletPatternMaster : ScriptableObject
    {
        public List<BulletPattern> patterns; 

    }

    [Serializable]
    public class BulletPattern 
    {
        [Title("Common")]
        public GameObject bulletPrefab;
        public bool useLockOn;
        public float fireRate;
        public float spinRate;
        public float initialRotation;

        [Title("Accuracy Options")]
        public float accuracy = 1;
        public Vector2 accuracyAngleRange;
        
        [Title("Bullet Distribution")]
        public int bulletsPerShot = 1;
        [Range(0, 360)]
        public float maxArrayAngleRange = 360;


        [Title("Burst Options")]
        public bool isBurst;
        public float burstDuration;
        public int BurstShotCount;

        [Title("Pooling")]
        public int maxBulletsPooled = 500;
        public void ApplySpin(ref Vector3 eulerRotation, in BulletPatternState patternState)
        {
            float convertedAccuracy = RangeConverter.ConvertValues(accuracy, 0, 1, 1, 0);

            eulerRotation.z += 
            initialRotation 
            + patternState.bulletSpin
            + UnityEngine.Random.Range(accuracyAngleRange.x * convertedAccuracy, + accuracyAngleRange.y * convertedAccuracy);
        }

        public void ApplyLockOn(ref Vector3 eulerRotation, in Vector3 targetDirection)
        {
            float lockedOnRotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            eulerRotation.z += lockedOnRotation;
        }

        public void ApplyBulletDistribution(ref Vector3 eulerRotation, in int index)
        {
            if(maxArrayAngleRange == 360)
                eulerRotation = new Vector3(0, 0, maxArrayAngleRange / bulletsPerShot * index);
            else
                eulerRotation = new Vector3(0, 0, (maxArrayAngleRange / (bulletsPerShot - 1)) * index);
        }
    }

    [Serializable]
    public class BulletPatternState
    {
        public float BurstResetTime;
        public int BurstShotCount;
        public float BulletInterval;
        public float bulletSpin;
        public BulletPatternState(float resetTime, int shotCount, float interval)
        {
            BurstResetTime = resetTime;
            BurstShotCount = shotCount;
            BulletInterval = interval;
        }
    }


}