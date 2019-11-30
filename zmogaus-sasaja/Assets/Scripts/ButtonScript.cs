using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : EyeTribe.Unity.Interaction.InteractionHandler
{
    private Button button;
    public override void Start()
    {
        base.Start();
        button = gameObject.GetComponent<Button>();
    }

    public override void HandleIn()
    {
        button.Select(); // selects the button
    }

    public override void HandleOut()
    {
        if(EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null); // deselects the button
    }

    public void OnClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public override void SelectionCompleted()
    {
        if (InteractiveItem.IsOver)
        {
            button.onClick.Invoke();
        }
    }
    public override void SelectionStarted()
    {
    }

    public override void SelectionCanceled()
    {
    }
}
