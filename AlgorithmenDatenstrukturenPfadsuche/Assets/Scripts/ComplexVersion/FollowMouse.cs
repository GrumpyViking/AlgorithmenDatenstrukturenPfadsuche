using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector2 mousePosition;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        mousePosition.x = Input.mousePosition.x;
        mousePosition.y = Input.mousePosition.z;
        
        transform.position = mousePosition/100;

        if (Input.GetMouseButtonDown(0))
        {
            Destroy(GetComponent<FollowMouse>());
        }
    }
}
