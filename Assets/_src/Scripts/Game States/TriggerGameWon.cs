using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoCo
{
    public class TriggerGameWon : MonoBehaviour
    {
        public void Call()
        {
            GameManager.OnGameWon?.Invoke();
        }
    }
}
