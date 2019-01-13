using UnityEngine;
using UnityEditor;
using System.Collections;

public class MouseWheelManager : MonoBehaviour {
    private bool mouseWheelAvailable;

    public void SetAvailability(bool state) {
        mouseWheelAvailable = state;
    }

    public bool GetAvailability() {
        return mouseWheelAvailable;
    }
}
