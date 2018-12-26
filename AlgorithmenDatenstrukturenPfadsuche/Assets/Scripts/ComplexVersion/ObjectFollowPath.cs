using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowPath : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 oldPosition;
    public Grid g;
    private bool moving;
    public float speed;
    private int index;
    void Start()
    {
        GameObject.Find("Ship").transform.position = GameObject.Find("StartPosition").transform.position;
        oldPosition = GameObject.Find("Ship").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (oldPosition != GameObject.Find("StartPosition").transform.position)
        {
            GameObject.Find("Ship").transform.position = GameObject.Find("StartPosition").transform.position;
            oldPosition = GameObject.Find("Ship").transform.position;
        }

        if (Input.GetKey(KeyCode.Space) && moving ==false)
        {
            moving = true;
            
        }

        if (moving)
        {
            if (g.path.Count>0)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, g.path[index].nodeGlobalPosition, step);
            }
            else
            {
                moving = false;
            }

            if (index < g.path.Count-1)
            {
                index++;  
            }
            
        }
    }

    private void FollowPath()
    {
        foreach (Node node in g.path)
        {
            StartCoroutine(Move_Routine(this.transform,  GameObject.Find("Ship").transform.position, node.nodeGlobalPosition));
        }
    }
    
    private IEnumerator Move_Routine(Transform transform, Vector3 from, Vector3 to)
    {
        float t = 0f;
        while(t < 5f)
        {
            t += Time.deltaTime;
            GameObject.Find("Ship").transform.position = Vector3.RotateTowards(from, to,15,1);
            GameObject.Find("Ship").transform.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
            Debug.Log(GameObject.Find("Ship").transform.position);
            yield return null;
        }
    }
}