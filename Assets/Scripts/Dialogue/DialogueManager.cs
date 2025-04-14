using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    private Dictionary<string, string> _dialogueDict = new();
    private const string _dialogueFolder = "/Scripts/Dialogue";

    public GameObject panel;

    void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        LoadDialogueTextFileFromCsv();

        panel.SetActive(false);
    }

    private void LoadDialogueTextFileFromCsv()
    {
        string fileName = "Dialogue.csv";
        string[] lines = null;
        lines = FileManager.LoadFileAsText(fileName, _dialogueFolder).Split("\n"[0]);
        string[] columns;

        for (int i = 0; i < lines.Length; i++)
        {
            if (!lines[i].Contains(",")) continue; //Ignore empty lines

            //Split the key and value and store them seperately.
            columns = lines[i].Split(new[] { ',' }, 2);
            columns[1] = columns[1].Trim('"');

            //Add seperated key and value into the dictionary, if already present, throw error.
            if (!_dialogueDict.TryAdd(columns[0], columns[1].TrimEnd('"', '\r', '\n')))
            {
                Debug.LogError($"duplicate entry found in {fileName} with key {columns[0]} on line {i + 1}");
            }
        }
    }

    public string GetDialogueValue(string key)
    {
        var result = key;

        if (!_dialogueDict.ContainsKey(key))
            return result;

        result = _dialogueDict[key];
        return result;
    }
    

    public void EnableDialogueBox()
    {
        panel.SetActive(true);
    }

    public void DisableDialogueBox()
    {
        panel.SetActive(false);
    }
}
