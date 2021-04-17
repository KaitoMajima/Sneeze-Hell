using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace KaitoCo
{
    public class ColorChanger : TweenController
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private string property;

        [SerializeField] [GradientUsage(true)]
        private Gradient gradient;
        [SerializeField]
        private PlayOnStartTween playOnStartTween = PlayOnStartTween.Activate;

        [SerializeField]
        private TweenSettings tweenSettings = TweenSettings.Default;

        private Material rendererMaterial;
        private void Awake()
        {
            rendererMaterial = spriteRenderer.material;
        }

        private void Start()
        {
            if(playOnStartTween == PlayOnStartTween.Activate)
                Activate();
            if(playOnStartTween == PlayOnStartTween.Deactivate)
                Deactivate();
        }
        public override void Activate()
        {
            DOTween.Sequence().Append(rendererMaterial.DOColor(gradient.Evaluate(0), property, 0))
            .Append(rendererMaterial.DOColor(gradient.Evaluate(1), property, tweenSettings.duration).SetEase(tweenSettings.easeType));
            
        }

        public override void Deactivate()
        {
            DOTween.Sequence().Append(rendererMaterial.DOColor(gradient.Evaluate(1), property, 0))
            .Append(rendererMaterial.DOColor(gradient.Evaluate(0), property, tweenSettings.duration).SetEase(tweenSettings.easeType));
        }
    }
}
