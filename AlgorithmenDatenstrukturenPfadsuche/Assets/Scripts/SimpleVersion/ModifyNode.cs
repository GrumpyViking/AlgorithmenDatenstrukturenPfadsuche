using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Die ModifyNode Klasse ermöglicht es die Materialfarbe eines Objektes zu ändern
 *
 * Martin Schuster
 */
public class ModifyNode {
    //Funktion bekommt das Objekt und eine Farbe übergeben und ändert die Farbe des Kind Objektes entsprechend (Feldobjekte in der SimpleVersion und Example Szene sind entsprechend aufgebaut)
    public void ChangeColorChild(GameObject Cell, Color color) {
        Cell.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
    }

    //Funktion bekommt das Objekt und eine Farbe übergeben und ändert die Farbe Objektes entsprechend
    public void ChangeColorObject(GameObject Cell, Color color) {
        Cell.GetComponent<Renderer>().material.color = color;
    }

}
