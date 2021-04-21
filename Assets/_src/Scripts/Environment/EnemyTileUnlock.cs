using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoCo
{
    public class EnemyTileUnlock : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemies = new List<GameObject>();
        [SerializeField] private GameObject[] tileObjs;
        [SerializeField] private SendAudio unlockSound;

        private void Update()
        {
            if(enemies.Count == 0)
                return;

            EnemiesCheck();
        }

        private void EnemiesCheck()
        {

            for (int i = 0; i < enemies.Count; i++)
            {
                if(!enemies[i])
                    enemies.RemoveAt(i);
            }

            if(enemies.Count > 0)
                return; 
            
            foreach (var tile in tileObjs)
            {
                tile.SetActive(true);
            }        
            unlockSound?.TriggerSound();
        }


    }
}
