using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : MonoBehaviour
{
    private Grid grid;
    private Node startNode, targetNode;
    public Transform startPosition; 

    public Transform targetPosition; 
    public List<Node> openList = new List<Node>(); 
    public HashSet<Node> closedList = new HashSet<Node>();

    void Update()
    {
        FindPath();
    }

    private void FindPath()
    {
        grid = GetComponent<Grid>();
        startNode = grid.NodeFromWorldPoint(startPosition.position);
        targetNode = grid.NodeFromWorldPoint(targetPosition.position);
        openList.Clear();
        closedList.Clear();
        openList.Add(startNode); 

        while (openList.Count > 0) 
        {
            Node current = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < current.fCost ||
                    openList[i].fCost == current.fCost && openList[i].hCost < current.hCost)
                {
                    current = openList[i];
                }
            }

            openList.Remove(current); 
            closedList.Add(current);

            if (current == targetNode)
            {
                GetPath(startNode, targetNode);
                break;
            }

            foreach (var neighbors in grid.GetNeighboringNodes(current)){
                if (!neighbors.traversable || closedList.Contains(neighbors))
                {
                    continue;
                } 
                
                var moveCost = current.gCost + GetManhattenDistance(current, neighbors); 

                if (moveCost < neighbors.gCost || !openList.Contains(neighbors)
                ) 
                {
                    neighbors.gCost = moveCost; 
                    neighbors.hCost = GetManhattenDistance(neighbors, targetNode); 
                    neighbors.parent = current; 

                    if (!openList.Contains(neighbors)) 
                        openList.Add(neighbors); 
                }
            }
        }
        
    }


    private void GetPath(Node startingNode, Node endNode)
    {
        List<Node> finalPath = new List<Node>();
        Node currentNode = endNode; 

        while (currentNode != startingNode) 
        {
            finalPath.Add(currentNode); 
            currentNode = currentNode.parent;
        }

        finalPath.Reverse();
        grid.path = finalPath;
    }

    private int GetManhattenDistance(Node nodeA, Node nodeB)
    {
        int disX = Mathf.Abs(nodeA.cordX - nodeB.cordX); 
        int disY = Mathf.Abs(nodeA.cordY - nodeB.cordY); 

        return disX + disY; 
    }
}