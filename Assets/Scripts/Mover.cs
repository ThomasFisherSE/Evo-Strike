using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed = 5;

    private Rigidbody rb;

    /// <summary>
    /// Set properties that should be set during run-time.
    /// Set the velocity of the attached object.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    /// <summary>
    /// Set a new speed value.
    /// </summary>
    /// <param name="s">The new speed value.</param>
    public void SetSpeed(float s)
    {
        speed = s;

        if (rb != null && transform != null)
            rb.velocity = transform.forward * speed;
    }
}
