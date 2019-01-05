using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeaphtFirstSearch : MonoBehaviour
{
    private CreateField grid;
    // Start is called before the first frame update
    void Awake()
    {
        grid = GetComponent<CreateField>();
    }
}
