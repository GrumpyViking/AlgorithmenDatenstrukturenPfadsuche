using System.Collections.Generic;
using UnityEngine;
using Event;

public class GreedyBestFirstSearch : MonoBehaviour {
    private CreateField grid;
    private Node startNode, targetNode;
    public List<Node> openList = new List<Node>();
    public HashSet<Node> closedList = new HashSet<Node>();
    private Statistics2 statistics;

    void Awake() {
        statistics = GetComponent<Statistics2>();
        grid = GetComponent<CreateField>();
    }

    private void visualFeedback(IAction action) {
        GetComponent<AnimationQueue>().enqueueAction(action);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !grid.paused) {
            Execute();
            visualFeedback(new ColorizeAction(Color.green, startNode.fieldCell));
            visualFeedback(new ColorizeAction(Color.red, targetNode.fieldCell));
        }
    }

    public void Execute() {
        foreach (Node node in grid.GetArray()) {
            if (node.start == true) {
                startNode = node;
            }

            if (node.target == true) {
                targetNode = node;
            }
        }
        GBFS();
    }

    private void GBFS() {
        openList.Clear();
        closedList.Clear();
        openList.Add(startNode);
        startNode.hCost = GetManhattenDistance(startNode, targetNode);

        while (openList.Count > 0) {
            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++) {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost) {
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
                print("GreedyBestFS Besuchte: " + closedList.Count);
                statistics.setVisited(closedList.Count);
                break;
            }

            foreach (Node NeighborNode in grid.GetNeighboringNodes(currentNode)) {
                if (!NeighborNode.traversable || closedList.Contains(NeighborNode)) {
                    continue;
                }

                if (!closedList.Contains(NeighborNode)) {
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, targetNode);

                    NeighborNode.parent = currentNode;
                    if (!openList.Contains(NeighborNode)) {
                        openList.Add(NeighborNode);
                        visualFeedback(new ColorizeAction(Color.cyan, NeighborNode.fieldCell));
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
        print("GreedyBestFS Pfadlänge: " + count);
    }

    private int GetManhattenDistance(Node nodeA, Node nodeB) {
        int disX = Mathf.Abs(nodeA.cordX - nodeB.cordX);
        int disY = Mathf.Abs(nodeA.cordY - nodeB.cordY);

        return disX + disY;
    }
}