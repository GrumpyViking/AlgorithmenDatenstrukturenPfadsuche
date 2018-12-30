using UnityEngine;

public class Node
{
    public int cordX; // X Position im NodeArray
    public int cordY; // Y Position im NodeArray

    public int gCost; // Distanz zum Start
    public int hCost; // Distanz zum Ziel
    public Vector3 nodeGlobalPosition; // Weltkoordinaten der Node.

    public Node parent; // Von wo kam der aktuelle Knoten (wichtig für Rückverfolgung)
    public GameObject fieldCell;
    public bool traversable, start, target, visited; // Gibt an ob Node begehbar, ein Startpunkt/endpunkt und bereits untersucht worden ist.
    public Node(bool traversable, Vector3 nodeGlobalPosition, int cordX, int cordY, GameObject fieldCell) // Konstruktor
    {
        this.traversable = traversable;
        this.nodeGlobalPosition = nodeGlobalPosition;
        this.cordX = cordX;
        this.cordY = cordY;
        this.fieldCell = fieldCell;
    }

    public int fCost
    {
        get { return gCost + hCost; } // Gesamtkosten der Node zum Ziel
    }
}