using System.Collections;
using System.Collections.Generic;
using Event;
using UnityEngine;

public class DepthFirstSearch : MonoBehaviour {
    private CreateField grid;
    // Start is called before the first frame update
    // Queue<Node> open = new Queue<Node>();
    Stack<Node> open = new Stack<Node>();
    private Node startNode;
    private GameObject startPosition;
    private Node targetNode;
    private GameObject targetPosition;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Execute();
        }
    }

    void Awake() {
        grid = GetComponent<CreateField>();
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

        DFS(startNode, targetNode);
    }

    private void DFS(Node start, Node target) {
        open.Push(start);
        start.visited = true;
        start.parent = null;
        Node current = null;

        while (open.Count > 0) {
            current = open.Pop();
            visualFeedback(new ColorizeAction(Color.cyan, current.fieldCell));

            if (current == target) {
                GeneratePath(current, start);
                break;
            }

            foreach (Node neighbor in grid.GetNeighboringNodes(current)) {
                if (!neighbor.traversable || neighbor.visited) {
                    continue;
                } else {
                    open.Push(neighbor);
                    neighbor.visited = true;
                    neighbor.parent = current;
                    visualFeedback(new ColorizeAction(Color.magenta, neighbor.fieldCell)); ;
                }
            }
        }
    }

    private void GeneratePath(Node backTrack, Node start) {
        visualFeedback(new ColorizeAction(Color.red, backTrack.fieldCell));
        List<Node> finalPath = new List<Node>();
        Node tmp = backTrack;

        while (backTrack != start) {
            finalPath.Add(tmp);
            tmp = tmp.parent;

            if (tmp == start) {
                visualFeedback(new ColorizeAction(Color.green, start.fieldCell));
            } else {
                visualFeedback(new ColorizeAction(Color.blue, tmp.fieldCell));
            }
        }

        finalPath.Reverse();
        grid.path = finalPath;
    }
}
