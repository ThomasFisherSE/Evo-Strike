using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed = 5;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    public void setSpeed(float s)
    {
        speed = s;

        if (rb != null && transform != null)
            rb.velocity = transform.forward * speed;
    }
}
