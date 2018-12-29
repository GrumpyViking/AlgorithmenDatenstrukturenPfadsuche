using UnityEngine;

public class MoveStart : MonoBehaviour
{
    private void Update()
    {
            if (Input.GetKey(KeyCode.W)) transform.position += Vector3.forward; // Bewegt den Startpunkt nach vorne
            if (Input.GetKey(KeyCode.D)) transform.position += Vector3.right; // Bewegt den Startpunkt nach rechts    
            if (Input.GetKey(KeyCode.S)) transform.position += Vector3.back; // Bewegt den Startpunkt nach hinten
            if (Input.GetKey(KeyCode.A)) transform.position += Vector3.left; // Bewegt den Startpunkt nach links  
    }
}