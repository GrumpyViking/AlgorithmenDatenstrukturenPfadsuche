using UnityEngine;
using UnityEngine.UI;

public class PlaceObjects : MonoBehaviour
{
    public GameObject cube;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void PlaceObject(Text objectToPlace)
    {
        switch (objectToPlace.text)
        {
            case "Cube":
                GameObject obstacle = Instantiate(cube, new Vector3(0, 0, 0), Quaternion.identity);
                obstacle.layer = 30;
                obstacle.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            default:
                Debug.Log("fehler im switch-case PlaceObject");
                break;
        }
    }
}
