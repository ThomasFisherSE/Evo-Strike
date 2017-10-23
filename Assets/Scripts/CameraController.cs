using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera mainCamera;
    public Camera thirdPersonCamera;

    private Queue<Camera> cameraQueue;

	// Use this for initialization
	void Start () {
        // Create camera queue, with highest priority camera (main camera) first
        cameraQueue = new Queue<Camera>();
        cameraQueue.Enqueue(mainCamera);
        cameraQueue.Enqueue(thirdPersonCamera);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("ChangeView"))
        {
            // Remove original camera from the queue and disable it
            Debug.Log("Change Camera");
            Camera originalCamera = cameraQueue.Peek();
            originalCamera.enabled = false;
            cameraQueue.Dequeue();

            // Change to next camera
            Camera nextCamera = cameraQueue.Peek();
            Debug.Log("Next Camera: " + nextCamera);
            nextCamera.enabled = true;
            cameraQueue.Enqueue(originalCamera);
        }
    }
}
