using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace KaitoCo
{
    public class ActivateCutsceneOnTrigger : MonoBehaviour
    {
        [SerializeField] private PlayableDirector timeline;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.TryGetComponent(out Player player))
                return;

            timeline.Play();
        }
    }
}
