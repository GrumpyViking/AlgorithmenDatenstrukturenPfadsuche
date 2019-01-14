using UnityEngine;

namespace ComplexVersion {
    public class ObjectFollowPath : MonoBehaviour {
        /*
        * ObjectFollowPath Klasse ermöglicht es Objekten den durch den A*-Algoritmus berechneten Pfad zu folgen
        */

        private Vector3 oldStartPosition, oldTargetPosition; // relevant für änderung des Start und Zielpunktes zur Laufzeit
        public CreateGrid g; // ermöglicht Zugriff auf den Pfad zum Ziel 
        private bool moving, reachedGoal; // Status variablen für den Ablauf des Programms
        public float speed; // Geschwindigkeit des Objetes
        private int index; // zähler variable um Pfadliste schritt für schritt nachzuverfolgen
        public GameObject startPosition, targetPosition;

        void Start() {
            this.transform.position = startPosition.transform.position; // Ändert die Position des Objektes auf den Startpunkt
            oldStartPosition = startPosition.transform.position; // Speichert Startpunkt Position
            oldTargetPosition = targetPosition.transform.position; // Speichert Zielpunkt Position
        }

        // Update is called once per frame
        void Update() {
            // Bei bewegung des Startpunktes wird das Objekt mit verschoben
            if (startPosition.transform.position != oldStartPosition) {
                this.transform.position = startPosition.transform.position;
                oldStartPosition = startPosition.transform.position;
                index = 0; // Setzt den Index zuück um ArrayOutofBounce fehler zu vermeiden, bei änderung des Startpunktes
            }

            if (targetPosition.transform.position != oldTargetPosition) {
                this.transform.position = startPosition.transform.position;
                oldTargetPosition = targetPosition.transform.position;
                index = 0;
            }

            // Startet die Bewegung des Objektes
            if (Input.GetKeyDown(KeyCode.Space) && moving == false) {
                moving = true;
            }
            // TODO: in extra Funktion ausgleidern
            if (moving) {
                float step = speed * Time.deltaTime;
                if (!reachedGoal) {
                    transform.position = Vector3.MoveTowards(transform.position, g.path[index].GetGlobalPosition(), step);
                    RotateShip(g.path[index].GetGlobalPosition());
                    if (index == g.path.Count - 1) {
                        reachedGoal = true;
                    } else {
                        if (transform.position == g.path[index].GetGlobalPosition()) {
                            index++;
                        }
                    }
                } else {
                    transform.position = Vector3.MoveTowards(transform.position, g.path[index].GetGlobalPosition(), step);
                    RotateShip(g.path[index].GetGlobalPosition());
                    if (index == 0) {
                        reachedGoal = false;
                    } else {
                        if (transform.position == g.path[index].GetGlobalPosition()) {
                            index--;
                        }
                    }

                }
            }
        }

        void RotateShip(Vector3 target) {

            Vector3 targetDir = target - transform.position;
            Debug.Log(targetDir);
            // The step size is equal to speed times frame time.
            float step = speed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }
}