using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Die ModifyNode Klasse ermöglicht es die Materialfarbe eines Objektes zu ändern
 */
public class ModifyNode 
{
    //Funktion bekommt das Objekt und eine Farbe übergeben
    public bool ChangeColor(GameObject Cell, Color color)
    {
        Cell.GetComponent<Renderer>().material.color = color;
        return true;
    }
}
