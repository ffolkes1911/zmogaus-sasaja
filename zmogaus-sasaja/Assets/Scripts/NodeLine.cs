﻿using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class NodeLine : EyeTribe.Unity.Interaction.InteractionHandler
{
    [HideInInspector] public Node nodeStart;
    [HideInInspector] public Node nodeEnd;
    [HideInInspector] public Transform start;
    [HideInInspector] public Transform end;

    private LineRenderer line;
    private float lineWidth;
    private float colliderWidthMult = 6f;
    private float circleColliderRadius = 0.2f;
    private float colliderDepth = 0.05f;

    private float x, y, a, b;

    private GameController controller;
    private ColorShifter colorShifter;
    private highlight highlighter;

    public int initialLineUsesLeft = 1;
    public int lineUsesLeft = 1;

    private Transform pointer;
    private bool tracking = false;
    private float distanceSum = 0;
    private float distanceCount = 0;

    public override void Awake()
    {
        base.Awake();
        //InteractiveItem = gameObject.GetComponentInChildren<VRInteractiveItem>();

        controller = GameObject.Find("GameController").GetComponent<GameController>();
        colorShifter = gameObject.GetComponent<ColorShifter>();
        highlighter = gameObject.GetComponentInChildren<highlight>();
    }

    public void Initialize(float width, Node start, Node end)
    {
        line = gameObject.GetComponent<LineRenderer>();
        lineUsesLeft = 1;
        SetLineWidth(width);
        Connect(start, end);
        CalculateLineEquation(this.start.position, this.end.position);
        AddColliderToLine();
    }

    public void SetLineWidth(float width)
    {
        lineWidth = width;
        line.SetWidth(lineWidth, lineWidth);
    }

    public void Connect(Node start, Node end)
    {
        this.nodeStart = start;
        start.ConnectToLine(this);
        this.nodeEnd = end;
        end.ConnectToLine(this);

        this.start = nodeStart.transform;
        this.end = nodeEnd.transform;

        line.SetPosition(0, start.transform.position);
        line.SetPosition(1, end.transform.position);
    }

    // line equation y = ax + b
    public void CalculateLineEquation(Vector3 start, Vector3 end)
    {
        a = (end.y - start.y) / (end.x - start.x);
        b = start.y - (a * start.x);
        //Debug.Log(gameObject.name + " a=" + a + " b=" + b + "startpoint=[" + start.x + ":" + (a*start.x + b) + "]" + "endpoint=[" + end.x + ":" + (a * end.x + b) + "]");
    }

    public float GetPointY(float x)
    {
        return a * x + b;
    }
    
    public float GetPointX(float y)
    {
        return (y - b) / a;
    }

    public Vector3 GetClosestPoint(Vector3 P)
    {
        return Vector3.zero;
    }

    // can possibly use http://wiki.unity3d.com/index.php/3d_Math_functions
    //this function assumes an infinite line
    public float GetDistanceToPoint(Vector3 P)
    {
        Vector3 p1 = start.position;
        Vector3 p2 = end.position;
        float result = Mathf.Abs((p2.y - p1.y) * P.x - (p2.x - p1.x) * P.y + p2.x * p1.y - p2.y * p1.x) /
            (Mathf.Sqrt((Mathf.Pow(p2.y - p1.y, 2) + Mathf.Pow(p2.x - p1.x, 2))));

        return result;
    }

    // from https://answers.unity.com/questions/470943/collider-for-line-renderer.html
    private void AddColliderToLine()
    {
        Vector3 startPoint = start.transform.position;
        Vector3 endPoint = end.transform.position;

        //create the collider for the line
        BoxCollider lineCollider = line.GetComponentInChildren<BoxCollider>();
        //set the collider as a child of your line
        lineCollider.transform.parent = line.transform;

        // get the length of the line using the Distance method
        float lineLength = Vector3.Distance(startPoint, endPoint);
        // size of collider is set where X is length of line, Y is width of line
        //z will be how far the collider reaches to the sky
        lineCollider.size = new Vector3(colliderDepth, lineWidth * colliderWidthMult, lineLength - (circleColliderRadius * 2));
        // get the midPoint
        Vector3 midPoint = (startPoint + endPoint) / 2;
        // move the created collider to the midPoint
        lineCollider.transform.position = midPoint;

        // rotate the collider transform to look at the end node
        lineCollider.transform.LookAt(end.transform);
    }

    public bool isConnected(Node node)
    {
        if(node != null)
        {
            bool result = false;
            if (Object.ReferenceEquals(node, nodeStart))
            {
                result = true;
            }
            else if (Object.ReferenceEquals(node, nodeEnd))
            {
                result = true;
            }
            Debug.Log("node: " + node + " startNode: " + nodeStart + " endNode: " + nodeEnd);
            return result;
        }
        else
        {
            Debug.Log("passed node is null");
            return false;
        }
    }

    // Update is called once per frame
    void Update () {
        if (tracking)
        {
            Debug.Log("tracking");
            if(distanceSum < 10000000)
            {
                pointer = GameObject.Find("Reticle").transform;
                float dist = GetDistanceToPoint(pointer.position);
                distanceSum += dist;
                distanceCount++;
                Debug.Log("dist Sum: " + distanceSum);
            }
            else
            {
                Debug.Log("overflow: " + distanceSum);
            }
        }
        //Transform pointer = GameObject.Find("testingPoint").transform;
        //float dist = GetDistanceToPoint(pointer.position);
        //Debug.Log("length: " + dist);
    }

    public void useLine()
    {
        if(--lineUsesLeft <= 0)
        {
            colorShifter.ShiftUp();
            Debug.Log("LINE HAS NO MORE USES");
            highlighter.Disable();
        }
    }

    public void SaveFinalState()
    {
        initialLineUsesLeft = lineUsesLeft;
    }

    public void Reset()
    {
        if(lineUsesLeft <= 0)
        {
            colorShifter.ShiftDown();
            highlighter.Enable();
        }
        lineUsesLeft = initialLineUsesLeft;

        //highlighter.enabled = true;
    }

    public void StartTrackingAccuraccy()
    {
        if (!tracking)
        {
            tracking = true;
            distanceCount = 0;
            distanceSum = 0;
        }
    }

    public void StopTrackingAccuraccy()
    {
        if (tracking)
        {
            tracking = false;
            distanceCount = 0;
            distanceSum = 0;
        }
    }

    public float GetAverageAccuraccy()
    {
        if(distanceCount > 0)
        {
            return distanceSum / distanceCount;
        }
        else
        {
            return -1;
        }
    }

    public override void HandleIn()
    {
        if (!disabled)
        {
            if (lineUsesLeft > 0)
            {
                Debug.Log("handle on line");
                controller.OnHandleEnterLine(this);
            }
            else
            {
                //highlighter.enabled = false;
                Debug.Log("line used up");
            }
        }
    }

    public override void HandleOut()
    {
        if (!disabled)
        {
            if (lineUsesLeft > 0)
            {
                controller.OnHandleLeaveLine(this);
            }
        }
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
