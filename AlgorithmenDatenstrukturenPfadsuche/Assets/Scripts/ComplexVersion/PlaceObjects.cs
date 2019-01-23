using ComplexVersion;
using UnityEngine;
using UnityEngine.UI;

public class PlaceObjects : MonoBehaviour {
    /**
     * PlaceObject Klasse fügt Hindernisse dem Spielfeld hinzu
     * Aufruf erfolgt über die Button an der linken Seite
     * je nach gedrückten Button wird ein entsprechendes Objekt erstellt
     *
     * Martin Schuster
     */

    public GameObject island, islandbig, barrel, kraken, wrack, cliff; // Objekte müssen in Unity Editor zugewiesen werden 
    private GameObject obstacle;
    private MouseWheelManager mouseWheelManager;

    void Awake() {
        mouseWheelManager = GameObject.Find("MouseWheelManager").GetComponent<MouseWheelManager>();
    }

    public void PlaceObject(Text objectToPlace) {
        switch (objectToPlace.text) {
            case "Klippe":
                obstacle = Instantiate(cliff, new Vector3(0, 0, 0), Quaternion.identity); // Erstllet Objekt
                obstacle.layer = 30; // Fügt Objekt dem "Obstacle" Layer hinzu (relevant für erkennung im A*-Algoritmus)
                obstacle.AddComponent<FollowMouse>();
                mouseWheelManager.SetAvailability(false);
                break;
            case "Wrack":
                obstacle = Instantiate(wrack, new Vector3(0, 0, 0), Quaternion.identity);
                obstacle.layer = 30;
                obstacle.AddComponent<FollowMouse>();
                mouseWheelManager.SetAvailability(false);
                break;
            case "Kraken":
                obstacle = Instantiate(kraken, new Vector3(0, 0, 0), Quaternion.identity);
                obstacle.layer = 30;
                obstacle.AddComponent<FollowMouse>();
                mouseWheelManager.SetAvailability(false);
                break;
            case "Fass":
                obstacle = Instantiate(barrel, new Vector3(0, 0, 0),
                    Quaternion.identity);
                obstacle.layer = 30;
                obstacle.AddComponent<FollowMouse>();
                mouseWheelManager.SetAvailability(false);
                break;
            case "Insel (klein)":
                obstacle = Instantiate(island, new Vector3(0, 0, 0),
                    Quaternion.identity);
                obstacle.layer = 30;
                obstacle.AddComponent<FollowMouse>();
                mouseWheelManager.SetAvailability(false);
                break;
            case "Insel (groß)":
                obstacle = Instantiate(islandbig, new Vector3(0, 0, 0),
                    Quaternion.identity);
                obstacle.layer = 30;
                obstacle.AddComponent<FollowMouse>();
                mouseWheelManager.SetAvailability(false);
                break;
            default:
                Debug.Log("Fehler im switch-case PlaceObject.");
                break;
        }
    }
}
