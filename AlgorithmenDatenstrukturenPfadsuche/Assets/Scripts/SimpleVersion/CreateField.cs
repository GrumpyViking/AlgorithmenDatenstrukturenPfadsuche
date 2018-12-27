using System.Collections.Generic;
using UnityEngine;


public class CreateField : MonoBehaviour
{
    public GameObject fieldCell;

    public Vector2 fieldSize;
    private float fieldCellSize = 0.5f;
    private int fieldSizeXAxis, fieldSizeYAxis;
    private float fieldCellDiameter;
    private Node[,] fieldCellArray;
    private bool startSelected, targetSelected;
    public List<Node> path;
    private GameObject field;
    // Start is called before the first frame update
    void Start()
    {
        fieldCellDiameter = fieldCellSize * 2;
        fieldSizeXAxis = Mathf.RoundToInt(fieldSize.x / fieldCellDiameter);
        fieldSizeYAxis = Mathf.RoundToInt(fieldSize.y / fieldCellDiameter);
        CreateFieldGrid();
    }
    

    private void CreateFieldGrid()
    {
        fieldCellArray = new Node[fieldSizeXAxis, fieldSizeYAxis];
        Vector3 bottomLeft = transform.position - Vector3.right * fieldSize.x / 2 - Vector3.forward * fieldSize.y / 2;
        bool wall = true;
        int counter = 0;
        for (int x = 0; x < fieldSizeXAxis; x++){
            for (int y = 0; y < fieldSizeYAxis; y++)
            {
                Vector3 worldCoordinate = bottomLeft + Vector3.right * (x * fieldCellDiameter + fieldCellSize) +
                                 Vector3.forward * (y * fieldCellDiameter + fieldCellSize);
                field = Instantiate(fieldCell, worldCoordinate, Quaternion.identity);
                field.name = "field" + counter;
                field.AddComponent<Rigidbody>();
                field.GetComponent<Rigidbody>().useGravity = false;
                field.GetComponent<Rigidbody>().isKinematic = true;
                fieldCellArray[x, y] = new Node(wall, worldCoordinate, x, y, field);
                counter++;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                if (!startSelected)
                {
                    GameObject target = hit.rigidbody.gameObject;
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

    private void SetBarricade(GameObject field)
    {
        new ModifyNode().ChangeColor(field, Color.yellow);
        foreach (Node node in fieldCellArray)
        {
            if (field.transform.position == node.nodeGlobalPosition)
            {
                node.traversable = false;
            }
        }
    }

    void SetStart(GameObject field)
    {
        new ModifyNode().ChangeColor(field, Color.green);
        foreach (Node node in fieldCellArray)
        {
            if (field.transform.position == node.nodeGlobalPosition)
            {
                node.start = true;
            }
        }
        startSelected = true;
    }

    void SetTarget(GameObject field)
    {
        new ModifyNode().ChangeColor(field, Color.red);
        foreach (Node node in fieldCellArray)
        {
            if (field.transform.position == node.nodeGlobalPosition)
            {
                node.target = true;
            }
        }
        targetSelected = true;
    }

    public Node[,] GetArray()
    {
        return fieldCellArray;
    }
    
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
}
