using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

/**
 * Funktion genutzt aus dem Praktikum Algorithmen und Datenstrukturen
 *
 * 
 */

public class AnimationQueue : MonoBehaviour {

    public float delaySeconds = 0.5f;

    private Queue<IAction> queue = new Queue<IAction>();

    public void enqueueAction(IAction action) {
        queue.Enqueue(action);
    }

    void Start() {
        StartCoroutine(worker());
    }

    IEnumerator worker() {
        while (true) {
            if (queue.Count != 0) {
                IAction action = queue.Dequeue();
                yield return action.Run();
            }
            yield return new WaitForSeconds(delaySeconds);
        }
    }
}