using UnityEngine;
using UnityEditor;
using System.Collections;

/**
 * MouseWheelManager nötig da das Zoomen der Kamera und das Drehen der platzierbaren Objekte beide mittels des Mausrades geschehen
 *
 * Martin Schuster
 */
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
