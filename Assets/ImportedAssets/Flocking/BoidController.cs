using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// these define the flock's behavior
/// </summary>
public class BoidController : MonoBehaviour
{
    public int targetAttachment;
    public float minVelocity = 5;
	public float maxVelocity = 20;
	public float randomness = 1;
	public int flockSize = 20;
	public BoidFlocking prefab;
	public Transform target;

	internal Vector3 flockCenter;
	internal Vector3 flockVelocity;

	List<BoidFlocking> boids = new List<BoidFlocking>();

	void Start()
	{
        SpawnWave();
	}
    
    void SpawnWave()
    {
        for (int i = 0; i < flockSize; i++)
        {
            // Instantiate a new enemy at the transform position of the controller
            BoidFlocking boid = Instantiate(prefab, transform.position, transform.rotation) as BoidFlocking;
            boid.transform.parent = transform;
            Vector3 boidLocalPosition = new Vector3(
                            Random.value * GetComponent<Collider>().bounds.size.x,
                            Random.value * GetComponent<Collider>().bounds.size.y,
                            Random.value * GetComponent<Collider>().bounds.size.z) - GetComponent<Collider>().bounds.extents;

            // Set y value to 0
            boidLocalPosition = new Vector3(boidLocalPosition.x, 0, boidLocalPosition.z);

            boid.transform.localPosition = boidLocalPosition;
            boid.controller = this;
            boids.Add(boid);
        }
    }

	void Update()
	{
		Vector3 center = Vector3.zero;
		Vector3 velocity = Vector3.zero;
		foreach (BoidFlocking boid in boids)
		{
			center += boid.transform.localPosition;
			velocity += boid.GetComponent<Rigidbody>().velocity;
		}
		flockCenter = center / flockSize;
		flockVelocity = velocity / flockSize;
	}
}