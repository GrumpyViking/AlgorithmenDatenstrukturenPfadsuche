using System.Collections.Generic;
using Event;
using UnityEngine;

public class BreadthFirstSearch : MonoBehaviour
{
    private CreateField grid;
    bool[] visited;
    List<Node> pathFrom = new List<Node>();
    Queue<Node> open = new Queue<Node>();
    private Node startNode;
    private GameObject startPosition;
    private Node targetNode;
    private GameObject targetPosition;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Test");
            Execute();
        }
    }
    
    private void visualFeedback(IAction action)
    {
        GetComponent<AnimationQueue>().enqueueAction(action);
    }
    
    private void Execute()
    {
        grid = GetComponent<CreateField>();
        foreach (Node node in grid.GetArray())
        {
            if (node.start == true)
            {
                startPosition = node.fieldCell;
                startNode = node;
            }

            if (node.target == true)
            {
                targetPosition = node.fieldCell;
                targetNode = node;
            }
        }

        BFS(startNode, targetNode);
    }

    private void BFS(Node start, Node target)
    {
        open.Enqueue(start);
        start.visited = true;
        start.parent = null;
        Node current = null;
        
        while (open.Count > 0)
        {
            current = open.Dequeue();
            visualFeedback(new ColorizeAction(Color.cyan, current.fieldCell));
            if (current == target)
            {
                GeneratePath(current, start);
                break;
            }
            
            foreach (Node neighbor in grid.GetNeighboringNodes(current)){
                if (!neighbor.traversable || neighbor.visited)
                {
                    continue; 
                }else{
                    open.Enqueue(neighbor);
                    neighbor.visited = true;
                    neighbor.parent = current;
                    visualFeedback(new ColorizeAction(Color.magenta, neighbor.fieldCell));
                } 
            }
        }
    }

    private void GeneratePath(Node backTrack, Node start)
    {
        visualFeedback(new ColorizeAction(Color.red, backTrack.fieldCell));
        List<Node> finalPath = new List<Node>();
        Node tmp = backTrack;
        while (backTrack != start)
        {
            finalPath.Add(tmp); 
            tmp = tmp.parent;
            if (tmp == start)
            {
                visualFeedback(new ColorizeAction(Color.green, start.fieldCell));
            }
            else
            {
                visualFeedback(new ColorizeAction(Color.blue, tmp.fieldCell));
            }
        }
        finalPath.Reverse();
        grid.path = finalPath;
    }
}
