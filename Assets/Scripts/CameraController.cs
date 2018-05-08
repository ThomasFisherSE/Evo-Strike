using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera mainCamera;
    public Camera thirdPersonCamera;

    private Queue<Camera> cameraQueue;
    private GameObject bg;
    
	/// <summary>
    /// Initialize camera queue and prepare properties that need to be found during run-time.
    /// </summary>
    void Start () {
        // Create camera queue, with highest priority camera (main camera) first
        cameraQueue = new Queue<Camera>();
        cameraQueue.Enqueue(mainCamera);
        cameraQueue.Enqueue(thirdPersonCamera);

        bg = GameObject.FindWithTag("Background");
    }
	
	/// <summary>
    /// When 'ChangeView' is pressed, change the selected camera view
    /// </summary>
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


            bg.SetActive(mainCamera.enabled);
        }
    }
}
