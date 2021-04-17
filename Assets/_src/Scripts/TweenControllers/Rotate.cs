using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace KaitoCo
{
    public class Rotate : TweenController
    {
        [SerializeField] 
        private Transform rotationTransform;

        [SerializeField]
        private PlayOnStartTween playOnStart = PlayOnStartTween.None;

        [SerializeField]
        private TweenSettings tweenSettings = TweenSettings.Default;

        [SerializeField]
        private TweenVector3Settings tweenVectorSettings = TweenVector3Settings.Default;

        private void Start()
        {
            
            if(playOnStart == PlayOnStartTween.Activate)
                Activate();
            if(playOnStart == PlayOnStartTween.Deactivate)
                Deactivate();
        }
        public override void Activate()
        {   
            rotationTransform.localEulerAngles = tweenVectorSettings.startValue;
            if(!tweenSettings.useLoops)
                rotationTransform.DOLocalRotate(tweenVectorSettings.endValue, tweenSettings.duration, RotateMode.FastBeyond360).SetEase(tweenSettings.easeType);
            else
                rotationTransform.DOLocalRotate(tweenVectorSettings.endValue, tweenSettings.duration, RotateMode.FastBeyond360).SetEase(tweenSettings.easeType).SetLoops(-1, tweenSettings.loopType);
        }

        public override void Deactivate()
        {
            rotationTransform.localEulerAngles = tweenVectorSettings.endValue;
            rotationTransform.DOLocalRotate(tweenVectorSettings.startValue, tweenSettings.duration, RotateMode.FastBeyond360).SetEase(tweenSettings.easeType);
        }
    }
}
