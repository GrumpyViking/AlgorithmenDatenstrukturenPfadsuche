using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData {
    public int xCord, yCord;

    public bool start, target, traversable;

    public LevelData(Node grid) {
        xCord = grid.cordX;
        yCord = grid.cordY;
        start = grid.start;
        target = grid.target;
        traversable = grid.traversable;
    }
}
