using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations;

/**
 * Steuerung der Animation des Kraken
 *
 * Martin Schuster
 */
public class AnimateKraken : MonoBehaviour {
    private Transform ship;
    private Animator controller;
    int hit = Animator.StringToHash("IsAttacking");
    float speed;
    void Start() {
        ship = GameObject.Find("Ship").transform;
        speed = 5f;
        controller = this.GetComponent<Animator>();
    }

    void Update() {
        Vector3 direction = ship.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);

        // Führt die Attack-Animation aus wenn das Schiff in einer entfernung von 15 Einheiten ist
        if (Vector3.Distance(transform.position, ship.position) < 15) {
            controller.SetBool("IsAttacking", true);
        } else {
            controller.SetBool("IsAttacking", false);
        }



    }
}
