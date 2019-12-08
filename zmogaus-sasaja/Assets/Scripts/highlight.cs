using UnityEngine;
using System.Collections;
using System;

public class highlight : EyeTribe.Unity.Interaction.InteractionHandler
{

    private IEnumerator _Highlight;
    private IEnumerator _Dehighlight;

    private float _Duration = 0.5f;
    private float _Time = 0f;
    private float _Step = 0.01f;

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
        ///////////////// highlight enum logic //////////////////
        if (null != _Dehighlight)
        {
            StopCoroutine(_Dehighlight);
            _Dehighlight = null;
        }

        if (null == _Highlight)
            StartCoroutine(_Highlight = Highlight());
        ///////////////// highlight enum logic //////////////////

    }

    public override void HandleOut()
    {
        ///////////////// highlight enum logic //////////////////
        if (null != _Highlight)
        {
            StopCoroutine(_Highlight);
            _Highlight = null;
        }

        if (null == _Dehighlight)
            StartCoroutine(_Dehighlight = Dehighlight());
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

            Color theCurrentColor = _InitialColor;
            Color alteredColor = new Color();

            if(theCurrentColor.r < 1f)
                alteredColor.r = theCurrentColor.r + _Step;
            if (theCurrentColor.g < 1f)
                alteredColor.g = theCurrentColor.g + _Step;
            if (theCurrentColor.b < 1f)
                alteredColor.b = theCurrentColor.b + _Step;
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

            if (theCurrentColor.r > _InitialColor.r)
                alteredColor.r = theCurrentColor.r - _Step;
            if (theCurrentColor.g > _InitialColor.g)
                alteredColor.g = theCurrentColor.g - _Step;
            if (theCurrentColor.b > _InitialColor.b)
                alteredColor.b = theCurrentColor.b - _Step;
            alteredColor.a = theCurrentColor.a;

            gameObject.GetComponent<SpriteRenderer>().material.color = alteredColor;

            yield return null;
        }

        gameObject.GetComponent<SpriteRenderer>().material.color = _InitialColor;

        _Time = 0;
    }
}
