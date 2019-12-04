using UnityEngine;
using System.Collections;
using System;

public class highlight : EyeTribe.Unity.Interaction.InteractionHandler
{

    private IEnumerator _Highlight;
    private IEnumerator _Dehighlight;

    private float _Duration = 0.5f;
    private float _Time = 0f;

    private SpriteRenderer _Sprite;
    private Color _InitialColor;

    public override void Start()
    {
        base.Start();
        _Sprite = gameObject.GetComponent<SpriteRenderer>();
        if (_Sprite == null)
            throw new Exception("could not find sprite attached to object");
        _InitialColor = _Sprite.material.color;
    }

    public override void HandleIn()
    {
        Debug.Log("IN");
        if (null != _Dehighlight)
        {
            StopCoroutine(_Dehighlight);
            _Dehighlight = null;
        }

        if (null == _Highlight)
            StartCoroutine(_Highlight = Highlight());

    }

    public override void HandleOut()
    {
        if (null != _Highlight)
        {
            StopCoroutine(_Highlight);
            _Highlight = null;
        }

        if (null == _Dehighlight)
            StartCoroutine(_Dehighlight = Dehighlight());
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

            Color theCurrentColor = _InitialColor;
            Color alteredColor = new Color();

            alteredColor.r = theCurrentColor.r + 0.1f;
            alteredColor.g = theCurrentColor.g + 0.1f;
            alteredColor.b = theCurrentColor.b + 0.1f;
            alteredColor.a = theCurrentColor.a;

            gameObject.GetComponent<SpriteRenderer>().material.color = alteredColor;

            yield return null;
        }
    }

    private IEnumerator Dehighlight()
    {
        while (_Time - Time.deltaTime > 0f)
        {
            _Time -= Time.deltaTime;

            Color theCurrentColor = gameObject.GetComponent<SpriteRenderer>().color;
            Color alteredColor = new Color();

            alteredColor.r = theCurrentColor.r - 0.1f;
            alteredColor.g = theCurrentColor.g - 0.1f;
            alteredColor.b = theCurrentColor.b - 0.1f;
            alteredColor.a = theCurrentColor.a;

            gameObject.GetComponent<SpriteRenderer>().material.color = alteredColor;

            yield return null;
        }

        //gameObject.GetComponent<SpriteRenderer>().material.color = _InitialColor;

        _Time = 0;
    }
}
