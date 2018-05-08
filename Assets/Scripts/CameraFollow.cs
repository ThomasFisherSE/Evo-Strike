using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;
	
	/// <summary>
    /// Make the camera follow the target
    /// </summary>
    void Update () {
        if (GetComponent<Camera>().enabled && target != null)
        {
            transform.position = target.position + new Vector3(0, 10, -15);
        }
	}
}
