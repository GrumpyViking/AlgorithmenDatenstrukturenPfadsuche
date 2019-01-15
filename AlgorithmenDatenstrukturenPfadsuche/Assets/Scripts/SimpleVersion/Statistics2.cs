using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics2 : MonoBehaviour {

    public Text visited;
    public Text time;
    public Text pathLength;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void setVisited(int numberOfVisitedFields) {
        visited.text += " " + numberOfVisitedFields.ToString();
    }

    public void setPathLength(int pathLength) {
        this.pathLength.text += " " + pathLength.ToString();
    }
}
