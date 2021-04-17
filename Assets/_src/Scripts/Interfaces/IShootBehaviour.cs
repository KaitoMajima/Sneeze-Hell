using UnityEngine;
using System.Collections.Generic;

namespace KaitoCo
{
    public interface IShootBehaviour
    {
        public bool ShootingEnabled {get; set;}
        void OnShootTrigger(GameObject bullet, Transform[] bulletFirePoints, Transform holder, Transform target = null);


    }
}
