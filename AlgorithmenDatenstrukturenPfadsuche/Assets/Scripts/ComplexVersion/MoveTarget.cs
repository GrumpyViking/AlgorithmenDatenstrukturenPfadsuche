using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    
    

    private void Update()
    {
        // Ziel per Mausklick positionieren
        if (Input.GetMouseButtonDown(0))
        {
            var mouse = Input.mousePosition;
            var castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity)) transform.position = hit.point;
        }


        if (Input.GetKey(KeyCode.UpArrow)) transform.position += Vector3.forward; // Bewegt Zielpunkt nach vorne
        if (Input.GetKey(KeyCode.RightArrow)) transform.position += Vector3.right; // Bewegt Zielpunkt nach rechts 
        if (Input.GetKey(KeyCode.DownArrow)) transform.position += Vector3.back; // Bewegt Zielpunkt nach hinten
        if (Input.GetKey(KeyCode.LeftArrow)) transform.position += Vector3.left; // Bewegt Zielpunkt nach links
    }
}