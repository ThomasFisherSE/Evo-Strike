using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour {

    public float tilt;
    public float dodge;
    public float smoothing;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public Boundary boundary;

    private Transform playerTransform;
    private float currentSpeed;
    private float targetManeuver;
    private Rigidbody rb;

	void Start () {
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        boundary.SetX(bottomCorner.x, topCorner.x);

        rb = GetComponent<Rigidbody>();

        currentSpeed = rb.velocity.z;
        StartCoroutine(Evade());
	}
	
	void FixedUpdate () {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }

        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.getXMin(), boundary.getXMax()),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            if (playerTransform != null)
            {
                // Dodge towards the player
                //targetManeuver = Random.Range(1,dodge) * playerTransform.position.x;
                
                // Dodge inwards (i.e. to the right if x is -ve / left if x is +ve
                targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);

            } else
            {
                // Dodge inwards (i.e. to the right if x is -ve / left if x is +ve
                targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
            }
            

            // Wait for maneuver to complete
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            // Set the target back to 0
            targetManeuver = 0;
            // Wait for some time before being able to maneuver again
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }
}
