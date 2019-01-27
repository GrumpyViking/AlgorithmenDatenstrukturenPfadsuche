using System.Collections.Generic;
using UnityEngine;
using Event;

public class DijsktraSearchNew : MonoBehaviour {
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
        DijsktraAlgo();
    }

    private void DijsktraAlgo() {

        openList.Clear();
        closedList.Clear();
        openList.Add(startNode);
        startNode.gCost = 0;
        Node currentNode;

        while (openList.Count > 0) {

            currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++) {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost) {
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

            foreach (Node NeighborNode in grid.GetNeighboringNodes(currentNode)) {

                if (!NeighborNode.traversable || closedList.Contains(NeighborNode)) {
                    continue;
                }
                var MoveCost = currentNode.gCost;

                if (!openList.Contains(NeighborNode)) {

                    NeighborNode.gCost = MoveCost;
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
        visualFeedback(new ColorizeAction(Color.green, startNode.fieldCell));
        visualFeedback(new ColorizeAction(Color.red, targetNode.fieldCell));
    }
}