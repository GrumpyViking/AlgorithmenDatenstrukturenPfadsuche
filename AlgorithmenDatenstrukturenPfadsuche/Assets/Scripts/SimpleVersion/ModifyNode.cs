using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyNode 
{
    public bool ChangeColor(GameObject Cell, Color color)
    {
        Cell.GetComponent<Renderer>().material.color = color;
        return true;
    }
}
