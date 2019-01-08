using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Internal.Experimental.UIElements;
using UnityEngine.UI;

/*
 * CreatField Klasse erstellt das Spielfeld 
 */
public class CreateField : MonoBehaviour
{
    public GameObject fieldCell; // Objekt aus dem das Feldbesteht (in Unity hinzufügen)
    public Vector2 fieldSize; // Größe des Spielfeldes (in Unity eintragen)
    private float fieldCellSize = 0.5f; // größe der einzelnen Felder
    private int fieldSizeXAxis, fieldSizeYAxis; // anzahl Felder auf der X bzw. Y Achse
    private float fieldCellDiameter; // Für Korrekte Feldgrößen berechnung notwendig 
    private Node[,] fieldCellArray; // Zweidimensionales Array zum Speichern der einzelnen Felder und derer Eigenschaften
    private bool startSelected, targetSelected; // Status ob Start/Ziel ausgewählt wurde
    public List<Node> path; // Speichert den Pfad zwischen Start und Ziel
    private GameObject field; // Objekt als das die Felder erstellt werden
    public GameObject astarpanel, bfspanel, dfspanel; // Legenden für A*-Algoritmuse und Breitensuche Algoritmus

    /*
     * Initialisierung mit Programmstart
     */
    void Start()
    {
        fieldCellDiameter = fieldCellSize * 2; 
        fieldSizeXAxis = Mathf.RoundToInt(fieldSize.x / fieldCellDiameter);
        fieldSizeYAxis = Mathf.RoundToInt(fieldSize.y / fieldCellDiameter);
        CreateFieldGrid();
    }

    /*
     * Erstellen des Spielfeldes
     */
    private void CreateFieldGrid()
    {
        fieldCellArray = new Node[fieldSizeXAxis, fieldSizeYAxis];
        Vector3 bottomLeft = transform.position - Vector3.right * fieldSize.x / 2 - Vector3.forward * fieldSize.y / 2; // Erstellung des Feldes um den Mittelpunkt anstelle des Mittlepunktes als Linken oberen Eckpunktes
        bool traversable = true;
        int counter = 0; // Zähler wie viele felder erstellt wurden 
        for (int x = 0; x < fieldSizeXAxis; x++){
            for (int y = 0; y < fieldSizeYAxis; y++)
            {
                Vector3 cordinate = bottomLeft + Vector3.right * (x * fieldCellDiameter + fieldCellSize) +
                                 Vector3.forward * (y * fieldCellDiameter + fieldCellSize); // Position an der neues Feld platziert wird
                field = Instantiate(fieldCell, cordinate, Quaternion.identity); // Erstellt ein neues FeldObjekt an der zuvor festgelegten Position mit der standart Rotation
                field.name = "field" + counter;
                field.AddComponent<Rigidbody>(); // Rigidbody wichtig um Felder anklicken zu können
                field.GetComponent<Rigidbody>().useGravity = false; 
                field.GetComponent<Rigidbody>().isKinematic = true; // wichtig um Kollision unter den Felder zu vermeiden
                fieldCellArray[x, y] = new Node(traversable, cordinate, x, y, field); // Fügt das aktuelle Feld dem array zu
                counter++;
            }
        }
    }

