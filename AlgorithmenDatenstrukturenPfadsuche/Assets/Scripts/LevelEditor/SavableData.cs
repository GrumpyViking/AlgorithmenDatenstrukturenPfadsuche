using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavableData {

    public Node[,] grid;
    public SavableData(Node[,] newGrid) {
        grid = newGrid;
    }
}
