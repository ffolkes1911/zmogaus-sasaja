using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    List<NodeLine> lines;
    List<Node> nodes;
    Transform levelObjects;
    GameObject nodePrefab;
    GameObject linePrefab;
    KeyCode playKey = KeyCode.Space;
    bool playing = false;
    bool gotNode = false;

    //[HideInInspector]
    private Node startNode; // node from which you start connecting
    private Node endNode; // node that you connect the startNode with

    private NodeLine currentLine; // line that will connect start and end nodes

	void Start () {
        levelObjects = GameObject.Find("LevelObjects").transform;
        nodePrefab = (GameObject)Resources.Load("Prefabs/Circle", typeof(GameObject));
        linePrefab = (GameObject)Resources.Load("Prefabs/Line", typeof(GameObject));
        LoadLevel("asd");
    }

    void LoadLevel(string name)
    {
        lines = new List<NodeLine>();
        nodes = new List<Node>();
        DestroyLevelObjects();
        // do level loading

        // initial testing code
        float depth = levelObjects.transform.position.z;
        GameObject node1 = GameObject.Instantiate(nodePrefab, levelObjects) as GameObject;
        node1.name = "node1";
        node1.transform.position = new Vector3(-1, 2, depth);
        nodes.Add(node1.GetComponent<Node>());

        GameObject node2 = GameObject.Instantiate(nodePrefab, levelObjects) as GameObject;
        node2.name = "node2";
        node2.transform.position = new Vector3(0.458f, 1.554f, depth);
        nodes.Add(node2.GetComponent<Node>());

        GameObject node3 = GameObject.Instantiate(nodePrefab, levelObjects) as GameObject;
        node3.name = "node3";
        node3.transform.position = new Vector3(-1.679f, 0.736f, depth);
        nodes.Add(node3.GetComponent<Node>());

        GameObject node4 = GameObject.Instantiate(nodePrefab, levelObjects) as GameObject;
        node4.name = "node4";
        node4.transform.position = new Vector3(-0.294f, 1.032f, depth);
        nodes.Add(node4.GetComponent<Node>());

        GameObject node5 = GameObject.Instantiate(nodePrefab, levelObjects) as GameObject;
        node5.name = "node5";
        node5.transform.position = new Vector3(1.306f, 0.792f, depth);
        nodes.Add(node5.GetComponent<Node>());


        GameObject line1 = GameObject.Instantiate(linePrefab, levelObjects) as GameObject;
        line1.name = "line1";
        NodeLine nodeline1 = line1.GetComponent<NodeLine>();
        nodeline1.Initialize(0.03f, node1.GetComponent<Node>(), node2.GetComponent<Node>());
        lines.Add(nodeline1);

        GameObject line2 = GameObject.Instantiate(linePrefab, levelObjects) as GameObject;
        line2.name = "line2";
        NodeLine nodeline2 = line2.GetComponent<NodeLine>();
        nodeline2.Initialize(0.03f, node1.GetComponent<Node>(), node3.GetComponent<Node>());
        lines.Add(nodeline2);

        GameObject line3 = GameObject.Instantiate(linePrefab, levelObjects) as GameObject;
        line3.name = "line3";
        NodeLine nodeline3 = line3.GetComponent<NodeLine>();
        nodeline3.Initialize(0.03f, node1.GetComponent<Node>(), node4.GetComponent<Node>());
        lines.Add(nodeline3);

        GameObject line4 = GameObject.Instantiate(linePrefab, levelObjects) as GameObject;
        line4.name = "line4";
        NodeLine nodeline4 = line4.GetComponent<NodeLine>();
        nodeline4.Initialize(0.03f, node3.GetComponent<Node>(), node4.GetComponent<Node>());
        lines.Add(nodeline4);

        GameObject line5 = GameObject.Instantiate(linePrefab, levelObjects) as GameObject;
        line5.name = "line5";
        NodeLine nodeline5 = line5.GetComponent<NodeLine>();
        nodeline5.Initialize(0.03f, node5.GetComponent<Node>(), node3.GetComponent<Node>());
        lines.Add(nodeline5);

        GameObject line6 = GameObject.Instantiate(linePrefab, levelObjects) as GameObject;
        line6.name = "line6";
        NodeLine nodeline6 = line6.GetComponent<NodeLine>();
        nodeline6.Initialize(0.03f, node5.GetComponent<Node>(), node4.GetComponent<Node>());
        lines.Add(nodeline6);

        // initial testing code
    }

    void DestroyLevelObjects()
    {
        for (int i = levelObjects.childCount - 1; i >= 0; i--)
        {
            GameObject obj = levelObjects.GetChild(i).gameObject;
            GameObject.Destroy(obj);
        }
    }
	
    public void OnHandleEnterNode(Node node)
    {
        if (playing)
        {
            if (startNode == null) // if startNode not set, then it will be startNode
            {
                startNode = node;
                gotNode = true;
            }
            else // else it's endNode
            {

            }
        }
        else
        { // if not playing, set startNode
            startNode = node;
            gotNode = true;
        }
    }

    public void OnHandleLeaveNode(Node node)
    {
        if (!playing)
        {
            startNode = null;
        }
    }

    public void OnHandleEnterLine(NodeLine line)
    {
        if(playing && startNode != null)
        {
            if (line.isConnected(startNode))
            {
                currentLine = line;
            }
        }
    }

    public void OnHandleLeaveLine(NodeLine line)
    {

    }

    void ConnectNodes(Node start, Node end, NodeLine line)
    {
        if(NodeConnectedToLine(line, end))
        {
            Debug.Log("Successfull connection");
            startNode = endNode;
            endNode = null;
            currentLine = null;
        }
    }

    bool NodeConnectedToLine(NodeLine line, Node node)
    {
        return line.isConnected(node);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(playKey))
        {
            SetPlayingState(true);
        }
        else if(Input.GetKeyUp(playKey))
        {
            SetPlayingState(false);
        }
    }

    void SetPlayingState(bool state)
    {
        if (state) // if enable playing state
        {
            playing = state;
        }
        else
        {
            playing = state;
        }
    }
}
