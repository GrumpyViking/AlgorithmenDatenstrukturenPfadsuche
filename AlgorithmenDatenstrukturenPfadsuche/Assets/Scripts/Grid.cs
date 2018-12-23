using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private float cellDiameter;

    public float cellSize, distanceOfCells;

    public Vector2 gridSize;

    private int gridSizeX, gridSizeY;

    private Node[,] nodeArray;

    public LayerMask Obstacle;

    public List<Node> path;
    public Transform startPos;

    private void Start()
    {
        cellDiameter = cellSize * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / cellDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / cellDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        nodeArray = new Node[gridSizeX, gridSizeY];
        var bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        for (var x = 0; x < gridSizeX; x++)
        for (var y = 0; y < gridSizeY; y++)
        {
            var worldPoint = bottomLeft + Vector3.right * (x * cellDiameter + cellSize) +
                             Vector3.forward * (y * cellDiameter + cellSize);
            var wall = !Physics.CheckSphere(worldPoint, cellSize, Obstacle);

            nodeArray[x, y] = new Node(wall, worldPoint, x, y);
        }
    }

    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        var NeighborList = new List<Node>();
        int icheckX;
        int icheckY;

        icheckX = a_NeighborNode.cordX + 1;
        icheckY = a_NeighborNode.cordY;
        if (icheckX >= 0 && icheckX < gridSizeX)
            if (icheckY >= 0 && icheckY < gridSizeY)
                NeighborList.Add(nodeArray[icheckX, icheckY]);


        icheckX = a_NeighborNode.cordX - 1;
        icheckY = a_NeighborNode.cordY;
        if (icheckX >= 0 && icheckX < gridSizeX)
            if (icheckY >= 0 && icheckY < gridSizeY)
                NeighborList.Add(nodeArray[icheckX, icheckY]);


        icheckX = a_NeighborNode.cordX;
        icheckY = a_NeighborNode.cordY + 1;
        if (icheckX >= 0 && icheckX < gridSizeX)
            if (icheckY >= 0 && icheckY < gridSizeY)
                NeighborList.Add(nodeArray[icheckX, icheckY]);


        icheckX = a_NeighborNode.cordX;
        icheckY = a_NeighborNode.cordY - 1;
        if (icheckX >= 0 && icheckX < gridSizeX)
            if (icheckY >= 0 && icheckY < gridSizeY)
                NeighborList.Add(nodeArray[icheckX, icheckY]);

        return NeighborList;
    }


    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        var ixPos = (a_vWorldPos.x + gridSize.x / 2) / gridSize.x;
        var iyPos = (a_vWorldPos.z + gridSize.y / 2) / gridSize.y;

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        var ix = Mathf.RoundToInt((gridSizeX - 1) * ixPos);
        var iy = Mathf.RoundToInt((gridSizeY - 1) * iyPos);

        return nodeArray[ix, iy];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,
            new Vector3(gridSize.x, 0.25f,
                gridSize.y));

        if (nodeArray != null)
            foreach (var n in nodeArray)
            {
                if (n.traversable)
                    Gizmos.color = Color.white;
                else
                    Gizmos.color = Color.yellow;


                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.cyan;
                

                Gizmos.DrawCube(n.nodeGlobalPosition,
                    Vector3.one * (cellDiameter - distanceOfCells));
            }
    }
}