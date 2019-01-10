using System.Collections.Generic;
using UnityEngine;
using Event;

public class AStarAlgorithmAlt : MonoBehaviour
{
    private CreateField grid; 
    private Node startNode, targetNode;
    public List<Node> openList = new List<Node>(); 
    public HashSet<Node> closedList = new HashSet<Node>();
    
    private void visualFeedback(IAction action)
    {
        GetComponent<AnimationQueue>().enqueueAction(action);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Execute();
            visualFeedback(new ColorizeAction(Color.green, startNode.fieldCell));
            visualFeedback(new ColorizeAction(Color.red, targetNode.fieldCell));
        }
    }    
    public void Execute()
    {
        grid = GetComponent<CreateField>();
        
        foreach (Node node in grid.GetArray())
        {
            if (node.start == true)
            {
                startNode = node;
            }

            if (node.target == true)
            {
                targetNode = node;
            }
        }
        AStarAlgo();
    }

    private void AStarAlgo()
    {
        openList.Add(startNode);
        startNode.gCost = 0;
        startNode.hCost = GetManhattenDistance(startNode, targetNode);
        
        while (openList.Count > 0) 
        {
            Node currentNode = openList[0];
            
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i]; 
                } 
            } 
                    
            openList.Remove(currentNode); 
            closedList.Add(currentNode);
            if (currentNode != startNode)
            {
                visualFeedback(new ColorizeAction(Color.magenta, currentNode.fieldCell));
            }

            if (currentNode != targetNode)
            {
                visualFeedback(new ColorizeAction(Color.magenta, currentNode.fieldCell));
            }
            
            if (currentNode == targetNode)
            {
                GetPath(startNode, targetNode);
                break;
            } 
            
            foreach (Node NeighborNode in grid.GetNeighboringNodes(currentNode)){
                if (!NeighborNode.traversable || closedList.Contains(NeighborNode))
                {
                    continue; 
                }                  
                var MoveCost = currentNode.gCost + GetManhattenDistance(currentNode, NeighborNode); 

                if (MoveCost < NeighborNode.gCost || !openList.Contains(NeighborNode)){
                    NeighborNode.gCost = MoveCost; 
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, targetNode); 
                    NeighborNode.parent = currentNode;
                    if (!openList.Contains(NeighborNode))
                    {
                        openList.Add(NeighborNode);
                        visualFeedback(new ColorizeAction(Color.cyan, NeighborNode.fieldCell));
                    }
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
            visualFeedback(new ColorizeAction(Color.blue, currentNode.fieldCell));
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