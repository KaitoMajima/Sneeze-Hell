using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace KaitoCo
{
    public class UIFade : TweenController
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private PlayOnStartTween playOnStart = PlayOnStartTween.None;

        [SerializeField]
        private bool destroyOnEndDeactivate;

        [SerializeField]
        private TweenSettings tweenSettings = TweenSettings.Default;

        private void Start()
        {
            if(playOnStart == PlayOnStartTween.Activate)
                Activate();
            if(playOnStart == PlayOnStartTween.Deactivate)
                Deactivate();
        }
        public override void Activate()
        {
            canvasGroup.DOFade(1, tweenSettings.duration).SetEase(tweenSettings.easeType);
        }

        public override void Deactivate()
        {
            canvasGroup.DOFade(0, tweenSettings.duration).SetEase(tweenSettings.easeType).OnComplete(DestroyOnEnd);
        }

        private void DestroyOnEnd()
        {
            if(destroyOnEndDeactivate)
                Destroy(gameObject);
        }
    }
}
