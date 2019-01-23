using UnityEngine;

namespace ComplexVersion {
    public class MoveStart : MonoBehaviour {
        /**
         * Ermöglicht den Start zu bewegen
         *
         * Martin Schuster
         */
        private void Update() {
            if (Input.GetKey(KeyCode.W) && (transform.position.z <= 47)) transform.position += Vector3.forward; // Bewegt den Startpunkt nach vorne
            if (Input.GetKey(KeyCode.D) && (transform.position.x <= 47)) transform.position += Vector3.right; // Bewegt den Startpunkt nach rechts    
            if (Input.GetKey(KeyCode.S) && (transform.position.z >= -47)) transform.position += Vector3.back; // Bewegt den Startpunkt nach hinten
            if (Input.GetKey(KeyCode.A) && (transform.position.x >= -47)) transform.position += Vector3.left; // Bewegt den Startpunkt nach links  
        }
    }
}