using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : EyeTribe.Unity.Interaction.InteractionHandler
{
    GameController controller;
    List<NodeLine> lines;
    int linesLeft;

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        lines = new List<NodeLine>(); 
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void ConnectToLine(NodeLine line)
    {
        lines.Add(line);
        linesLeft += line.lineUsesLeft;
    }

    public bool IsDeadEnd()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if(lines[i].lineUsesLeft > 0)
            {
                return false;
            }
        }
        return true;
    }

    public void Reset()
    {

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
