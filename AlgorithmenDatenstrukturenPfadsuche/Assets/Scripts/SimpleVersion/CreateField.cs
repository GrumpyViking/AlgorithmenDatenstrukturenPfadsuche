using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/*
CreatField Klasse erstellt das Spielfeld und ist für den Programmverlauf der SimpleVersion Scene verantwortlich 
*/
public class CreateField : MonoBehaviour {
    public GameObject fieldCell; // Objekt aus dem das Feldbesteht (in Unity hinzufügen)
    public Vector2 fieldSize; // Größe des Spielfeldes (in Unity eintragen)
    private float fieldCellSize = 0.5f; // größe der einzelnen Felder
    private int fieldSizeXAxis, fieldSizeYAxis; // anzahl Felder auf der X bzw. Y Achse
    private float fieldCellDiameter; // Für Korrekte Feldgrößen berechnung notwendig 
    private List<LevelData> modifyedNodes = new List<LevelData>();
    public Node[,] fieldCellArray; // Zweidimensionales Array zum Speichern der einzelnen Felder und derer Eigenschaften
    private bool startSelected, targetSelected; // Status ob Start/Ziel ausgewählt wurde
    public List<Node> path; // Speichert den Pfad zwischen Start und Ziel
    private GameObject field; // Objekt als das die Felder erstellt werden
    public GameObject astarpanel, bfspanel, dfspanel; // Legenden für A*-Algoritmuse und Breitensuche Algoritmus
    public bool paused;
    public GameObject saveDialogPanel, loadDialogPanel;
    public RenderTexture renderTexture;
    public RawImage rawImage;
    public Dropdown levelList;

    #region Initialize
    /*
     * Initialisierung mit Programmstart
     */
    void Start() {
        fieldCellDiameter = fieldCellSize * 2;
        fieldSizeXAxis = Mathf.RoundToInt(fieldSize.x / fieldCellDiameter);
        fieldSizeYAxis = Mathf.RoundToInt(fieldSize.y / fieldCellDiameter);
        CreateFieldGrid();
        paused = false;
    }

