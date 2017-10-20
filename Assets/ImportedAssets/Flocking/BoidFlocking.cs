using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidFlocking : MonoBehaviour
{
	internal BoidController controller;

	IEnumerator Start()
	{
        Rigidbody rb = GetComponent<Rigidbody>();

        while (true)
		{
			if (controller)
			{
                Vector3 steerTowards = steer();
                steerTowards = new Vector3(steerTowards.x, 0, steerTowards.z);
                

				rb.velocity += steerTowards * Time.deltaTime;

                if (rb.velocity.z > 1)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z * -1);
                }

				// enforce minimum and maximum speeds for the boids
				float speed = rb.velocity.magnitude;
				if (speed > controller.maxVelocity)
				{
					GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * controller.maxVelocity;
				}
				else if (speed < controller.minVelocity)
				{
					GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * controller.minVelocity;
				}
			}
			float waitTime = Random.Range(0.3f, 0.5f);
			yield return new WaitForSeconds(waitTime);
		}
	}

	Vector3 steer()
	{
		Vector3 randomize = new Vector3((Random.value * 2) - 1, 0, (Random.value * 2) - 1);
		randomize.Normalize();
		randomize *= controller.randomness;

		Vector3 center = controller.flockCenter - transform.localPosition;
		Vector3 velocity = controller.flockVelocity - GetComponent<Rigidbody>().velocity;
		Vector3 follow = controller.target.localPosition - transform.localPosition;

		return (center + velocity + follow * controller.targetAttachment + randomize);
	}
}