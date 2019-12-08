using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonBackToMenu : EyeTribe.Unity.Interaction.InteractionHandler
{
    private Button button;
    public override void Awake()
    {
        base.Awake();
        button = gameObject.GetComponent<Button>();
    }

    public override void HandleIn()
    {
        button.Select(); // selects the button
    }

    public override void HandleOut()
    {
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null); // deselects the button
    }

    public void OnClick()
    {
        SceneManager.LoadScene("MainMenu");
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
