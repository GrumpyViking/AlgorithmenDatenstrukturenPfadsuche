using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    float cellDiameter;
    public float cellSize, distanceOfCells;
    public Vector2 gridSize;
    private int fieldSizeXAxis, fieldSizeYAxis;
    private Node[,] fieldCellArray;

    public LayerMask Obstacle;
    public List<Node> path;
    public Transform startPos;

    private void Start()
    {
        cellDiameter = cellSize * 2;
        fieldSizeXAxis = Mathf.RoundToInt(gridSize.x / cellDiameter);
        fieldSizeYAxis = Mathf.RoundToInt(gridSize.y / cellDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        fieldCellArray = new Node[fieldSizeXAxis, fieldSizeYAxis];
        var bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        for (var x = 0; x < fieldSizeXAxis; x++)
        for (var y = 0; y < fieldSizeYAxis; y++)
        {
            var worldPoint = bottomLeft + Vector3.right * (x * cellDiameter + cellSize) +
                             Vector3.forward * (y * cellDiameter + cellSize);
            var wall = !Physics.CheckSphere(worldPoint, cellSize, Obstacle);

            fieldCellArray[x, y] = new Node(wall, worldPoint, x, y, null);
        }
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
    public Node[,] GetArray()
    {
        return fieldCellArray;
    }
    
    // Ziel und Start können freibewegt werden wandelt die Globale Position in eine Position im Array der Felder zu
    public Node NodeFromWorldPoint(Vector3 globalPos)
    {
        float posXAxis = (globalPos.x + gridSize.x / 2) / gridSize.x;
        float posYAxis = (globalPos.z + gridSize.y / 2) / gridSize.y;

        posXAxis = Mathf.Clamp01(posXAxis);
        posYAxis = Mathf.Clamp01(posYAxis);

        int cordX = Mathf.RoundToInt((fieldSizeXAxis - 1) * posXAxis);
        int cordY = Mathf.RoundToInt((fieldSizeYAxis - 1) * posYAxis);

        return fieldCellArray[cordX, cordY];
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,new Vector3(gridSize.x, 0.25f,gridSize.y));
        if (fieldCellArray != null)
            foreach (var node in fieldCellArray)
            {
                if (node.traversable)
                    Gizmos.color = Color.white;
                else
                    Gizmos.color = Color.yellow;
                
                if (path != null)
                    if (path.Contains(node))
                        Gizmos.color = Color.green;
                
                Gizmos.DrawCube(node.nodeGlobalPosition,Vector3.one * (cellDiameter - distanceOfCells));
            }
        
    }
}