using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleManager : MonoBehaviour {
    void Awake() {
        switch (PlayerSceneData.lastScene) {
            case 5:
                Debug.Log("Tiefensuche");
                break;
            case 6:
                Debug.Log("Breitensuche");
                break;
            case 7:
                Debug.Log("A*");
                break;
            default:
                Debug.Log("Fehler im ExampleMode!");
                break;
        }
    }

}
