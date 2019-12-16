using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    List<NodeLine> lines;
    List<Node> nodes;
    Transform levelObjects;
    GameObject nodePrefab;
    GameObject linePrefab;

    [HideInInspector] public Node startedNode; // node which you connect to another node

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
        GameObject node1 = GameObject.Instantiate(nodePrefab, levelObjects) as GameObject;
        node1.transform.localPosition = new Vector3(-1.249f, 0.352f, 0);

        GameObject node2 = GameObject.Instantiate(nodePrefab, levelObjects) as GameObject;
        node2.transform.localPosition = new Vector3(-0.133f, 0.668f, 0);

        GameObject line1 = GameObject.Instantiate(linePrefab, levelObjects) as GameObject;
        NodeLine nodeline1 = line1.GetComponent<NodeLine>();
        nodeline1.Initialize(0.03f, node1.GetComponent<Node>(), node2.GetComponent<Node>());
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
	
    void SetStartNode(Node node)
    {
        startedNode = node;
    }

    bool CheckConnection(NodeLine line, Node node)
    {
        return line.isConnected(node);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
