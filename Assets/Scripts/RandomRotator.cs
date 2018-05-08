using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {
    public float tumble = 3;

	/// <summary>
    /// Initialize properties that should be set during run-time.
    /// Start rotating the attached object.
    /// </summary>
    void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.angularVelocity = Random.insideUnitSphere * tumble;
	}
}
