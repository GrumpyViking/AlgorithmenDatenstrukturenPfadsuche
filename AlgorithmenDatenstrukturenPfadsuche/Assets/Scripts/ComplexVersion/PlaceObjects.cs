using UnityEngine;
using UnityEngine.UI;

public class PlaceObjects : MonoBehaviour
{
    public GameObject cube, cylinder;
    private GameObject obstacle;
    
    public void PlaceObject(Text objectToPlace)
    {
        switch (objectToPlace.text)
        {
            case "Cube":
                obstacle = Instantiate(cube, new Vector3(0, 0, 0), Quaternion.identity);
                obstacle.layer = 30;
                obstacle.GetComponent<Renderer>().material.color = Color.yellow;
                obstacle.AddComponent<FollowMouse>();
                break;
            case  "LargeCube":
                obstacle = Instantiate(cube, new Vector3(0, 0, 0),
                    Quaternion.identity);
                obstacle.transform.localScale += new Vector3(2f,0,2f);
                obstacle.layer = 30;
                obstacle.GetComponent<Renderer>().material.color = Color.yellow;
                obstacle.AddComponent<FollowMouse>();
                break;
            case "Rectangle":
                obstacle = Instantiate(cube, new Vector3(0, 0, 0),
                    Quaternion.identity);
                obstacle.transform.localScale += new Vector3(0,0,3f);
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
                Debug.Log("fehler im switch-case PlaceObject");
                break;
        }
    }
}
