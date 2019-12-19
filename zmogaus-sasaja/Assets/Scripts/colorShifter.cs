using UnityEngine;
using System.Collections;
using System;

public class colorShifter : MonoBehaviour
{

    private IEnumerator _ShiftColorUp;
    private IEnumerator _ShiftColorDown;

    private float _Duration = 0.1f;
    private float _Time = 0f;
    private float _Step = 0.3f;

    private Material _Material;
    private Color _InitialColor;
    private Color _CurrentColor;

    public void Awake()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer == null)// if didn't find SpriteRenderer, try to find LineRenderer
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

    public void ShiftUp()
    {
        ///////////////// highlight enum logic //////////////////
        if (gameObject.activeInHierarchy)
        {
            if (null != _ShiftColorDown)
            {
                StopCoroutine(_ShiftColorDown);
                _ShiftColorDown = null;
            }

            if (null == _ShiftColorUp)
                StartCoroutine(_ShiftColorUp = ShiftColorUp());
        }
        ///////////////// highlight enum logic //////////////////

    }

    public void ShiftDown()
    {
        ///////////////// highlight enum logic //////////////////
        if (gameObject.activeInHierarchy)
        {
            if (null != _ShiftColorUp)
            {
                StopCoroutine(_ShiftColorUp);
                _ShiftColorUp = null;
            }

            if (null == _ShiftColorDown)
            {
                StartCoroutine(_ShiftColorDown = ShiftColorDown());
            }
        }
        ///////////////// highlight enum logic //////////////////

    }

    private IEnumerator ShiftColorUp()
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

                _Material.color = alteredColor;
            }
            else
            {
                break;
            }

            yield return null;
        }
    }

    private IEnumerator ShiftColorDown()
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
