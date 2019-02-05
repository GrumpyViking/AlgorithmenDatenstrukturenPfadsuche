using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Speichert eine Liste aus LevelData Objekte aus dieser das Spielfeld wiederhergestellt werden kann.
 * 
 * Martin Schuster
 */

[System.Serializable]
public class SavableData {

    // Liste der veränderten Nodes als LevelData Objekte
    public List<LevelData> saveNodes;

    // Konstruktor
    public SavableData(List<LevelData> nodes) {
        saveNodes = nodes;
    }
}
