using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateKraken : MonoBehaviour
{
    public Transform ship;

    float speed;

    void Start()
    {
        ship = GameObject.Find("Ship").transform;
        speed = 5f;
    }

    void Update()
    {
        Vector3 direction = ship.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);

    }
}