    /*
     * Erstellen des Spielfeldes
     */
    private void CreateFieldGrid() {
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

    #endregion

    /*
     * Interaktion durch den Benutzer um Start, Ziel und Hindernisse zu bestimmen
     */
    void Update() {
        LeftClickAction();
        RightClickAction();
    }

    #region ModifyNodes with LeftClick

    private void LeftClickAction() {
        if (Input.GetMouseButtonDown(0) && !paused) // Linkemaustaste
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) // Von der aktuellen Mausposition wird ein Strahl gesendet wenn dieser ein Objekt trifft wird eine der Aktionen ausgeführt
            {
                if (!startSelected) {
                    GameObject target = hit.rigidbody.gameObject; // Liefert das getroffene Objekt zurück
                    SetStart(target);

                } else if (startSelected && !targetSelected) {
                    GameObject target = hit.rigidbody.gameObject;
                    SetTarget(target);
                } else if (startSelected && targetSelected) {
                    GameObject target = hit.rigidbody.gameObject;
                    SetBarricade(target);
                }
            }
        }
    }

    /*
    * Bestimmt das dass ausgewählte Feld eine Hindernis ist
    */
    private void SetBarricade(GameObject field) {
        new ModifyNode().ChangeColor(field, Color.yellow);
        foreach (Node node in fieldCellArray) {
            if (field.transform.position == node.GetGlobalPosition()) {
                node.traversable = false; // Feld als nicht mehr begehbar markiert
                LevelData mnode = new LevelData(node);
                mnode.index = node.index;
                modifyedNodes.Add(mnode);
            }
        }
    }

    /*
     * Bestimmt das dass ausgewählte Feld der Start ist
     */
    void SetStart(GameObject field) {
        new ModifyNode().ChangeColor(field, Color.green);
        foreach (Node node in fieldCellArray) {
            if (field.transform.position == node.GetGlobalPosition()) {
                node.start = true; // Feld wird als Startpunkt markiert
                LevelData mnode = new LevelData(node);
                mnode.index = node.index;
                modifyedNodes.Add(mnode);
            }
        }
        startSelected = true;
    }

    /*
     * Bestimmt das dass ausgewählte Feld das Ziel ist
     */
    void SetTarget(GameObject field) {
        new ModifyNode().ChangeColor(field, Color.red);
        foreach (Node node in fieldCellArray) {
            if (field.transform.position == node.GetGlobalPosition()) {
                node.target = true; // Feld wird als Zielpunkt markiert
                LevelData mnode = new LevelData(node);
                mnode.index = node.index;
                modifyedNodes.Add(mnode);
            }
        }
        targetSelected = true;
    }
    #endregion

    #region ChangeModifyedNodes with RightClick
    private void RightClickAction() {
        if (Input.GetMouseButtonDown(1) && !paused) {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) // Von der aktuellen Mausposition wird ein Strahl gesendet wenn dieser ein Objekt trifft wird eine der Aktionen ausgeführt
            {
                GameObject target = hit.rigidbody.gameObject;
                RemoveStatus(target);
            }
        }
    }

    void RemoveStatus(GameObject field) {
        foreach (Node node in fieldCellArray) {
            if (node.fieldCell == field) {
                if (node.start) {
                    RemoveElement(node);
                    node.start = false;
                    new ModifyNode().ChangeColor(node.fieldCell, Color.white);
                    startSelected = false;
                }

                if (node.target) {
                    RemoveElement(node);
                    node.target = false;
                    new ModifyNode().ChangeColor(node.fieldCell, Color.white);
                    targetSelected = false;
                }

                if (!node.traversable) {
                    RemoveElement(node);
                    node.traversable = true;
                    new ModifyNode().ChangeColor(node.fieldCell, Color.white);
                }
            }
        }
    }

    void RemoveElement(Node node) {
        int counter = 0;
        foreach (LevelData listElement in modifyedNodes) {
            if (listElement.index == node.index) {
                break;
            } else {
                counter++;
            }
        }
        modifyedNodes.RemoveAt(counter);
    }

    #endregion

    // Gibt das Felderarray an andere Klassen zurück
    public Node[,] GetArray() {
        return fieldCellArray;
    }

    /*
     * Liefert die Nachbarfelder eines Feldes als Liste zurück
     */
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

    /*
     * Anzeige der richtigen Legende zum gewählten Algoritmus
     */
    public void ShowPanel(Text panel) {
        switch (panel.text) {
            case "A* - Algorithmus":
                print("A*");
                GameObject.Find("GameManager").GetComponent<AStarAlgorithmAlt>().enabled = true;
                GameObject.Find("GameManager").GetComponent<BreadthFirstSearch>().enabled = false;
                GameObject.Find("GameManager").GetComponent<DepthFirstSearch>().enabled = false;
                astarpanel.SetActive(true);
                bfspanel.SetActive(false);
                dfspanel.SetActive(false);
                break;
            case "Breitensuche - Algorithmus":
                GameObject.Find("GameManager").GetComponent<AStarAlgorithmAlt>().enabled = false;
                GameObject.Find("GameManager").GetComponent<BreadthFirstSearch>().enabled = true;
                GameObject.Find("GameManager").GetComponent<DepthFirstSearch>().enabled = false;
                astarpanel.SetActive(false);
                bfspanel.SetActive(true);
                dfspanel.SetActive(false);
                break;
            case "Tiefensuche - Algorithmus":
                GameObject.Find("GameManager").GetComponent<AStarAlgorithmAlt>().enabled = false;
                GameObject.Find("GameManager").GetComponent<BreadthFirstSearch>().enabled = false;
                GameObject.Find("GameManager").GetComponent<DepthFirstSearch>().enabled = true;
                astarpanel.SetActive(false);
                bfspanel.SetActive(false);
                dfspanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    #region LevelPicture
    void ScreenShot(string filename) {
        string savePath = Application.dataPath + "/levels";
        try {
            if (!Directory.Exists(savePath)) {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(savePath);
            }
        } catch (IOException ioex) {
            Debug.Log(ioex.Message);
        }
        byte[] bytes = toTexture2D(renderTexture).EncodeToPNG();
        System.IO.File.WriteAllBytes(savePath + "/" + filename + ".png", bytes);
    }
    Texture2D toTexture2D(RenderTexture rTex) {
        Texture2D tex = new Texture2D(300, 300, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
    #endregion

    #region SaveLevel
    void ShowSaveDialog() {
        saveDialogPanel.SetActive(true);
        paused = true;
    }

    public void SaveLevel(Text name) {
        SaveSystem.levelName = name.text;
        ScreenShot(name.text);
        SaveSystem.SaveData(modifyedNodes);
        saveDialogPanel.SetActive(false);
        paused = false;
    }
    #endregion

    #region LoadingLevel
    public void ShowLoadDialog() {
        string filePath = Application.dataPath + "/levels";
        try {
            if (!Directory.Exists(filePath)) {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(filePath);
            }
        } catch (IOException ioex) {
            Debug.Log(ioex.Message);
        }
        DirectoryInfo dir = new DirectoryInfo(filePath);
        FileInfo[] names = dir.GetFiles("*.grid");
        levelList.options.Clear();
        foreach (FileInfo f in names) {
            levelList.options.Add(new Dropdown.OptionData(f.Name));
        }
        loadDialogPanel.SetActive(true);
        paused = true;
    }

    public void LoadLevel(Text filename) {
        ClearGrid();
        SavableData savedLevel = SaveSystem.LoadLevel(filename.text);

        foreach (Node node in fieldCellArray) {
            foreach (LevelData ld in savedLevel.saveNodes) {
                if (node.index == ld.index) {
                    if (ld.start) {
                        node.start = true;
                        new ModifyNode().ChangeColor(node.fieldCell, Color.green);
                        startSelected = true;
                    }
                    if (ld.target) {
                        node.target = true;
                        new ModifyNode().ChangeColor(node.fieldCell, Color.red);
                        targetSelected = true;
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
        loadDialogPanel.SetActive(false);
        paused = false;
    }

    void ClearGrid() {
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

    public void ChangePreview(Text name) {
        string filePath = Application.dataPath + "/levels";
        Texture2D preview;
        string filename;
        byte[] bytes;
        DirectoryInfo dir = new DirectoryInfo(filePath);
        FileInfo[] images = dir.GetFiles("*.png");
        foreach (FileInfo i in images) {
            if (i.Name.Substring(0, i.Name.Length - 5) == name.text.Substring(0, name.text.Length - 6)) {
                preview = new Texture2D(300, 300, TextureFormat.RGB24, false);
                filename = i.Name;
                bytes = File.ReadAllBytes(Application.dataPath + "/levels/" + filename);
                preview.LoadImage(bytes);
                rawImage.texture = preview;
            }
        }
    }

    #endregion
}