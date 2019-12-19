using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : EyeTribe.Unity.Interaction.InteractionHandler
{
    GameController controller;
    List<NodeLine> lines;
    ColorShifter colorShifter;
    private highlight highlighter;
    int initialLinesLeft;
    int currentLinesLeft;

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        lines = new List<NodeLine>();
        colorShifter = gameObject.GetComponent<ColorShifter>();
        highlighter = gameObject.GetComponentInChildren<highlight>();
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void ConnectToLine(NodeLine line)
    {
        lines.Add(line);
        currentLinesLeft += line.lineUsesLeft;
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

    public void MarkNodeCleared()
    {
        highlighter.Disable();
        colorShifter.ShiftUp();
    }

    public void SaveFinalState()
    {
        initialLinesLeft = currentLinesLeft;
    }

    public void Reset()
    {
        if (IsDeadEnd())
        {
            highlighter.Enable();
            colorShifter.ShiftDown();
        }
        currentLinesLeft = initialLinesLeft;
    }

    public override void HandleIn()
    {
        if(!disabled)
            controller.OnHandleEnterNode(this);
    }

    public override void HandleOut()
    {
        if (!disabled)
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
