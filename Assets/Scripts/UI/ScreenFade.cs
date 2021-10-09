using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFade : MonoBehaviour
{
    CanvasGroup _canvasGroup;

    float _duration;
    float _elapsedTime;

    bool _fading;
    bool _appearing;

    Action _finishedFadeAction;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (_fading)
        {

            var deltaTime = Time.deltaTime;
            _elapsedTime += deltaTime;

            if (_appearing)
            {
                _canvasGroup.alpha += deltaTime / _duration;
            }
            else
            {
                _canvasGroup.alpha -= deltaTime / _duration;
            }

            if (_elapsedTime >= _duration)
            {
                _fading = false;

                if (_finishedFadeAction != null)
                {
                    _finishedFadeAction.Invoke();
                    _finishedFadeAction = null;

                }
            }

        }


    }

    public void Appear(float duration, Action afterAppearAction)
    {
        Appear(duration);

        _finishedFadeAction = afterAppearAction;
    }

    public void Disappear(float duration, Action afterDisappearAction)
    {
        Disappear(duration);

        _finishedFadeAction = afterDisappearAction;
    }

    public void Appear(float duration)
    {
        _canvasGroup.alpha = 0;
        _elapsedTime = 0;


        _fading = true;
        _appearing = true;

        _duration = duration;
    }

    public void Disappear(float duration)
    {
        _canvasGroup.alpha = 1;
        _elapsedTime = 0;

        _fading = true;
        _appearing = false;

        _duration = duration;
    }


}
