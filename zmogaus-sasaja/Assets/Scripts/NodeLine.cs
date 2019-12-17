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
    private float circleColliderRadius = 0.2f;

    private float colliderDepth = 0.05f;

    private float x, y, a, b;

    private bool used;

	void Start () {

	}

    public void Initialize(float width, Node start, Node end)
    {
        line = gameObject.GetComponent<LineRenderer>();
        used = false;
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
        this.nodeEnd = end;
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
        return Object.ReferenceEquals(node, start) || Object.ReferenceEquals(node, end);
    }
	
	// Update is called once per frame
	void Update () {
        //Transform pointer = GameObject.Find("testingPoint").transform;
        //float dist = GetDistanceToPoint(pointer.position);
        //Debug.Log("length: " + dist);
    }
}
