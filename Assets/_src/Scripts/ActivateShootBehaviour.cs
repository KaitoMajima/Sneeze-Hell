using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoCo
{
    public class ActivateShootBehaviour : MonoBehaviour
    {
        [SerializeField] private BulletPatternBehaviour[] patternBehaviours;

        public void Activate()
        {
            foreach (var patternBehaviour in patternBehaviours)
            {
                patternBehaviour.ShootingEnabled = true;
            }
            
        }

        public void Deactivate()
        {
            foreach (var patternBehaviour in patternBehaviours)
            {
                patternBehaviour.ShootingEnabled = false;
            }
        }
    }
}
