using UnityEngine;
using System.Collections;

public class NodeLine : MonoBehaviour {

    [HideInInspector] public Node nodeStart;
    [HideInInspector] public Node nodeEnd;
    [HideInInspector] public Transform start;
    [HideInInspector] public Transform end;
    private LineRenderer line;
    private float lineWidth;
    private float colliderWidthMult = 6f;

    private float colliderDepth = 0.05f;

    private bool used;

	void Start () {

	}

    public void Initialize(float width, Node start, Node end)
    {
        line = gameObject.GetComponent<LineRenderer>();
        used = false;
        SetLineWidth(width);
        Connect(start, end);
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
        this.nodeEnd = end;
        this.start = nodeStart.transform;
        this.end = nodeEnd.transform;
    }

    public bool isConnected(Node node)
    {
        return Object.ReferenceEquals(node, start) || Object.ReferenceEquals(node, end);
    }
	
	// Update is called once per frame
	void Update () {
        line.SetPosition(0, start.transform.position);
        line.SetPosition(1, end.transform.position);
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
        lineCollider.size = new Vector3(colliderDepth, lineWidth * colliderWidthMult, lineLength);
        // get the midPoint
        Vector3 midPoint = (startPoint + endPoint) / 2;
        // move the created collider to the midPoint
        lineCollider.transform.position = midPoint;

        // rotate the collider transform to look at the end node
        lineCollider.transform.LookAt(end.transform);
    }
}
