using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeAI : MonoBehaviour {

    private GameController gc;

    public float tilt;
    public float maxDodgeAmount;
    public float smoothing;
    public Vector2 startWait;
    public Vector2 dodgeTime;
    public Vector2 dodgeWait;
    public Boundary boundary;

    private Transform playerTransform;
    private float currentSpeed;
    private float targetPosition;
    private Rigidbody rb;

	/// <summary>
    /// Prepare properties that need to be set at run-time, and start the dodge algorithm.
    /// </summary>
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gc = gameControllerObject.GetComponent<GameController>();
        
        boundary.SetX(gc.MinXSpawn, gc.MaxXSpawn);

        rb = GetComponent<Rigidbody>();

        currentSpeed = rb.velocity.z;
        StartCoroutine(Dodge());
	}
	
	/// <summary>
    /// Move towards the dodge target
    /// </summary>
    void FixedUpdate () {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }

        float newDodge = Mathf.MoveTowards(rb.velocity.x, targetPosition, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newDodge, 0.0f, currentSpeed);

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.GetXMin(), boundary.GetXMax()),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin - 1, boundary.zMax));

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    /// <summary>
    /// Set a random dodge target position
    /// </summary>
    /// <returns>A WaitForSeconds IENumerator, causing the coroutine to wait for some time.</returns>
    IEnumerator Dodge()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            if (playerTransform != null)
            {
                // Dodge towards the player
                //targetPosition = Random.Range(1,dodge) * playerTransform.position.x;
                
                // Dodge inwards (i.e. to the right if x is -ve / left if x is +ve
                targetPosition = Random.Range(1, maxDodgeAmount) * -Mathf.Sign(transform.position.x);

            } else
            {
                // Dodge inwards (i.e. to the right if x is -ve / left if x is +ve
                targetPosition = Random.Range(1, maxDodgeAmount) * -Mathf.Sign(transform.position.x);
            }
            

            // Wait for dodge to complete
            yield return new WaitForSeconds(Random.Range(dodgeTime.x, dodgeTime.y));
            // Set the target back to 0
            targetPosition = 0;
            // Wait for some time before being able to dodge again
            yield return new WaitForSeconds(Random.Range(dodgeWait.x, dodgeWait.y));
        }
    }
}
