using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace KaitoCo
{
    public class MaterialChanger : TweenController
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private string property;

        [SerializeField]
        private PlayOnStartTween playOnStartTween = PlayOnStartTween.Activate;

        [SerializeField]
        private TweenSettings tweenSettings = TweenSettings.Default;

        [SerializeField]
        private TweenValueSettings tweenValueSettings = TweenValueSettings.Default;

        private Material rendererMaterial;
        private void Awake()
        {
            rendererMaterial = spriteRenderer.material;
        }

        private void Start()
        {
            if(playOnStartTween == PlayOnStartTween.Activate)
                Activate();
        }
        public override void Activate()
        {
            rendererMaterial.DOFloat(tweenValueSettings.startValue, property, tweenSettings.duration).SetEase(tweenSettings.easeType);
        }

        public override void Deactivate()
        {
            rendererMaterial.DOFloat(tweenValueSettings.endValue, property, tweenSettings.duration).SetEase(tweenSettings.easeType);
        }
    }
}
