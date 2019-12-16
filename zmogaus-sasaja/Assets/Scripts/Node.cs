using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    //currently does not do anything.

    List<Node> connectedNodes = new List<Node>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    bool isConnected(Node node)
    {
        return connectedNodes.Contains(node);
    }

    void Connect(Node node)
    {

    }
}
