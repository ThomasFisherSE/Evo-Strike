using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {
    public Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Camera>().enabled && target != null)
        {
            transform.position = target.position + new Vector3(0, 10, -15);
        }
	}
}
