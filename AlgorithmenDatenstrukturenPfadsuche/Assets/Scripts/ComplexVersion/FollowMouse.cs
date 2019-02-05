using UnityEngine;

namespace ComplexVersion {
    public class FollowMouse : MonoBehaviour {
        /**
         * Objekt folgt der Mausbewegung des Nutzers und wird innerhalb der Spielfeldgrenzen Platziert
         *
         * Martin Schuster
         */
        float speed = 200f; // Geschwindigkeit der Rotation von Objekten
        private MouseWheelManager mouseWheelManager;
        void Awake() {
            mouseWheelManager = GameObject.Find("MouseWheelManager").GetComponent<MouseWheelManager>();
        }
        void Update() {
            // Quelle: https://forum.unity.com/threads/solved-moving-gameobject-along-x-and-z-axis-by-drag-and-drop-using-x-and-y-from-screenspace.488476/#post-3185720
            float planeY = 0;

            Plane plane = new Plane(Vector3.up, Vector3.up * planeY); // ground plane

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance; // the distance from the ray origin to the ray intersection of the plane
            if (plane.Raycast(ray, out distance)) {
                // distance along the ray
                transform.position = ray.GetPoint(distance);

                // Rotation des Objektes
                if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                    transform.Rotate(Vector3.up * speed * Time.deltaTime);
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                    transform.Rotate(-Vector3.up * speed * Time.deltaTime);
                }

            }

            // Das Objekt wird bei Mausklick platziert in dem das Skript vom Objekt entfernt wird
            if (Input.GetMouseButtonDown(0) && ObjectInPlayField()) {

                mouseWheelManager.SetAvailability(true);
                Destroy(GetComponent<FollowMouse>());
            }
        }

        bool ObjectInPlayField() {
            int boarderLeft = -48;
            int boarderRight = 48;
            int boarderTop = 48;
            int boarderBottom = -48;

            if ((transform.position.x < boarderRight) && (transform.position.x > boarderLeft) && (transform.position.z > boarderBottom) && (transform.position.z < boarderTop)) {
                return true;
            }

            return false;
        }
    }
}
