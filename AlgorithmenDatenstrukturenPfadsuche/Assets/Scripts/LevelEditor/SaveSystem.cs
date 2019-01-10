using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    // Application.persistentDataPath
    private static string savePath = "C:/SavePath";

    public static void SaveData(Node[,] grid) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = savePath + "/level.grid";
        FileStream stream = new FileStream(path, FileMode.Create);

        SavableData saveData = new SavableData(grid);

        formatter.Serialize(stream, saveData);

        stream.Close();
    }

    public static SavableData LoadLevel() {
        string path = savePath + "/level.grid";
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
