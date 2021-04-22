using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoCo
{
    public class DebugActivate : MonoBehaviour
    {
        [SerializeField] private GameObject[] debugs;

        private void Start()
        {
            Change(Settings.debugActivated);

            Settings.settingsChanged += Change;     
        }

        private void Change(bool setting)
        {
            foreach (var debug in debugs)
            {
                debug.SetActive(setting);
            }
        }

        private void OnDestroy()
        {
            Settings.settingsChanged -= Change;  
        }
    }
}
