using UnityEngine;
using System.Collections;
using System;

public class testt : EyeTribe.Unity.Interaction.InteractionHandler
{
    private bool handleIn = false;
    public override void Awake()
    {
        base.Awake();
    }

    public override void HandleIn()
    {
        ///////////////// highlight enum logic //////////////////
        handleIn = true;
        Debug.Log("test IN");
        ///////////////// highlight enum logic //////////////////

    }

    public override void HandleOut()
    {
        ///////////////// highlight enum logic //////////////////
        handleIn = false;
        Debug.Log("test OUT");
        ///////////////// highlight enum logic //////////////////

    }

    public override void SelectionCanceled()
    {
    }

    public override void SelectionCompleted()
    {
        if (handleIn)
        {
            Debug.Log("click TEST");
        }
    }

    public override void SelectionStarted()
    {
    }

}
