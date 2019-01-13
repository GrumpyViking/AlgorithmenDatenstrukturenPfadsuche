using UnityEngine;

public class Node {
    public int cordX; // X Position im NodeArray
    public int cordY; // Y Position im NodeArray
    public int gCost; // Distanz zum Start
    public int hCost; // Distanz zum Ziel
    public int index;
    public float[] nodeGlobalPosition = new float[3]; // Weltkoordinaten der Node.

    public Node parent; // Von wo kam der aktuelle Knoten (wichtig für Rückverfolgung)
    public GameObject fieldCell;
    public bool traversable, start, target, visited; // Gibt an ob Node begehbar, ein Startpunkt/endpunkt und bereits untersucht worden ist.
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

    public Node() {

    }

    public int fCost {
        get { return gCost + hCost; } // Gesamtkosten der Node zum Ziel
    }

    public Vector3 GetGlobalPosition() {
        Vector3 pos = new Vector3();
        pos.x = nodeGlobalPosition[0];
        pos.y = nodeGlobalPosition[1];
        pos.z = nodeGlobalPosition[2];
        return pos;
    }
}