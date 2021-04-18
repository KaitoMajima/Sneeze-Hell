using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace KaitoCo
{
    public class GlobalMove : TweenController
    {
        [SerializeField] 
        private Transform movingTransform;

        [SerializeField]
        private PlayOnStartTween playOnStart = PlayOnStartTween.None;

        [SerializeField]
        private TweenSettings tweenSettings = TweenSettings.Default;

        [SerializeField]
        private TweenVector2Settings tweenVectorSettings = TweenVector2Settings.Default;

        private void Start()
        {
            
            if(playOnStart == PlayOnStartTween.Activate)
                Activate();
            if(playOnStart == PlayOnStartTween.Deactivate)
                Deactivate();
        }
        public override void Activate()
        {   
            movingTransform.position = tweenVectorSettings.startValue;
            if(!tweenSettings.useLoops)
                movingTransform.DOMove(tweenVectorSettings.endValue, tweenSettings.duration).SetEase(tweenSettings.easeType);
            else
                movingTransform.DOMove(tweenVectorSettings.endValue, tweenSettings.duration).SetEase(tweenSettings.easeType).SetLoops(-1, tweenSettings.loopType);
        }

        public override void Deactivate()
        {
            movingTransform.position = tweenVectorSettings.endValue;
            movingTransform.DOMove(tweenVectorSettings.startValue, tweenSettings.duration).SetEase(tweenSettings.easeType);
        }
    }
}
