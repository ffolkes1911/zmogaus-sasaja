using UnityEngine;
using System.Collections;

public class CreationNode : MonoBehaviour {
    [SerializeField] private CreationController creationController;

	// Use this for initialization
	void Start ()
    {
        creationController = this.transform.parent.GetComponent<CreationController>();
	}

    private void OnMouseDown()
    {
        creationController.JoinNode(this.gameObject);
    }
}