    /*
     * Interaktion durch den Benutzer um Start, Ziel und Hindernisse zu bestimmen
     */
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Linkemaustaste
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) // Von der aktuellen Mausposition wird ein Strahl gesendet wenn dieser ein Objekt trifft wird eine der Aktionen ausgeführt
            {
                if (!startSelected) 
                {
                    GameObject target = hit.rigidbody.gameObject; // Liefert das getroffene Objekt zurück
                    SetStart(target);
                    
                }else if (startSelected && !targetSelected)
                {
                    GameObject target = hit.rigidbody.gameObject;
                    SetTarget(target);
                }else if (startSelected && targetSelected)
                {
                    GameObject target = hit.rigidbody.gameObject;
                    SetBarricade(target);
                }
            }           
        }
    }

    /*
     * Bestimmt das dass ausgewählte Feld eine Hindernis ist
     */
    private void SetBarricade(GameObject field)
    {
        new ModifyNode().ChangeColor(field, Color.yellow);
        foreach (Node node in fieldCellArray)
        {
            if (field.transform.position == node.nodeGlobalPosition)
            {
                node.traversable = false; // Feld als nicht mehr begehbar markiert
            }
        }
    }

    /*
     * Bestimmt das dass ausgewählte Feld der Start ist
     */
    void SetStart(GameObject field)
    {
        new ModifyNode().ChangeColor(field, Color.green);
        foreach (Node node in fieldCellArray)
        {
            if (field.transform.position == node.nodeGlobalPosition)
            {
                node.start = true; // Feld wird als Startpunkt markiert
            }
        }
        startSelected = true;
    }

    /*
     * Bestimmt das dass ausgewählte Feld das Ziel ist
     */
    void SetTarget(GameObject field)
    {
        new ModifyNode().ChangeColor(field, Color.red);
        foreach (Node node in fieldCellArray)
        {
            if (field.transform.position == node.nodeGlobalPosition)
            {
                node.target = true; // Feld wird als Zielpunkt markiert
            }
        }
        targetSelected = true;
    }

    // Gibt das Felderarray an andere Klassen zurück
    public Node[,] GetArray()
    {
        return fieldCellArray;
    }
    
    /*
     * Liefert die Nachbarfelder eines Feldes als Liste zurück
     */
    public List<Node> GetNeighboringNodes(Node current)
    {
        List<Node> neighbors = new List<Node>();
        int checkXAxis; // prüfen ob Feld noch im gültigen X-Achsen Rahmen ist 
        int checkYAxis; // prüfen ob Feld noch im gültigen Y-Achsen Rahmen ist
        
        //oberer-Nachbar
        checkXAxis = current.cordX;
        checkYAxis = current.cordY + 1;
        if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis)
        {
            if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis)
            {
                neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
            }
        }
        
        //rechter-Nachbar
        checkXAxis = current.cordX + 1;
        checkYAxis = current.cordY;
        if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis)
        {
            if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis)
            {
                neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
            }
        }
        
        //unterer-Nachbar
        checkXAxis = current.cordX;
        checkYAxis = current.cordY - 1;
        if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis)
        {
            if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis)
            {
                neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
            }
        }
      
        //linker-Nachbar
        checkXAxis = current.cordX - 1;
        checkYAxis = current.cordY;
        if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis)
        {
            if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis)
            {
                neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
            }
        }
        
        return neighbors;
    }

    /*
     * Anzeige der richtigen Legende zum gewählten Algoritmus
     */
    public void ShowPanel(Text panel)
    {
        switch (panel.text)
        {
            case "A* - Algoritmus":
                print("A*");
                GameObject.Find("GameManager").GetComponent<AStarAlgorithmAlt>().enabled = true;
                GameObject.Find("GameManager").GetComponent<BreadthFirstSearch>().enabled = false;
                GameObject.Find("GameManager").GetComponent<DeaphtFirstSearch>().enabled = false;
                astarpanel.SetActive(true);
                bfspanel.SetActive(false);
                dfspanel.SetActive(false);
                break;
            case "Breitensuche - Algoritmus":
                GameObject.Find("GameManager").GetComponent<AStarAlgorithmAlt>().enabled = false;
                GameObject.Find("GameManager").GetComponent<BreadthFirstSearch>().enabled = true;
                GameObject.Find("GameManager").GetComponent<DeaphtFirstSearch>().enabled = false;
                astarpanel.SetActive(false);
                bfspanel.SetActive(true);
                dfspanel.SetActive(false);
                break;
            case "Tiefensuche - Algorithmus":
                GameObject.Find("GameManager").GetComponent<AStarAlgorithmAlt>().enabled = false;
                GameObject.Find("GameManager").GetComponent<BreadthFirstSearch>().enabled = false;
                GameObject.Find("GameManager").GetComponent<DeaphtFirstSearch>().enabled = true;
                astarpanel.SetActive(false);
                bfspanel.SetActive(false);
                dfspanel.SetActive(true);
                break;
            default:
                break;
        }        
    }
}
