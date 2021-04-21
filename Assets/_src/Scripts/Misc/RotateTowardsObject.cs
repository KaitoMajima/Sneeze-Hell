using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoCo
{
    public class RotateTowardsObject : MonoBehaviour
    {
        [SerializeField] private Transform rotatedTransform;

        [SerializeField] private Transform targetTransform;

        private void Start()
        {
            if(targetTransform == null)
                targetTransform = FindObjectOfType<FollowReticle>().transform;
        }
        private void Update()
        {
            Vector2 offset = targetTransform.position - rotatedTransform.position;

            transform.localRotation = Quaternion.FromToRotation(Vector2.right, offset);
        }
    }
}
