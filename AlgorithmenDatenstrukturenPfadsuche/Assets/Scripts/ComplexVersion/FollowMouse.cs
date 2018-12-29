using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
		// Quelle: https://forum.unity.com/threads/solved-moving-gameobject-along-x-and-z-axis-by-drag-and-drop-using-x-and-y-from-screenspace.488476/#post-3185720
        float planeY = 0;
        Transform draggingObject = transform;
         
        Plane plane = new Plane(Vector3.up, Vector3.up * planeY); // ground plane
        
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         
        float distance; // the distance from the ray origin to the ray intersection of the plane
        if(plane.Raycast(ray, out distance))
        {
            draggingObject.position = ray.GetPoint(distance); // distance along the ray
        }
		
		
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(GetComponent<FollowMouse>());
            
        }
    }
}
