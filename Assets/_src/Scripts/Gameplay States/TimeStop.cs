using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

namespace KaitoCo
{
    [Serializable]
    public struct TimeStopSettings
    {
        public float timeStopLength;
        [Range(0, 1)]
        public float timeStopScale;
        public static TimeStopSettings Default = new TimeStopSettings()
        {
            timeStopLength = 0.2f,
            timeStopScale = 0
        };
    }
    public class TimeStop : MonoBehaviour
    {
        [SerializeField]
        private TimeStopSettings timeStopSettings = TimeStopSettings.Default;

        [SerializeField]
        private Ease easeType = Ease.Linear;
        public void Stop()
        {
            if(timeStopSettings.timeStopLength == 0)
                return;
            
            DOTween.Sequence().Append(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeStopSettings.timeStopScale, 0))
            .Append(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, timeStopSettings.timeStopLength).SetEase(easeType));
        }
    }
}
