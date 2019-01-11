using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("NodeArray")]
public class SavableData {

    [XmlArray("Nodes"), XmlArrayItem("Node")]
    public Node[] grid;
    public SavableData() {

    }
    public SavableData(Node[,] newGrid) {
        grid = new Node[newGrid.GetLength(0) * newGrid.GetLength(1)];
        int k = 0;
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 2; j++) {
                grid[k++] = newGrid[i, j];
            }
        }
    }

    public void SaveLevel() {
        var savePath = "C:/SavePath/level.xml";
        var serializer = new XmlSerializer(typeof(SavableData));
        var stream = new FileStream(savePath, FileMode.Create);
        serializer.Serialize(stream, this);
        stream.Close();
    }

}
