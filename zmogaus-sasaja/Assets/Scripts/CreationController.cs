using UnityEngine;
using System.Collections.Generic;

public class CreationController : MonoBehaviour {
    private struct Pair
    {
        string node1;
        string node2;
    };

    private const string nodeName = "Node";
    private bool isCreatingNode = false;
    private bool isCreatingLine = false;
    private int nodeCount = 0;
    private Vector3 mousePos;
    private List<Pair> Pairs = new List<Pair>();
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject previewNode;

    void Start() {

    }

    // Update is called once per frame
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
    }

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
}
