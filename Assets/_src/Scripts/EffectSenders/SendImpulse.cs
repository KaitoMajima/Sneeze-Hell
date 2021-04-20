using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace KaitoCo
{
    public class SendImpulse : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource impulseSource;
        
        public void TriggerImpulse()
        {
            impulseSource.GenerateImpulse(Vector2.up);
        }

    }
}
