using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject feldObjekt;
    GameObject field, start, end;
    public bool aStar, build;
    // Start is called before the first frame update
    void Start()
    {
        aStar = false;
        build = false;
    }

    public void GenerateField(){
        int x = 0;
        int y = 0;
        int count = 0;
        
        if (GameObject.Find("RowIP").GetComponent<InputField>().text == "")
        {
            x = int.Parse(GameObject.Find("RowIP").GetComponent<InputField>().placeholder.GetComponent<Text>().text);
        }
        else
        {
            x = int.Parse(GameObject.Find("RowIP").GetComponent<InputField>().text);
        }

        if (GameObject.Find("ColIP").GetComponent<InputField>().text == "")
        {
            y = int.Parse(GameObject.Find("ColIP").GetComponent<InputField>().placeholder.GetComponent<Text>().text);
        }
        else
        {
            y = int.Parse(GameObject.Find("ColIP").GetComponent<InputField>().text);
        }

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                field = Instantiate(feldObjekt, new Vector3(i + (feldObjekt.GetComponent<BoxCollider>().bounds.size.x +0.5f), 0, j + (feldObjekt.GetComponent<BoxCollider>().bounds.size.z+0.5f)), Quaternion.identity);
                field.GetComponent<FieldProps>().traverseable = true;
                field.GetComponent<FieldProps>().cordX = i;
                field.GetComponent<FieldProps>().cordY = j;
                field.name = "feld" + count;
                count++;
            }
        }
        
        GameObject.Find("Center").transform.position = new Vector3((x+feldObjekt.GetComponent<BoxCollider>().bounds.size.x +0.5f)/2,0,(y+feldObjekt.GetComponent<BoxCollider>().bounds.size.x +0.5f)/2);
        GameObject.Find("Main Camera").transform.position = new Vector3(GameObject.Find("Center").transform.position.x+2, 1, GameObject.Find("Center").transform.position.z);
        build = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (build)
        {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit = new RaycastHit();
                GameObject target = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction * 10, out hit)) {
                    target = hit.collider.gameObject;
                    if (target.GetComponent<FieldProps>().traverseable) {
                        target.GetComponent<Renderer>().material.color = Color.red;
                        target.GetComponent<FieldProps>().traverseable = false;
                    } 
                }
            }
        
            if (Input.GetMouseButtonDown(1)) {
                RaycastHit hit = new RaycastHit();
                GameObject target = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction * 10, out hit)) {
                    target = hit.collider.gameObject;
                    if (!target.GetComponent<FieldProps>().traverseable) {
                        target.GetComponent<Renderer>().material.color = Color.white;
                        target.GetComponent<FieldProps>().traverseable = true;
                    } 
                }
            }
        }

        if (aStar)
        {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit = new RaycastHit();
                if (start == null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray.origin, ray.direction * 10, out hit)) {
                        start = hit.collider.gameObject;
                        if (start.GetComponent<FieldProps>().traverseable) {
                            start.GetComponent<Renderer>().material.color = Color.green;
                        } 
                    }
                }
                
            }
            if (Input.GetMouseButtonDown(1)) {
                RaycastHit hit = new RaycastHit();
                if (end == null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray.origin, ray.direction * 10, out hit)) {
                        end= hit.collider.gameObject;
                        if (end.GetComponent<FieldProps>().traverseable) {
                            end.GetComponent<Renderer>().material.color = Color.cyan;
                        } 
                    }
                }
               
            }
        }
        
    }

    public void AStarAlgorithm()
    {
        GameObject.Find("Commands").GetComponent<Text>().text = "Start Punktwählen!";
        GameObject.Find("Hinweis").GetComponent<Text>().text = "links Klick Startpunkt wählen (einfärbung grün) \n rechts Klick end punkt wählen (einfärbung cyan)";
        aStar = true;
        build = false;

        if (start != null && end != null)
        {
            Stack<GameObject> path = new Stack<GameObject>();
            GameObject next = null;
            next = nextField(start);
            start.GetComponent<FieldProps>().isPath = true;
            path.Push(start);
            while (path.Count != 0)
            {
                path.Push(next);   
                next = nextField(next);
                if (next == end)
                {
                    GameObject.Find("Commands").GetComponent<Text>().text = "Pfad gefunden!";   
                    break;   
                    
                }   
                
            }

            if (next == null)
            {
                GameObject.Find("Commands").GetComponent<Text>().text ="Kein Pfad gefunden!";  
                return;   
                
            }
        }
    }

    int ManhattenHeuristik(GameObject start, GameObject end)
    {
        int gcost = 2;
        return (((Math.Abs(start.GetComponent<FieldProps>().cordX - end.GetComponent<FieldProps>().cordX) + Math.Abs(start.GetComponent<FieldProps>().cordY - end.GetComponent<FieldProps>().cordY))*gcost)+gcost);
    }

    GameObject nextField(GameObject current)
    {
        return null;
    }
}
