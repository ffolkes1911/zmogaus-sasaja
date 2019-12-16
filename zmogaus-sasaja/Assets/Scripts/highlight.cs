using UnityEngine;
using System.Collections;
using System;

public class highlight : EyeTribe.Unity.Interaction.InteractionHandler
{

    private IEnumerator _Highlight;
    private IEnumerator _Dehighlight;

    private float _Duration = 0.1f;
    private float _Time = 0f;
    private float _Step = 0.3f;

    private Material _Material;
    private Color _InitialColor;
    private Color _CurrentColor;

    private bool active = false;

    public override void Awake()
    {
        base.Awake();

        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if(renderer == null)// if didn't find SpriteRenderer, try to find LineRenderer
        {
            LineRenderer line = gameObject.GetComponentInParent<LineRenderer>();
            if (line == null)
            {
                throw new Exception("could not find material attached to object");
            }
            else
            {
                _Material = line.material;
            }
        }
        else
        {
            _Material = renderer.material;
        }

        _InitialColor = _Material.color;

        _Step /= _Duration;
    }

    public override void HandleIn()
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
        }
        ///////////////// highlight enum logic //////////////////

    }

    public override void HandleOut()
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

    public override void SelectionCanceled()
    {
    }

    public override void SelectionCompleted()
    {
        Debug.Log("click");
    }

    public override void SelectionStarted()
    {
    }

    private IEnumerator Highlight()
    {
        while (_Time < _Duration)
        {
            _Time += Time.deltaTime;

            _CurrentColor = _Material.color;
            Color alteredColor = new Color();

            if (_CurrentColor.r > 0f && _CurrentColor.g > 0f && _CurrentColor.b > 0f)
            {
                float step = _Step * Time.deltaTime;
                alteredColor.r = _CurrentColor.r - step;
                alteredColor.g = _CurrentColor.g - step;
                alteredColor.b = _CurrentColor.b - step;
                alteredColor.a = _CurrentColor.a;

                gameObject.GetComponent<SpriteRenderer>().material.color = alteredColor;
            }
            else
            {
                break;
            }

            yield return null;
        }
    }

    private IEnumerator Dehighlight()
    {
        while (_Time - Time.deltaTime > 0f)
        {
            _Time -= Time.deltaTime;

            _CurrentColor = _Material.color;
            Color alteredColor = new Color();

            if (_CurrentColor.r < _InitialColor.r && _CurrentColor.g < _InitialColor.g && _CurrentColor.b < _InitialColor.b)
            {
                float step = _Step * Time.deltaTime;
                alteredColor.r = _CurrentColor.r + step;
                alteredColor.g = _CurrentColor.g + step;
                alteredColor.b = _CurrentColor.b + step;
                alteredColor.a = _CurrentColor.a;

                _Material.color = alteredColor;
            }
            else
            {
                break;
            }

            yield return null;
        }

        _Material.color = _InitialColor;

        _Time = 0;
    }
}
