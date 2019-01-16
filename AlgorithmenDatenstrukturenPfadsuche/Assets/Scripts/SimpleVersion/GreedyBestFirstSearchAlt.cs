using System.Collections.Generic;
using UnityEngine;
using System;
using Event;

public class GreedyBestFirstSearchAlt : MonoBehaviour {
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
        openList.Add(startNode);
        List<Node> closedList = new List<Node>();

        while (openList.Count > 0) {
            Node best = GetNextNode(openList);
            closedList.Add(best);
            if (best == targetNode) {
                GetPath(startNode, targetNode);
                break;
            }

            foreach (Node next in grid.GetNeighboringNodes(best)) {
                if (!closedList.Contains(next)) {
                    next.hCost = GetManhattenDistance(targetNode, next);
                    openList.Add(next);
                    next.parent = best;
                } else {
                    if (next.fCost > best.fCost) {
                        next.parent = best;
                    }
                }
            }
        }
    }

    Node GetNextNode(List<Node> nodes) {
        Node bestnextnode = new Node();
        int cost = Int32.MaxValue;
        foreach (Node node in nodes) {
            if (node.fCost < cost) {
                bestnextnode = node;
            }
        }
        return bestnextnode;
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