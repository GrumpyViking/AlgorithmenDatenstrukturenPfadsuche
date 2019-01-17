using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ExampleManager : MonoBehaviour {

    public GameObject fieldCell; // Objekt aus dem das Feldbesteht (in Unity hinzufügen)
    public Vector2 fieldSize; // Größe des Spielfeldes (in Unity eintragen)
    private float fieldCellSize = 0.5f; // größe der einzelnen Felder
    private int fieldSizeXAxis, fieldSizeYAxis; // anzahl Felder auf der X bzw. Y Achse
    private float fieldCellDiameter; // Für Korrekte Feldgrößen berechnung notwendig 
    private List<LevelData> modifyedNodes = new List<LevelData>();
    public Node[,] fieldCellArray; // Zweidimensionales Array zum Speichern der einzelnen Felder und derer Eigenschaften
    public List<Node> path = new List<Node>(); // Speichert den Pfad zwischen Start und Ziel
    private GameObject field;
    private bool paused;
    private int examples;
    public GameObject astarpanel, bfspanel, dfspanel, questions;
    private bool bfsActiv, dfsActiv, aStarActiv;

    void Start() {
        switch (PlayerSceneData.lastScene) {
            case 5:
                Debug.Log("Tiefensuche");
                GameObject.Find("ExampleManager").GetComponent<AStarAlgorithmLM>().enabled = false;
                GameObject.Find("ExampleManager").GetComponent<BreadthFirstSearchLM>().enabled = false;
                GameObject.Find("ExampleManager").GetComponent<DepthFirstSearchLM>().enabled = true;
                astarpanel.SetActive(false);
                bfspanel.SetActive(false);
                dfspanel.SetActive(true);
                dfsActiv = true;
                examples = 1;
                LoadLevel("DFSLevel" + examples, "DeapthFirstSearch");
                ChangeQuestions();

                break;
            case 6:
                Debug.Log("Breitensuche");
                GameObject.Find("ExampleManager").GetComponent<AStarAlgorithmLM>().enabled = false;
                GameObject.Find("ExampleManager").GetComponent<BreadthFirstSearchLM>().enabled = true;
                GameObject.Find("ExampleManager").GetComponent<DepthFirstSearchLM>().enabled = false;
                astarpanel.SetActive(false);
                bfspanel.SetActive(true);
                dfspanel.SetActive(false);
                bfsActiv = true;
                examples = 1;
                LoadLevel("BFSLevel" + examples, "BreathFirstSearch");
                ChangeQuestions();

                break;
            case 7:
                Debug.Log("A*");
                GameObject.Find("ExampleManager").GetComponent<AStarAlgorithmLM>().enabled = true;
                GameObject.Find("ExampleManager").GetComponent<BreadthFirstSearchLM>().enabled = false;
                GameObject.Find("ExampleManager").GetComponent<DepthFirstSearchLM>().enabled = false;
                astarpanel.SetActive(true);
                bfspanel.SetActive(false);
                dfspanel.SetActive(false);
                aStarActiv = true;
                examples = 1;
                LoadLevel("AStarLevel" + examples, "AStar");
                ChangeQuestions();
                break;
            default:
                Debug.Log("Fehler im ExampleMode!");
                break;
        }
    }

    int GetLevelCount(string folder) {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/" + "LearnModeLevels/" + folder + "/");
        FileInfo[] level = dir.GetFiles("*.grid");
        return level.Length;
    }
    void Awake() {
        aStarActiv = false;
        bfsActiv = false;
        dfsActiv = false;
        fieldCellDiameter = fieldCellSize * 2;
        fieldSizeXAxis = Mathf.RoundToInt(fieldSize.x / fieldCellDiameter);
        fieldSizeYAxis = Mathf.RoundToInt(fieldSize.y / fieldCellDiameter);
        CreateFieldGrid();
    }

    private void CreateFieldGrid() {
        Debug.Log("Test");
        fieldCellArray = new Node[fieldSizeXAxis, fieldSizeYAxis];
        Vector3 bottomLeft = transform.position - Vector3.right * fieldSize.x / 2 - Vector3.forward * fieldSize.y / 2; // Erstellung des Feldes um den Mittelpunkt anstelle des Mittlepunktes als Linken oberen Eckpunktes
        bool traversable = true;
        int counter = 0; // Zähler wie viele felder erstellt wurden 
        for (int x = 0; x < fieldSizeXAxis; x++) {
            for (int y = 0; y < fieldSizeYAxis; y++) {
                Vector3 cordinate = bottomLeft + Vector3.right * (x * fieldCellDiameter + fieldCellSize) +
                                 Vector3.forward * (y * fieldCellDiameter + fieldCellSize); // Position an der neues Feld platziert wird
                field = Instantiate(fieldCell, cordinate, Quaternion.identity); // Erstellt ein neues FeldObjekt an der zuvor festgelegten Position mit der standart Rotation
                field.name = "field" + counter;
                field.AddComponent<Rigidbody>(); // Rigidbody wichtig um Felder anklicken zu können
                field.GetComponent<Rigidbody>().useGravity = false;
                field.GetComponent<Rigidbody>().isKinematic = true; // wichtig um Kollision unter den Felder zu vermeiden
                fieldCellArray[x, y] = new Node(traversable, cordinate, x, y, field); // Fügt das aktuelle Feld dem array zu
                fieldCellArray[x, y].index = counter;
                counter++;
            }
        }
    }

    public void ChangeNextLevel() {
        if (aStarActiv && examples < GetLevelCount("AStar")) {
            examples++;
            LoadLevel("AStarLevel" + examples, "AStar");
            ChangeQuestions();
        }
        if (bfsActiv && examples < GetLevelCount("BreathFirstSearch")) {
            examples++;
            LoadLevel("BFSLevel" + examples, "BreathFirstSearch");
            ChangeQuestions();
        }
        if (dfsActiv && examples < GetLevelCount("DeapthFirstSearch")) {
            examples++;
            LoadLevel("DFSLevel" + examples, "DeapthFirstSearch");
            ChangeQuestions();
        }

    }

    public void ChangePrevLevel() {
        if (aStarActiv && examples > 1) {
            examples--;
            LoadLevel("AStarLevel" + examples, "AStar");
            ChangeQuestions();
        }
        if (bfsActiv && examples > 1) {
            examples--;
            LoadLevel("BFSLevel" + examples, "BreathFirstSearch");
            ChangeQuestions();
        }
        if (dfsActiv && examples > 1) {
            examples--;
            LoadLevel("DFSLevel" + examples, "DeapthFirstSearch");
            ChangeQuestions();
        }
    }

    private void ChangeQuestions() {
        if (bfsActiv) {
            LoadNewQuestions(examples, "BFS", "BreathFirstSearch");
        }
        if (dfsActiv) {
            LoadNewQuestions(examples, "DFS", "DeapthFirstSearch");
        }

        if (aStarActiv) {
            LoadNewQuestions(examples, "AStar", "AStar");
        }

    }

    private void LoadNewQuestions(int number, string filename, string folder) {
        string path = "Assets/LearnModeLevels/" + folder + "/" + filename + "" + number + ".txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path, System.Text.Encoding.UTF8, true);
        questions.GetComponent<Text>().text = reader.ReadToEnd();
        reader.Close();
    }
    public void LoadLevel(string filename, string folder) {
        ClearGrid();
        SavableData savedLevel = SaveSystem.LoadLevelExamples((filename + ".grid"), folder);

        foreach (Node node in fieldCellArray) {
            foreach (LevelData ld in savedLevel.saveNodes) {
                if (node.index == ld.index) {
                    if (ld.start) {
                        node.start = true;
                        new ModifyNode().ChangeColor(node.fieldCell, Color.green);
                    }
                    if (ld.target) {
                        node.target = true;
                        new ModifyNode().ChangeColor(node.fieldCell, Color.red);
                    }
                    if (!ld.traversable) {
                        node.traversable = false;
                        new ModifyNode().ChangeColor(node.fieldCell, Color.yellow);
                    }
                    LevelData mnode = new LevelData(node);
                    mnode.index = node.index;
                    modifyedNodes.Add(mnode);
                }
            }
        }
    }

    public Node[,] GetArray() {
        return fieldCellArray;
    }

    public void ClearGrid() {
        path.Clear();
        modifyedNodes.Clear();
        foreach (Node node in fieldCellArray) {
            node.start = false;
            node.target = false;
            node.traversable = true;
            node.parent = null;
            node.gCost = Int32.MaxValue;
            node.hCost = Int32.MaxValue;
            node.visited = false;
            new ModifyNode().ChangeColor(node.fieldCell, Color.white);
        }
    }

    public List<Node> GetNeighboringNodes(Node current) {
        List<Node> neighbors = new List<Node>();
        int checkXAxis; // prüfen ob Feld noch im gültigen X-Achsen Rahmen ist 
        int checkYAxis; // prüfen ob Feld noch im gültigen Y-Achsen Rahmen ist

        //oberer-Nachbar
        checkXAxis = current.cordX;
        checkYAxis = current.cordY + 1;
        if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis) {
            if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis) {
                neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
            }
        }

        //rechter-Nachbar
        checkXAxis = current.cordX + 1;
        checkYAxis = current.cordY;
        if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis) {
            if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis) {
                neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
            }
        }

        //unterer-Nachbar
        checkXAxis = current.cordX;
        checkYAxis = current.cordY - 1;
        if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis) {
            if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis) {
                neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
            }
        }

        //linker-Nachbar
        checkXAxis = current.cordX - 1;
        checkYAxis = current.cordY;
        if (checkXAxis >= 0 && checkXAxis < fieldSizeXAxis) {
            if (checkYAxis >= 0 && checkYAxis < fieldSizeYAxis) {
                neighbors.Add(fieldCellArray[checkXAxis, checkYAxis]);
            }
        }

        return neighbors;
    }

    public void ResetLevel() {
        path.Clear();
        bool isSet = false;
        foreach (Node node in fieldCellArray) {
            isSet = false;
            foreach (LevelData ld in modifyedNodes) {
                if (node.index == ld.index) {
                    if (ld.start) {
                        node.visited = false;
                    }
                    if (ld.target) {
                        node.visited = false;
                    }
                    isSet = true;
                }
            }
            if (!isSet) {
                node.parent = null;
                node.gCost = Int32.MaxValue;
                node.hCost = Int32.MaxValue;
                node.visited = false;
                new ModifyNode().ChangeColor(node.fieldCell, Color.white);
            }
        }
    }

    public void ExecuteAlg() {
        if (bfsActiv) {
            GameObject.Find("ExampleManager").GetComponent<BreadthFirstSearchLM>().Execute();
        }
        if (dfsActiv) {
            GameObject.Find("ExampleManager").GetComponent<DepthFirstSearchLM>().Execute();
        }
        if (aStarActiv) {
            GameObject.Find("ExampleManager").GetComponent<AStarAlgorithmLM>().Execute();
        }
    }
}