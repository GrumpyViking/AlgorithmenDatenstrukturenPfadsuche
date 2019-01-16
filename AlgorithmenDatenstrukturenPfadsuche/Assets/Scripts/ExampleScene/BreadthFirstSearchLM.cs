using System.Collections.Generic;
using Event;
using UnityEngine;

/**
    Breitensuche (englisch breadth-first search, BFS) ist ein Verfahren in der Informatik zum Durchsuchen bzw. Durchlaufen der Knoten eines Graphen. 
    Sie zählt zu den uninformierten Suchalgorithmen. Im Gegensatz zur Tiefensuche werden zunächst alle Knoten beschritten, die vom Ausgangsknoten direkt erreichbar sind. 
    Erst danach werden Folgeknoten beschritten.
    Quelle: https://de.wikipedia.org/wiki/Breitensuche
 */

public class BreadthFirstSearchLM : MonoBehaviour {
    private ExampleManager grid;
    Queue<Node> open = new Queue<Node>();
    private Node startNode;
    private GameObject startPosition;
    private Node targetNode;
    private GameObject targetPosition;
    private Statistics2 statistics;

    void Awake() {
        grid = GetComponent<ExampleManager>();
        statistics = GetComponent<Statistics2>();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Execute();
        }
    }

    private void visualFeedback(IAction action) {
        GetComponent<AnimationQueue>().enqueueAction(action);
    }

    private void Execute() {
        foreach (Node node in grid.GetArray()) {
            if (node.start == true) {
                startPosition = node.fieldCell;
                startNode = node;
            }

            if (node.target == true) {
                targetPosition = node.fieldCell;
                targetNode = node;
            }
        }
        BFS();
    }

    private void BFS() {
        open.Clear();
        open.Enqueue(startNode);
        startNode.visited = true;
        startNode.parent = null;
        Node current = null;
        int visited = 0;

        while (open.Count > 0) {
            current = open.Dequeue();
            visualFeedback(new ColorizeAction(Color.cyan, current.fieldCell));
            visited++;

            if (current == targetNode) {
                GeneratePath(current, startNode);
                print("Breitensuche besuchte: " + visited);
                statistics.setVisited(visited);
                break;
            }

            foreach (Node neighbor in grid.GetNeighboringNodes(current)) {
                if (!neighbor.traversable || neighbor.visited) {
                    continue;
                } else {
                    open.Enqueue(neighbor);
                    neighbor.visited = true;
                    neighbor.parent = current;
                    visualFeedback(new ColorizeAction(Color.magenta, neighbor.fieldCell));
                }
            }
        }
    }

    private void GeneratePath(Node backTrack, Node start) {
        visualFeedback(new ColorizeAction(Color.red, backTrack.fieldCell));
        List<Node> finalPath = new List<Node>();
        int pathCount = 0;
        while (backTrack != start) {
            finalPath.Add(backTrack);
            backTrack = backTrack.parent;
            if (backTrack == start) {
                visualFeedback(new ColorizeAction(Color.green, start.fieldCell));
            } else {
                visualFeedback(new ColorizeAction(Color.blue, backTrack.fieldCell));
                pathCount++;
            }
        }

        pathCount++;
        print("Breitensuche Pfadlänge: " + pathCount);
        statistics.setPathLength(pathCount);
        finalPath.Reverse();
        grid.path = finalPath;
    }
}
