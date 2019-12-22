using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class GameController : MonoBehaviour {

    List<NodeLine> lines;
    List<Node> nodes;
    Transform levelObjects;
    GameObject nodePrefab;
    GameObject linePrefab;
    KeyCode playKey = KeyCode.Space;
    bool playing = false;
    bool gotNode = false;

    List<string> files;
    int fileIndex = 0;

    //[HideInInspector]
    private Node startNode = null; // node from which you start connecting
    private Node endNode = null; // node that you connect the startNode with

    private NodeLine currentLine; // line that will connect start and end nodes

    private List<float> accuraccyList;
    private Text textScore;

	void Start () {
        levelObjects = GameObject.Find("LevelObjects").transform;
        nodePrefab = (GameObject)Resources.Load("Prefabs/Circle", typeof(GameObject));
        linePrefab = (GameObject)Resources.Load("Prefabs/Line", typeof(GameObject));
        textScore = GameObject.Find("TextScore").GetComponent<Text>();

        files = new List<string>();


        foreach (string file in System.IO.Directory.GetFiles(Application.persistentDataPath))
        {
            string[] name = file.Split('\\');
            files.Add(name[name.Length - 1]);
        }

        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        LoadLevel(files[fileIndex++]);
    }

    void LoadLevel(string name)
    {
        lines = new List<NodeLine>();
        nodes = new List<Node>();
        accuraccyList = new List<float>();

        DestroyLevelObjects();
        float depth = levelObjects.transform.position.z;

        // do level loading

        string path = Application.persistentDataPath + "/" + name;
        StreamReader reader = new StreamReader(path);
        string line = reader.ReadLine();

        while (!reader.EndOfStream && !string.IsNullOrEmpty(line))
        {
            string[] words = line.Split(' ');

            GameObject node = GameObject.Instantiate(nodePrefab, levelObjects) as GameObject;
            node.name = words[0];
            node.transform.position = new Vector3(float.Parse(words[1]), float.Parse(words[2]), depth);
            nodes.Add(node.GetComponent<Node>());
            // Do Something with the input. 

            line = reader.ReadLine();
        }
        line = reader.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            int node1Index = 0, node2Index = 0;

            string[] words = line.Split(' ');

            GameObject nodeLineObject = GameObject.Instantiate(linePrefab, levelObjects) as GameObject;
            nodeLineObject.name = "line" + lines.Count;
            NodeLine nodeLine = nodeLineObject.GetComponent<NodeLine>();

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].name == words[0])
                {
                    node1Index = i;
                }
                if (nodes[i].name == words[1])
                {
                    node2Index = i;
                }
            }
            nodeLine.Initialize(0.03f, nodes[node1Index], nodes[node2Index]);
            lines.Add(nodeLine);
            // Do Something with the input. 

            if (!reader.EndOfStream)
                line = reader.ReadLine();
            else
                break;
        }

        Debug.Log(path);

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].SaveFinalState();
        }
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].SaveFinalState();
        }

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
                Debug.Log("got start node");
                startNode = node;
                gotNode = true;
            }
            else if(currentLine != null && node != startNode)// else it's endNode
            {
                Debug.Log("connecting nodes");
                endNode = node;
                ConnectNodes(startNode, endNode, currentLine);
                if (node.IsDeadEnd())
                {
                    Debug.Log("GAME OVER");
                    DisplayScore();
                    ResetLevel();
                    SetPlayingState(false);
                    // display score
                }
            }
        }
        else
        { // if not playing, set startNode
            startNode = node;
            Debug.Log("got start node");
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
                Debug.Log("got current line");
                currentLine = line;
                currentLine.StartTrackingAccuraccy();
            }
            else
            {
                Debug.Log("line not connected to node");
            }
        }
        else
        {
            Debug.Log("playing: " + playing + " startnode: " + startNode);
        }
    }

    public void OnHandleLeaveLine(NodeLine line)
    {

    }

    void ConnectNodes(Node start, Node end, NodeLine line)
    {
        if(line.isConnected(end))
        {
            Debug.Log("Successfull connection");
            // get average accuraccy from line

            startNode = endNode;
            endNode = null;

            accuraccyList.Add(currentLine.GetAverageAccuraccy());
            currentLine.StopTrackingAccuraccy();
            currentLine.useLine();
            currentLine = null;

            if (start.IsDeadEnd()) // MUST BE AFTER useLine()
            {
                start.MarkNodeCleared();
            }
        }
        else
        {
            Debug.Log("line not connected to end node");
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(playKey))
        {
            Debug.Log("space press");
            SetPlayingState(true);
        }
        else if(Input.GetKeyUp(playKey))
        {
            Debug.Log("space release");
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
            ResetLevel();
        }
    }

    void ResetLevel()
    {
        for (int i = 0; i < nodes.Count; i++) // RESET NODES FIRST
        {
            nodes[i].Reset();
        }
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].Reset();
        }
        accuraccyList = new List<float>();
    }

    void DisplayScore()
    {
        float score = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            if(lines[i].lineUsesLeft <= 0)
            {
                score += 100;
            }
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].IsDeadEnd())
            {
                score += 1000;
            }
        }
        float avgAccuraccySum = 0;
        for (int i = 0; i < accuraccyList.Count; i++)
        {
            avgAccuraccySum += accuraccyList[i];
        }
        Debug.Log("accuraccySum: " + avgAccuraccySum);
        float avgAccuraccy = avgAccuraccySum / accuraccyList.Count;
        score += 10 / avgAccuraccy;

        textScore.text = "Surinkti taškai: " + score.ToString();
        textScore.GetComponent<Fader>().FadeIn();
        Debug.Log("FINAL SCORE: " + score);
        // add accuraccy
    }
}
