using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace KaitoCo
{
    public class TweenController : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float scaleRate;
    [SerializeField] private Ease moveEase = Ease.Linear;

    private void Start()
    {
        transform.DOScale(scaleRate, duration).SetEase(moveEase);
    }
}

}
