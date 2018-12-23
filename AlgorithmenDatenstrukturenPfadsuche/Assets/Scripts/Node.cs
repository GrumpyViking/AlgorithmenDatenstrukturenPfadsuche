using UnityEngine;

public class Node
{
    public int cordX; // X Position im NodeArray
    public int cordY; // Y Position im NodeArray

    public int gCost; // Distanz zum Start
    public int hCost; // Distanz zum Ziel
    public Vector3 nodeGlobalPosition; // Weltkoordinaten der Node.

    public Node parent; // Von wo kam der aktuelle Knoten (wichtig für Rückverfolgung)

    public bool traversable; // Gibt an ob Node begehbar ist.

    public Node(bool _traversable, Vector3 _nodeGlobalPosition, int _cordX, int _cordY) // Konstruktor
    {
        traversable = _traversable;
        nodeGlobalPosition = _nodeGlobalPosition;
        cordX = _cordX;
        cordY = _cordY;
    }

    public int fCost
    {
        get { return gCost + hCost; } // Gesamtkosten der Node zum Ziel
    }
}