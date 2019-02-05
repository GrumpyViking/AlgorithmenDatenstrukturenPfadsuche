using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * LevelData speichert die Relevanten Informationen einer Node langfristig
 * musste extra ausgegliedert werden da Unity eigene Objekte wie Vector3, GameObject usw. 
 * nicht ohne weiteres Serializiert werden können.
 * Es werden die wichtigen Eigenschaften welche zum wiederherstellen der Nodes gespeichert.
 *
 * Martin Schuster
 */

[System.Serializable]
public class LevelData {
    // X-, Y-Koordinate, Index der Node
    public int xCord, yCord, index;
    // Status ob Start, Ziel und ob begehbar ist
    public bool start, target, traversable;
    // Konstruktor
    public LevelData(Node grid) {
        xCord = grid.cordX;
        yCord = grid.cordY;
        start = grid.start;
        target = grid.target;
        traversable = grid.traversable;
    }
}
