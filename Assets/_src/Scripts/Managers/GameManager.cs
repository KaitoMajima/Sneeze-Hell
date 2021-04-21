using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KaitoCo
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {get; private set;}

        public static Action OnGameOver;
        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
