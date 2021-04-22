using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoCo
{
    public class StaticSettingChange : MonoBehaviour
    {
        public void ChangeMusic()
        {
            Settings.ChangeMusic();
        }

        public void ChangeSFX()
        {
            Debug.Log("cahgnd");
            Settings.ChangeSFX();
        }

        public void ChangeDebug()
        {
            Settings.ChangeDebug();
        }
    }
}
