using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

namespace KaitoCo
{
    public class LightFade : TweenController
    {
        [SerializeField] 
        private Light2D light2D;

        [SerializeField]
        private PlayOnStartTween playOnStartTween = PlayOnStartTween.Deactivate;

        [SerializeField]
        private TweenSettings tweenSettings = TweenSettings.Default;

        [SerializeField]
        private bool destroyOnEndDeactivate;
        private void Start()
        {
            if(playOnStartTween == PlayOnStartTween.Activate)
                Activate();
            if(playOnStartTween == PlayOnStartTween.Deactivate)
                Deactivate();
        }

        public override void Activate()
        {
            DOTween.To(() => light2D.intensity, x => light2D.intensity = x, 1, tweenSettings.duration).SetEase(tweenSettings.easeType);
        }
        
        public override void Deactivate()
        {
            DOTween.To(() => light2D.intensity, x => light2D.intensity = x, 0, tweenSettings.duration).SetEase(tweenSettings.easeType).OnComplete(() => DestroyOnEnd());
        }
    
        private void DestroyOnEnd()
        {
            if(!destroyOnEndDeactivate)
                return;
            Destroy(gameObject);
        }
    }
    
}
