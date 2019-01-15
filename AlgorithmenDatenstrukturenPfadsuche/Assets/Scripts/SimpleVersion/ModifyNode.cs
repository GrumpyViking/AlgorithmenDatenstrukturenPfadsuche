using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Die ModifyNode Klasse ermöglicht es die Materialfarbe eines Objektes zu ändern
 */
public class ModifyNode {
    //Funktion bekommt das Objekt und eine Farbe übergeben und ändert die Farbe entsprechend
    public void ChangeColor(GameObject Cell, Color color) {
        Cell.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
    }
}
