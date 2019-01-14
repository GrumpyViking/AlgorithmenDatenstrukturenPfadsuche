using UnityEngine;
/*
    Beinhaltet die relevanten Informationen damit die Such-Algorithmen arbeiten können.

 */
public class Node {
    public int cordX; // X Position im NodeArray
    public int cordY; // Y Position im NodeArray
    public int gCost; // Distanz zum Start
    public int hCost; // Distanz zum Ziel
    public int index;
    public float[] nodeGlobalPosition = new float[3]; // Weltkoordinaten der Node als float Array.
    public Node parent; // Von wo kam der aktuelle Knoten (wichtig für Rückverfolgung)
    public GameObject fieldCell;
    public bool traversable, start, target, visited; // Gibt an ob Node begehbar, ein Startpunkt/endpunkt und bereits untersucht worden ist.

    // Konstruktor initialisiert einen neuen Node mit den übergebenen Werten
    public Node(bool traversable, Vector3 nodeGlobalPosition, int cordX, int cordY, GameObject fieldCell) // Konstruktor
    {
        this.traversable = traversable;
        this.nodeGlobalPosition[0] = nodeGlobalPosition.x;
        this.nodeGlobalPosition[1] = nodeGlobalPosition.y;
        this.nodeGlobalPosition[2] = nodeGlobalPosition.z;
        this.cordX = cordX;
        this.cordY = cordY;
        this.fieldCell = fieldCell;
    }

    // Gesamtkosten der Node zum Ziel als Methode da der Wert nicht gespeichert werden muss
    public int fCost {
        get { return gCost + hCost; }
    }

    // Erstellt aus einem float Array ein Vector3 Objekt
    public Vector3 GetGlobalPosition() {
        Vector3 pos = new Vector3();
        pos.x = nodeGlobalPosition[0];
        pos.y = nodeGlobalPosition[1];
        pos.z = nodeGlobalPosition[2];
        return pos;
    }
}