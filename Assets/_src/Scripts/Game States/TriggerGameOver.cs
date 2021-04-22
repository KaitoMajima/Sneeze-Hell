using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoCo
{
    public class TriggerGameOver : MonoBehaviour
    {
        public void Call()
        {
            GameManager.OnGameOver?.Invoke();
        }
    }
}
