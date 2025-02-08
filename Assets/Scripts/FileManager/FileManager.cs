using System.IO;
using UnityEngine;

public static class FileManager
{
#if UNITY_EDITOR
    public static readonly string DataPath = Application.dataPath;
#else
    public static readonly string DataPath = Application.persistentDataPath;
#endif


    public static string LoadFileAsText(string fileName, string folder)
    {
        string folderPath = Path.Combine(DataPath + folder);
        string filePath = Path.Combine(folderPath, fileName);

        //Debug.Log(folderPath);
        //Debug.Log(filePath);
        string dataAsText = null;

        if (File.Exists(filePath))
        {
            dataAsText = File.ReadAllText(filePath);
            return dataAsText;
        }

        Debug.LogError("<color=red> File not found. </color>");
        return null;
    }

}
