using System.Collections.Generic;
using UnityEngine;

namespace ComplexVersion {
    public class CreateGrid : MonoBehaviour {
        private float cellDiameter;
        public float cellSize, distanceOfCells;
        public Vector2 gridSize;
        private int fieldSizeXAxis, fieldSizeYAxis;
        private Node[,] fieldCellArray;
        public LayerMask obstacle;
        public List<Node> path; // Speichert den Zielpfad

        private void Start() {
            cellDiameter = cellSize * 2;
            fieldSizeXAxis = Mathf.RoundToInt(gridSize.x / cellDiameter);
            fieldSizeYAxis = Mathf.RoundToInt(gridSize.y / cellDiameter);
        }

        void Update() {
            CreateOverlayGrid(); // Aktualisiert das Grid mit jedem Frame 
        }

        // Legt über das Spielfeld ein unsichtbares Grid damit der Pfad berechnet werden kann
        private void CreateOverlayGrid() {
            fieldCellArray = new Node[fieldSizeXAxis, fieldSizeYAxis];
            Vector3 bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
            for (int x = 0; x < fieldSizeXAxis; x++) {
                for (int y = 0; y < fieldSizeYAxis; y++) {
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * cellDiameter + cellSize) + Vector3.forward * (y * cellDiameter + cellSize);
                    bool wall = !Physics.CheckSphere(worldPoint, cellSize, obstacle);

                    fieldCellArray[x, y] = new Node(wall, worldPoint, x, y, null);
                }
            }
        }

        // Liefer die Nachbarfelder des aktuellen Feldes
        public List<Node> GetNeighboringNodes(Node current) {
            List<Node> neighbors = new List<Node>();
            int checkXAxis; // prüfen ob Feld noch im gültigen X-Achsen Rahmen ist 
            int checkYAxis; // prüfen ob Feld noch im gültigen Y-Achsen Rahmen ist

            //oberer-Nachbar
            checkXAxis = current.cordX;
            checkYAxis = current.cordY + 1;
            if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis) {
                if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis) {
                    neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
                }
            }

            //rechter-Nachbar
            checkXAxis = current.cordX + 1;
            checkYAxis = current.cordY;
            if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis) {
                if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis) {
                    neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
                }
            }

            //unterer-Nachbar
            checkXAxis = current.cordX;
            checkYAxis = current.cordY - 1;
            if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis) {
                if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis) {
                    neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
                }
            }

            //linker-Nachbar
            checkXAxis = current.cordX - 1;
            checkYAxis = current.cordY;
            if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis) {
                if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis) {
                    neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
                }
            }

            return neighbors;
        }

        // Liefert das fieldCellArray zurück
        public Node[,] GetArray() {
            return fieldCellArray;
        }

        // Ziel und Start können freibewegt werden, die Funktion NodeFromGlobalPosition sucht zu der Position die entsprechende Node im array
        public Node NodeFromGlobalPosition(Vector3 globalPos) {
            float posXAxis = (globalPos.x + gridSize.x / 2) / gridSize.x;
            float posYAxis = (globalPos.z + gridSize.y / 2) / gridSize.y;

            // Clamp01 liefer bei negativen Zahlen 0 und bei positiven Zahlen größer als 1 1
            posXAxis = Mathf.Clamp01(posXAxis);
            posYAxis = Mathf.Clamp01(posYAxis);

            int cordX = Mathf.RoundToInt((fieldSizeXAxis - 1) * posXAxis);
            int cordY = Mathf.RoundToInt((fieldSizeYAxis - 1) * posYAxis);

            return fieldCellArray[cordX, cordY];
        }

        // Ermöglicht die Anzeige des Pfades ohne Einfluss auf das Spielgeschehen zu nehmen
        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 0f, gridSize.y));
            if (fieldCellArray != null) {
                foreach (Node node in fieldCellArray) {
                    if (node.traversable) {
                        Gizmos.color = Color.white;
                    } else {
                        Gizmos.color = Color.yellow;
                    }
                    if (path != null) {
                        if (path.Contains(node)) {
                            Gizmos.color = Color.green;
                        }
                    }
                    Gizmos.DrawCube(node.nodeGlobalPosition, Vector3.one * (cellDiameter - distanceOfCells));
                }
            }
        }
    }
}