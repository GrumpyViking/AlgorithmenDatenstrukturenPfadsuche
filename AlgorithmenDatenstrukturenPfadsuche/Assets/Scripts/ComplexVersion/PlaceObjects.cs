using ComplexVersion;
using UnityEngine;
using UnityEngine.UI;

public class PlaceObjects : MonoBehaviour
{
    /*
     * PlaceObject Klasse fügt Hindernisse dem Spielfeld hinzu
     * Aufruf erfolgt über die Button an der linken Seite
     * je nach gedrückten Button wird ein entsprechendes Objekt erstellt
     */
    
    public GameObject cube, cylinder; // Objekte müssen in Unity Editor zugewiesen werden 
    private GameObject obstacle;
    
    public void PlaceObject(Text objectToPlace)
    {
        switch (objectToPlace.text)
        {
            case "Cube":
                obstacle = Instantiate(cube, new Vector3(0, 0, 0), Quaternion.identity); // Erstllet Objekt
                obstacle.layer = 30; // Fügt Objekt dem "Obstacle" Layer hinzu (relevant für erkennung im A*-Algoritmus)
                obstacle.GetComponent<Renderer>().material.color = Color.yellow; // Ändert die Farbe des Objektes
                obstacle.AddComponent<FollowMouse>(); // Fügt ein Skript dem Objekt hinzu, bis zum ersten Mausklick folgt das Objekt der Mausbewegung des Nutzers
                break;
            case  "LargeCube":
                obstacle = Instantiate(cube, new Vector3(0, 0, 0),
                    Quaternion.identity);
                obstacle.transform.localScale += new Vector3(2f,0,2f); // Skaliert das cube Objekt in der x und y Achse um Faktor 2
                obstacle.layer = 30;
                obstacle.GetComponent<Renderer>().material.color = Color.yellow;
                obstacle.AddComponent<FollowMouse>();
                break;
            case "Rectangle":
                obstacle = Instantiate(cube, new Vector3(0, 0, 0),
                    Quaternion.identity);
                obstacle.transform.localScale += new Vector3(0,0,3f); // Skaliert das cube Objekt in der z Achse um faktor 3
                obstacle.layer = 30;
                obstacle.GetComponent<Renderer>().material.color = Color.yellow;
                obstacle.AddComponent<FollowMouse>();
                break;
            case "Cylinder":
                obstacle = Instantiate(cylinder, new Vector3(0, 0, 0),
                    Quaternion.identity);
                obstacle.layer = 30;
                obstacle.GetComponent<Renderer>().material.color = Color.yellow;
                obstacle.AddComponent<FollowMouse>();
                break;
            case "LargeCylinder":
                obstacle = Instantiate(cylinder, new Vector3(0, 0, 0),
                    Quaternion.identity);
                obstacle.transform.localScale += new Vector3(2f,0,2f);
                obstacle.layer = 30;
                obstacle.GetComponent<Renderer>().material.color = Color.yellow;
                obstacle.AddComponent<FollowMouse>();
                break;
            default:
                Debug.Log("Fehler im switch-case PlaceObject.");
                break;
        }
    }
}
