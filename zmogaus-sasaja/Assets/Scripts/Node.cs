using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : EyeTribe.Unity.Interaction.InteractionHandler
{

    //currently does not do anything.

    GameController controller;

	// Use this for initialization
	void Start () {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void HandleIn()
    {
        controller.OnHandleEnterNode(this);
    }

    public override void HandleOut()
    {
        controller.OnHandleLeaveNode(this);
    }

    public override void SelectionStarted()
    {
    }

    public override void SelectionCanceled()
    {
    }

    public override void SelectionCompleted()
    {
    }
}
