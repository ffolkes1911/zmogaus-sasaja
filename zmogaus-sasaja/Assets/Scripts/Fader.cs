using UnityEngine;
using System.Collections;
using System;

public class Fader : MonoBehaviour
{
    private IEnumerator _Highlight;
    private IEnumerator _Dehighlight;

    private float _Duration = 2f;
    private float _Time = 0f;
    private float _Step = 1f;

    private CanvasRenderer _Renderer;
    private float _InitialAlpha;
    private float _CurrentAlpha;

    public void Awake()
    {
        _Renderer = gameObject.GetComponent<CanvasRenderer>();

        _Renderer.SetAlpha(0f);

        _InitialAlpha = _Renderer.GetAlpha();

        _Step /= _Duration;
    }

    public void FadeIn()
    {
        ///////////////// highlight enum logic //////////////////
        if (gameObject.activeInHierarchy)
        {
            if (null != _Dehighlight)
            {
                StopCoroutine(_Dehighlight);
                _Dehighlight = null;
            }

            if (null == _Highlight)
                StartCoroutine(_Highlight = Highlight());
            else
            {
                StopCoroutine(_Highlight);
                StartCoroutine(_Highlight = Highlight());
            }
        }
        ///////////////// highlight enum logic //////////////////
    }

    public void FadeOut()
    {
        ///////////////// highlight enum logic //////////////////
        if (gameObject.activeInHierarchy)
        {
            if (null != _Highlight)
            {
                StopCoroutine(_Highlight);
                _Highlight = null;
            }

            if (null == _Dehighlight)
            {
                StartCoroutine(_Dehighlight = Dehighlight());
            }
        }
        ///////////////// highlight enum logic //////////////////
    }

    private IEnumerator Highlight()
    {
        while (_Time < _Duration)
        {
            _Time += Time.deltaTime;

            _CurrentAlpha = _Renderer.GetAlpha();
            float alteredAlpha;

            if (_CurrentAlpha < 1f)
            {
                float step = _Step * Time.deltaTime;
                alteredAlpha = _CurrentAlpha + step;

                _Renderer.SetAlpha(alteredAlpha);
            }
            else
            {
                break;
            }

            yield return null;
        }
        _Time = 0;

        while (_Time < _Duration)
        {
            _Time -= Time.deltaTime;

            _CurrentAlpha = _Renderer.GetAlpha();
            float alteredAlpha;

            if (_CurrentAlpha > 0f)
            {
                float step = _Step * Time.deltaTime;
                alteredAlpha = _CurrentAlpha - step;

                _Renderer.SetAlpha(alteredAlpha);
            }

            yield return null;
        }

        _Time = 0;
        while (_Time < _Duration)
        {
            _Time -= Time.deltaTime;

            _CurrentAlpha = _Renderer.GetAlpha();
            float alteredAlpha;

            if (_CurrentAlpha > 0f)
            {
                float step = _Step * Time.deltaTime;
                alteredAlpha = _CurrentAlpha - step;

                _Renderer.SetAlpha(alteredAlpha);
            }

            yield return null;
        }

        _Renderer.SetAlpha(_InitialAlpha);

        _Time = 0;
    }

    private IEnumerator Dehighlight()
    {
        while (_Time - Time.deltaTime > 0f)
        {
            _Time -= Time.deltaTime;

            _CurrentAlpha = _Renderer.GetAlpha();
            float alteredAlpha;

            if (_CurrentAlpha > 0f)
            {
                float step = _Step * Time.deltaTime;
                alteredAlpha = _CurrentAlpha - step;

                _Renderer.SetAlpha(alteredAlpha);
            }

            yield return null;
        }

        _Renderer.SetAlpha(_InitialAlpha);

        _Time = 0;
    }
}
