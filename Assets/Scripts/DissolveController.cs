using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DissolveController : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] float dissolveRate = 0.0125f;
    [SerializeField] float refreshTime = 0.025f;

    float _startDissolve;
    bool _playing;

    public static Action FinishedDissolve;
    public bool IsPlaying => _playing;

    private void Start()
    {
        _startDissolve = material.GetFloat("_DissolveAmount");
    }

    public void PlayAnimation()
    {
        StartCoroutine(DissolveCoroutine());
    }
    IEnumerator DissolveCoroutine()
    {
        float counter = 0;
        _playing = true;

        while (material.GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;

            material.SetFloat("_DissolveAmount", counter);
            
            yield return new WaitForSeconds(refreshTime);
        }

        material.SetFloat("_DissolveAmount", _startDissolve);

        _playing = false;
        FinishedDissolve?.Invoke();
    }
}
