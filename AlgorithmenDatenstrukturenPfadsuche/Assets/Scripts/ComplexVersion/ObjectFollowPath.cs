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

        void Start() {
            this.transform.position = GameObject.Find("StartPosition").transform.position; // Ändert die Position des Objektes auf den Startpunkt
            oldStartPosition = GameObject.Find("StartPosition").transform.position; // Speichert Startpunkt Position
            oldTargetPosition = GameObject.Find("TargetPosition").transform.position; // Speichert Zielpunkt Position
        }

        // Update is called once per frame
        void Update() {
            // Bei bewegung des Startpunktes wird das Objekt mit verschoben
            if (GameObject.Find("StartPosition").transform.position != oldStartPosition) {
                this.transform.position = GameObject.Find("StartPosition").transform.position;
                oldStartPosition = GameObject.Find("StartPosition").transform.position;
                index = 0; // Setzt den Index zuück um ArrayOutofBounce fehler zu vermeiden, bei änderung des Startpunktes
            }

            if (GameObject.Find("TargetPosition").transform.position != oldTargetPosition) {
                this.transform.position = GameObject.Find("StartPosition").transform.position;
                oldTargetPosition = GameObject.Find("TargetPosition").transform.position;
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
                    transform.position = Vector3.MoveTowards(transform.position, g.path[index].nodeGlobalPosition, step);
                    if (index == g.path.Count - 1) {
                        reachedGoal = true;
                    } else {
                        index++;
                    }
                } else {
                    transform.position = Vector3.MoveTowards(transform.position, g.path[index].nodeGlobalPosition, step);
                    if (index == 0) {
                        reachedGoal = false;
                    } else {
                        index--;
                    }
                }
            }
        }
    }
}