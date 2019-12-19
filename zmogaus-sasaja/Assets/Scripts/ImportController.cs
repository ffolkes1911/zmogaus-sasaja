using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImportController : MonoBehaviour {
    [SerializeField] private Text directoryText;
    [SerializeField] private Text levelInfoText;
    [SerializeField] private Text levelListText;
    [SerializeField] private Text previewLevelName;

	// Use this for initialization
	void Start ()
    {
        directoryText.text = Application.persistentDataPath;
        levelListText.text = "";
        foreach (string file in System.IO.Directory.GetFiles(Application.persistentDataPath))
        {
            string[] name = file.Split('\\');
            levelListText.text += name[name.Length - 1] + "\n";
        }
    }

    public void PreviewLevel()
    {
        Debug.Log(previewLevelName.text);
        StreamReader reader = new StreamReader(Application.persistentDataPath + "/" + previewLevelName.text);
        string levelText = reader.ReadToEnd();
        Debug.Log(levelText);
        levelInfoText.text = levelText;
    }

    public void ImportLevel()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Pritaikytas"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Pritaikytas");
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/Pritaikytas/" + previewLevelName.text);
        writer.Write(levelInfoText.text);
        writer.Close();
        SceneManager.LoadScene("MainMenu");
    }
}
