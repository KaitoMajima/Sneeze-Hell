using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KaitoCo
{
    public class ProcessSneezeCountdown : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textComponent;
        [SerializeField] private Sneezing sneezing;
        private void Start()
        {
            sneezing.OnCountdownChanged += UpdateCountdown;
        }

        private void UpdateCountdown(int value)
        {
            textComponent.text = value.ToString();
        }
    }
}
