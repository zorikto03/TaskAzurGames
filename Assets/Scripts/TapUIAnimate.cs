using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TapUIAnimate : MonoBehaviour
{
    Vector3 startScale;
    Sequence sequence;

    void Awake()
    {
        startScale = transform.localScale;
    }

    private void Start()
    {
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(startScale * 0.7f, 1)).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopAnimation()
    {
        sequence.Kill();
    }
}
