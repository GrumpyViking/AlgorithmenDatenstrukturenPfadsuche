using System.Collections.Generic;
using UnityEngine;
using Event;

public class AStarAlgorithmLM : MonoBehaviour {
    private ExampleManager grid;
    private Node startNode, targetNode;
    public List<Node> openList = new List<Node>();
    public HashSet<Node> closedList = new HashSet<Node>();
    private Statistics2 statistics;

    void Awake() {
        statistics = GetComponent<Statistics2>();
        grid = GetComponent<ExampleManager>();
    }
    private void visualFeedback(IAction action) {
        GetComponent<AnimationQueue>().enqueueAction(action);
    }
    /*
        void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Execute();
        }
    }
     */
    public void Execute() {


        foreach (Node node in grid.GetArray()) {
            if (node.start == true) {
                startNode = node;
            }

            if (node.target == true) {
                targetNode = node;
            }
        }
        AStarAlgo();
    }

    private void AStarAlgo() {
        openList.Clear();
        closedList.Clear();
        openList.Add(startNode);
        startNode.gCost = 0;
        startNode.hCost = GetManhattenDistance(startNode, targetNode);
        Node currentNode;
        while (openList.Count > 0) {
            currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++) {
                if (openList[i].fCost < currentNode.fCost ||
                    openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost) {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);
            if (currentNode != startNode) {
                visualFeedback(new ColorizeAction(Color.magenta, currentNode.fieldCell));
            }

            if (currentNode != targetNode) {
                visualFeedback(new ColorizeAction(Color.magenta, currentNode.fieldCell));
            }

            if (currentNode == targetNode) {
                GetPath(startNode, targetNode);
                statistics.setVisited(closedList.Count);
                break;
            }

            foreach (Node next in grid.GetNeighboringNodes(currentNode)) {
                if (!next.traversable || closedList.Contains(next)) {
                    continue;
                }

                var moveCost = currentNode.gCost + GetManhattenDistance(currentNode, next);

                if (moveCost < next.gCost || !openList.Contains(next)
                ) {
                    next.gCost = moveCost;
                    next.hCost = GetManhattenDistance(next, targetNode);
                    next.parent = currentNode;

                    if (!openList.Contains(next)) {
                        openList.Add(next);
                        visualFeedback(new ColorizeAction(Color.cyan, next.fieldCell));
                    }


                }
            }
        }
    }


    private void GetPath(Node startingNode, Node endNode) {
        List<Node> finalPath = new List<Node>();
        Node currentNode = endNode;
        int count = 0;

        while (currentNode != startingNode) {
            count++;
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
            visualFeedback(new ColorizeAction(Color.blue, currentNode.fieldCell));
        }
        statistics.setPathLength(count);
        finalPath.Reverse();
        grid.path = finalPath;
        visualFeedback(new ColorizeAction(Color.green, startNode.fieldCell));
        visualFeedback(new ColorizeAction(Color.red, targetNode.fieldCell));
    }

    private int GetManhattenDistance(Node nodeA, Node nodeB) {
        int disX = Mathf.Abs(nodeA.cordX - nodeB.cordX);
        int disY = Mathf.Abs(nodeA.cordY - nodeB.cordY);

        return disX + disY;
    }
}