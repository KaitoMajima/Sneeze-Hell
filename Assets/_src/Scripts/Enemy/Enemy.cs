using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoCo
{
    public class Enemy : MonoBehaviour, IActor
    {
        private IDetectionBehaviour detectionBehaviour;
        private IShootBehaviour shootBehaviour;

        private void Start()
        {

        }
    }
}
