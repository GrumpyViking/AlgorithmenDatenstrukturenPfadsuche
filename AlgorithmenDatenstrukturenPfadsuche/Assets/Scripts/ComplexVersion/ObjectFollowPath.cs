using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowPath : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 oldStartPosition, oldTargetPosition;
    public Grid g;
    private bool moving, reachedGoal;
    public float speed;
    private int index;
    private List<Node> tmp = new List<Node>();
    
    void Start()
    {
        GameObject.Find("Ship").transform.position = GameObject.Find("StartPosition").transform.position;
        oldStartPosition = GameObject.Find("StartPosition").transform.position;
        oldTargetPosition = GameObject.Find("TargetPosition").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Bei bewegung des Startpunktes wird Schiff mit verschoben
        if (GameObject.Find("StartPosition").transform.position != oldStartPosition)
        {
            GameObject.Find("Ship").transform.position = GameObject.Find("StartPosition").transform.position;
            oldStartPosition = GameObject.Find("StartPosition").transform.position;
            index = 0;
        }
        
        if (GameObject.Find("TargetPosition").transform.position != oldTargetPosition)
        {
            GameObject.Find("Ship").transform.position = GameObject.Find("StartPosition").transform.position;
            oldTargetPosition = GameObject.Find("TargetPosition").transform.position;
            index = 0;
        }

        if (Input.GetKey(KeyCode.Space) && moving ==false)
        {
            moving = true;
            
        }

        if (moving)
        {
            float step = speed * Time.deltaTime;
            if (!reachedGoal)
            {
                transform.position = Vector3.MoveTowards(transform.position, g.path[index].nodeGlobalPosition, step);
                if (index == g.path.Count-1)
                {
                    reachedGoal = true;
                }
                else
                {
                    index++;    
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, g.path[index].nodeGlobalPosition, step);
                if (index == 0)
                {
                    reachedGoal = false;
                }
                else
                {
                    index--;    
                }
            }

            
        }
    }
}