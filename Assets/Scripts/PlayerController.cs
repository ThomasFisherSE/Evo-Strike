using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float zMin, zMax;
    private float xMin, xMax;

    /// <summary>
    /// Accessor for the minimum x value of the boundary.
    /// </summary>
    /// <returns>The value of xMin.</returns>
    public float GetXMin()
    {
        return xMin;
    }

    /// <summary>
    /// Accessor for the maximum x value of the boundary.
    /// </summary>
    /// <returns>The value of xMax.</returns>
    public float GetXMax()
    {
        return xMax;
    }
    
    /// <summary>
    /// Set a new range in x for the boundary.
    /// </summary>
    /// <param name="min">The new minimum x value of the boundary.</param>
    /// <param name="max">The new maximum x value of the boundary.</param>
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
    public float fireRate = 4.0f;
    private float nextFire = 0.5f;

    /// <summary>
    /// Initialize properties that should be set during run-time.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.volume = PlayerPrefs.GetFloat("MasterVol");
        }

        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance)).x;
        float rightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance)).x;

        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        boundary.SetX(leftLimit, rightLimit);
        GameObject destroyBoundary = GameObject.FindGameObjectWithTag("Boundary");
        destroyBoundary.transform.localScale = new Vector3(screenWidth, 1, screenHeight);
        destroyBoundary.transform.position = new Vector3(0,0,Camera.main.transform.position.z);
    }

    /// <summary>
    /// Handles firing of weapons.
    /// </summary>
    void Update()
    {
        // Shooting
        if ((Input.GetButton("Fire") || Input.GetKey("space")) && (Time.time > nextFire))
        {
            nextFire = Time.time + 1/fireRate;

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

    /// <summary>
    /// Temporarily boost the player's fire-rate.
    /// </summary>
    /// <param name="multiplier">The multiplier to boost the fire-rate by.</param>
    /// <param name="secs">The time for which the fire-rate is increased.</param>
    public void FireRateBoost(float multiplier, float secs)
    {
        StartCoroutine(TempFireRateBoost(multiplier, secs));
    }

    /// <summary>
    /// Coroutine for temporarily boosting the player's fire-rate.
    /// </summary>
    /// <param name="multiplier">The multiplier to boost the fire-rate by.</param>
    /// <param name="secs">The time for which the fire-rate is increased.</param>
    /// <returns>A WaitForSeconds IEnumerator, allowing the coroutine to wait for some time.</returns>
    private IEnumerator TempFireRateBoost(float multiplier, float secs)
    {
        fireRate *= multiplier;
        yield return new WaitForSeconds(secs);
        Debug.Log("Fire rate power-up wore off.");
        fireRate /= multiplier;
    }

    /// <summary>
    /// Handles player movement.
    /// </summary>
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
