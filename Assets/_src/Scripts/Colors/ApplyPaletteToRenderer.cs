using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace KaitoCo
{
    public class ApplyPaletteToRenderer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer rend;
        [ColorPalette]
        [SerializeField] private Color color;


        [Button("Apply Color To Renderer")]
        public void ApplyColor()
        {
            rend.color = color;
        }
    }
}
