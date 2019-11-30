using UnityEngine;
using System.Collections;

public class ButtonScript : EyeTribe.Unity.Interaction.InteractionHandler
{
    public override void Start()
    {
        base.Start();
    }

    public override void HandleIn()
    {
    }

    public override void HandleOut()
    {
    }

    public override void SelectionCompleted()
    {
        if (InteractiveItem.IsOver)
        {
            Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        }
    }

    public override void SelectionCanceled()
    {
    }

    public override void SelectionStarted()
    {
    }
}
