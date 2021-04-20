using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace KaitoCo
{
    public class OnSightAttackBehaviour : MonoBehaviour, IDetectionBehaviour
    {
        [SerializeField] private Weapon weapon;

        [SerializeField] private Transform detectionTransform;

        private Transform focusedTarget;

        [SerializeField]
        private float detectionRange;

        [SerializeField]
        private LayerMask layerMask;

        private Collider2D[] targetArray = new Collider2D[1];
        [ReadOnly] [SerializeField]
        private Collider2D target;

        public bool HasDetectedTarget {get; private set;}
        public void Detect(ref MovementInput input)
        {
            int targetCount = Physics2D.OverlapCircleNonAlloc(detectionTransform.position, detectionRange, targetArray, layerMask);
            HasDetectedTarget = targetCount > 0;
            
            if(!HasDetectedTarget && targetArray.Length > 0)
            {
                targetArray = new Collider2D[1];
                focusedTarget = null;
                target = null;
                input.MoveVector = Vector2.zero;
                weapon.Stop();
                return;
            }

            target = targetArray[0];
                
            focusedTarget = target.transform;

            input.MoveVector = focusedTarget.position - detectionTransform.position;
            input.MoveVector.Normalize();
            
            if(!weapon.IsFiring)
                weapon.Use(focusedTarget);

            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(detectionTransform.position, detectionRange);
        }
    }
}
