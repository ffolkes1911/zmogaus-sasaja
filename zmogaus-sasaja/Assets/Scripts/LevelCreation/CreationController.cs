using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class CreationController : MonoBehaviour {
    private struct Pair
    {
        public string node1;
        public string node2;
    };

    private const string nodeName = "Node";
    private bool isCreatingNode = false;
    private bool isCreatingLine = false;
    private GameObject joiningNode1 = null;
    private GameObject joiningNode2 = null;
    private int nodeCount = 0;
    private Vector3 mousePos;
    private List<Pair> NodePairs = new List<Pair>();
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject previewNode;
    [SerializeField] private GameObject previewLine;
    [SerializeField] private Text levelName;

    void Update()
    {
        if (isCreatingNode)
        {
            mousePos = Input.mousePosition;
            mousePos.z = 2;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            previewNode.transform.position = mousePos;
        }
        if (Input.GetMouseButtonDown(0) && isCreatingNode)
        {
            isCreatingNode = false;
            InstantiateNode(mousePos);
        }
        if (Input.GetMouseButtonDown(1) && isCreatingNode)
        {
            isCreatingNode = false;
            DetachNodeFromMouse();
        }

        if (isCreatingLine && joiningNode1 != null)
        {
            mousePos = Input.mousePosition;
            mousePos.z = 2;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            previewLine.GetComponent<LineRenderer>().SetPosition(1, mousePos);
        }
        if (Input.GetMouseButtonDown(1) && isCreatingLine)
        {
            isCreatingLine = false;
            DetachLineFromMouse();
        }
    }
    //========= Node Creation ===============
    void InstantiateNode(Vector3 mousePos)
    {
        DetachNodeFromMouse();
        GameObject newNode;
        newNode = Instantiate(nodePrefab, mousePos, Quaternion.identity) as GameObject;
        nodeCount++;
        newNode.name = nodeName + nodeCount.ToString();
        newNode.transform.parent = this.transform;
    }

    public void ToggleCreatingNode()
    {
        if (!isCreatingNode)
        {
            isCreatingNode = true;
            AttachNodeToMouse();
        }
        else
        {
            isCreatingNode = false;
            DetachNodeFromMouse();
        }
    }

    void AttachNodeToMouse()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 2;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        previewNode.transform.position = mousePos;
        previewNode.SetActive(true);
    }

    void DetachNodeFromMouse()
    {
        previewNode.SetActive(false);
    }

    //============ Line Creation ===================
    public void JoinNode(GameObject joinedNode)
    {
        if (isCreatingLine && joiningNode1 == null)
        {
            joiningNode1 = joinedNode;
            AttachLineToMouse();
        }
        else if (isCreatingLine && joiningNode1 != null)
        {
            isCreatingLine = false;
            joiningNode2 = joinedNode;
            Pair newPair = new Pair();
            newPair.node1 = joiningNode1.name;
            newPair.node2 = joiningNode2.name;
            NodePairs.Add(newPair);
            InstantiateLine();
        }
    }

    public void ToggleCreatingLine()
    {
        if (!isCreatingLine)
        {
            isCreatingLine = true;
        }
        else
        {
            isCreatingLine = false;
            DetachLineFromMouse();
        }
    }

    void AttachLineToMouse()
    {
        previewLine.GetComponent<LineRenderer>().SetPosition(0, joiningNode1.transform.position);
        mousePos = Input.mousePosition;
        mousePos.z = 2;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        previewLine.GetComponent<LineRenderer>().SetPosition(1, mousePos);
        previewLine.SetActive(true);
    }

    void DetachLineFromMouse()
    {
        joiningNode1 = null;
        previewLine.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
        previewLine.SetActive(false);
    }

    void InstantiateLine()
    {
        GameObject newLine;
        newLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        newLine.GetComponent<LineRenderer>().SetPosition(0, joiningNode1.transform.position);
        newLine.GetComponent<LineRenderer>().SetPosition(1, joiningNode2.transform.position);
        joiningNode1 = null;
        joiningNode2 = null;
        DetachLineFromMouse();
    }

    //================= Level Saving =================
    public void SaveLevel()
    {
        string fileData = "";
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            fileData += child.name + " " + child.position.x + " " + child.position.y + "\n";
        }
        foreach (var pair in NodePairs)
        {
            fileData += "\n" + pair.node1 + " " + pair.node2;
        }
        if (!Directory.Exists(Application.persistentDataPath + "/Pritaikytas"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Pritaikytas");
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/Pritaikytas/" + levelName.text + ".txt");
        writer.Write(fileData);
        writer.Close();
        foreach (string file in System.IO.Directory.GetFiles(Application.persistentDataPath))
        {
            Debug.Log(file.ToString());
        }
        SceneManager.LoadScene("MainMenu");
    }
}
