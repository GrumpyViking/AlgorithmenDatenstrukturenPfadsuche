using UnityEngine;

namespace ComplexVersion
{
    public class FollowMouse : MonoBehaviour
    {
        // TODO: Grenzen des Spielfelds einhalten
        /*
         * Objekt folgt der Mausbewegung des Nutzers
         */
        
        float speed = 200f; // Geschwindigkeit der Rotation von Objekten
        void Update()
        {
            // Quelle: https://forum.unity.com/threads/solved-moving-gameobject-along-x-and-z-axis-by-drag-and-drop-using-x-and-y-from-screenspace.488476/#post-3185720
            float planeY = 0;
         
            Plane plane = new Plane(Vector3.up, Vector3.up * planeY); // ground plane
        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         
            float distance; // the distance from the ray origin to the ray intersection of the plane
            if(plane.Raycast(ray, out distance))
            {
                // distance along the ray
                transform.position = ray.GetPoint(distance); 
                
                // Rotation des Objektes
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    transform.Rotate(Vector3.up * speed * Time.deltaTime);
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    transform.Rotate(-Vector3.up * speed * Time.deltaTime);
                }
            }
    
            // Das Objekt wird bei Mausklick platziert in dem das Skript vom Objekt entfernt wird
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(GetComponent<FollowMouse>());
            }
        }
    }
}
