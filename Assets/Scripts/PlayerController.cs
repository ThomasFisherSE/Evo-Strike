using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float zMin, zMax;
    private float xMin, xMax;

    public float GetXMin()
    {
        return xMin;
    }

    public float GetXMax()
    {
        return xMax;
    }

    public void SetX(float min, float max)
    {
        xMin = min;
        xMax = max;
    }
}

public class PlayerController : MonoBehaviour
{
    private AudioSource audioSource;

    public int health = 1;
    public float speed = 10;
    public float tilt = 2;
    public Boundary boundary;

    public GameObject shot;
    public AudioClip shotSound;
    public Transform[] shotSpawns;
    public static float fireRate = 0.25f;
    private float nextFire = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        boundary.SetX(bottomCorner.x, topCorner.x);
    }

    void Update()
    {
        // Shooting
        if ((Input.GetButton("Fire1") || Input.GetKey("space")) && (Time.time > nextFire))
        {
            nextFire = Time.time + fireRate;

            foreach (var shotSpawn in shotSpawns)
            {
                // Get the bullet from the object pooler
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject();

                if (bullet != null)
                {
                    // Move the bullet to the location of the shotSpawn, and activate it
                    bullet.transform.position = shotSpawn.transform.position;
                    bullet.transform.rotation = shotSpawn.transform.rotation;
                    bullet.SetActive(true);
                }
                //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
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
            Mathf.Clamp(rb.position.x, boundary.GetXMin(), boundary.GetXMax()),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
