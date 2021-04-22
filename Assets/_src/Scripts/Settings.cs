using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KaitoCo
{
    public class Settings : MonoBehaviour
    {
        public static bool musicOn = true;
        public static bool SFXOn = true;
        public static bool debugActivated = false;
        public static Action<bool> settingsChanged;

        [SerializeField] private AudioMixer mixer;

        private static AudioMixer _mixer;
        private void Start()
        {
            _mixer = mixer;
        }
        public static void ChangeMusic()
        {
            musicOn = !musicOn;
            if(!musicOn)
                _mixer.SetFloat("MusicVol", -80);
            else
                _mixer.SetFloat("MusicVol", 0);
        }

        public static void ChangeSFX()
        {
            SFXOn = !SFXOn;
            Debug.Log(SFXOn);
            if(!musicOn)
                _mixer.SetFloat("SFXVol", -80);
            else
                _mixer.SetFloat("SFXVol", 0);
        }

        public static void ChangeDebug()
        {
            debugActivated = !debugActivated;
            settingsChanged?.Invoke(debugActivated);
        }
    }
}
