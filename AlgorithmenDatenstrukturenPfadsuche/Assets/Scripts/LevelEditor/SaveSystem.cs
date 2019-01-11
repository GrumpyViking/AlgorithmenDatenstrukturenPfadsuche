using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    // Application.persistentDataPath
    private static string savePath = "C:/SavePath";

    public static void SaveData(LevelData[] data) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = savePath + "/level.grid";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData saveData = new LevelData(data);

        formatter.Serialize(stream, saveData);

        stream.Close();
    }

    public static LevelData LoadLevel() {
        string path = savePath + "/level.grid";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();
            return data;
        } else {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
