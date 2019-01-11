using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavableData {

    public List<LevelData> saveNodes;

    public SavableData(List<LevelData> nodes) {
        saveNodes = nodes;
    }
}
