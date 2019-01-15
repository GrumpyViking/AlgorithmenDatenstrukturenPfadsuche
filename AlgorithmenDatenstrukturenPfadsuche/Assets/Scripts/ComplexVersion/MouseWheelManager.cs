using UnityEngine;
using UnityEditor;
using System.Collections;

public class MouseWheelManager : MonoBehaviour {
    private bool mouseWheelAvailable;

    void Start() {
        mouseWheelAvailable = true;
    }
    public void SetAvailability(bool state) {
        mouseWheelAvailable = state;
    }

    public bool GetAvailability() {
        return mouseWheelAvailable;
    }
}
