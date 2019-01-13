using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Camera cameraPlayer;
    public Transform target;
    public float rotSpeed = 60f;
    private float boarderSize = 10f;
    float scroll;

    //Zoom einstellungen
    public float maxZoom = 90f;
    public float zoomSpeed = 20f;
    public float minZoom = 20f;
    public float currentZoom = 65f;

    private MouseWheelManager mouseWheelManager;

    void Start() {
        mouseWheelManager = GameObject.Find("MouseWheelManager").GetComponent<MouseWheelManager>();
    }
    // Update is called once per frame
    void Update() {
        if (Input.mousePosition.x >= Screen.width - boarderSize) {
            transform.LookAt(target);
            transform.Translate((Vector3.right * Time.deltaTime) * rotSpeed);
        }
        if (Input.mousePosition.x <= boarderSize) {
            transform.LookAt(target);
            transform.Translate((Vector3.left * Time.deltaTime) * rotSpeed);
        }
        if (mouseWheelManager.GetAvailability()) {
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && currentZoom <= maxZoom)   //Heraus Zoomen
                                {
                scroll = Input.GetAxisRaw("Mouse ScrollWheel");

                cameraPlayer.fieldOfView -= scroll * zoomSpeed;
                currentZoom = cameraPlayer.fieldOfView;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentZoom >= minZoom) {
                scroll = Input.GetAxisRaw("Mouse ScrollWheel");
                cameraPlayer.fieldOfView -= scroll * zoomSpeed;
                currentZoom = cameraPlayer.fieldOfView;
            }
        }
    }
}
