using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public static class SaveSystem {
    // Application.persistentDataPath
    private static string savePath = Application.dataPath + "/levels";

    public static string levelName;

    public static void SetName(Text name)
    {
        levelName = name.text;
    }
    
    public static void SaveData(List<LevelData> data) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = savePath + "/" + levelName + ".grid";
        FileStream stream = new FileStream(path, FileMode.Create);

        SavableData saveData = new SavableData(data);

        formatter.Serialize(stream, saveData);

        stream.Close();
    }

    public static SavableData LoadLevel(string filename) {
        string path = savePath + "/" + filename;
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SavableData data = formatter.Deserialize(stream) as SavableData;
            stream.Close();
            return data;
        } else {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
