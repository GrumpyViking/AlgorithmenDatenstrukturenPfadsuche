using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject feldObjekt;
    GameObject field;
    // Start is called before the first frame update
    void Start()
    {
        
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
                field.name = "feld" + count;
                count++;
            }
        }
        
        GameObject.Find("Center").transform.position = new Vector3((x+feldObjekt.GetComponent<BoxCollider>().bounds.size.x +0.5f)/2,0,(y+feldObjekt.GetComponent<BoxCollider>().bounds.size.x +0.5f)/2);
        GameObject.Find("Main Camera").transform.position = new Vector3(GameObject.Find("Center").transform.position.x+2, 1, GameObject.Find("Center").transform.position.z);

    }
    // Update is called once per frame
    void LateUpdate()
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
}
