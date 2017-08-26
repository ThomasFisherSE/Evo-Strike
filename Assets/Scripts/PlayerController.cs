using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
    private AudioSource audioSource;

    public float speed = 10;
    public float tilt = 2;
    public Boundary boundary;

    public GameObject shot;
    public AudioClip shotSound;
    public Transform[] shotSpawns;
    public float fireRate = 0.25f;
    private float nextFire = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Shooting
        if ((Input.GetButton("Fire1") || Input.GetKeyDown("space")) && (Time.time > nextFire))
        {
            nextFire = Time.time + fireRate;

            foreach(var shotSpawn in shotSpawns)
            {
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            }
            
            audioSource.clip = shotSound;
            audioSource.Play();
        }
    }

    void FixedUpdate()
    {
        // Movement
        Rigidbody rb = GetComponent<Rigidbody>();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.velocity = movement * speed;

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 
            0.0f, 
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
