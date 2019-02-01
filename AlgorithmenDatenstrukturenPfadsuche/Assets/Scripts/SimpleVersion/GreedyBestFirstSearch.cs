using System.Collections.Generic;
using UnityEngine;
using Event;

/**
 * Greedy Best First Search für Experimentiermodus
 * 
 * Tobias Stinner
 */

public class GreedyBestFirstSearch : MonoBehaviour {
    private CreateField grid;
    private Node startNode, targetNode;
    public List<Node> openListGreedy = new List<Node>();
    public HashSet<Node> closedListGreedy = new HashSet<Node>();
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
        openListGreedy.Clear();
        closedListGreedy.Clear();
        openListGreedy.Add(startNode);
        startNode.hCost = GetManhattenDistance(startNode, targetNode);
        Node currentNode;

        while (openListGreedy.Count > 0) {
            currentNode = openListGreedy[0];

            for (int i = 1; i < openListGreedy.Count; i++) {
                if (openListGreedy[i].fCost < currentNode.fCost || openListGreedy[i].fCost == currentNode.fCost && openListGreedy[i].hCost < currentNode.hCost) {
                    currentNode = openListGreedy[i];
                }
            }

            openListGreedy.Remove(currentNode);
            closedListGreedy.Add(currentNode);

            if (currentNode != startNode) {
                visualFeedback(new ColorizeAction(Color.magenta, currentNode.fieldCell));
            }

            if (currentNode != targetNode) {
                visualFeedback(new ColorizeAction(Color.magenta, currentNode.fieldCell));
            }

            if (currentNode == targetNode) {
                //print("Finish");
                GetPath(startNode, targetNode);
                statistics.setVisited(closedListGreedy.Count);
                break;
            }

            foreach (Node NeighborNode in grid.GetNeighboringNodes(currentNode)) {
                if (!NeighborNode.traversable || closedListGreedy.Contains(NeighborNode)) {
                    continue;
                }

                if (!openListGreedy.Contains(NeighborNode)) {
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, targetNode);
                    NeighborNode.parent = currentNode;
                    if (!openListGreedy.Contains(NeighborNode)) {
                        openListGreedy.Add(NeighborNode);
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
        //print("getPath");

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