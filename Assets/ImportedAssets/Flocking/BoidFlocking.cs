using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidFlocking : MonoBehaviour
{
	internal BoidController controller;

    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.linearVelocity.x * -5);
    }

    IEnumerator Start()
	{
        Rigidbody rb = GetComponent<Rigidbody>();

        while (true)
		{
			if (controller)
			{
                Vector3 steerTowards = steer();
                steerTowards = new Vector3(steerTowards.x, 0, steerTowards.z);
                

				rb.linearVelocity += steerTowards * Time.deltaTime;

                if (rb.linearVelocity.z > 1)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z * -1);
                }

                // enforce minimum and maximum speeds for the boids
                float speed = rb.linearVelocity.magnitude;
				if (speed > controller.maxVelocity)
				{
					GetComponent<Rigidbody>().linearVelocity = GetComponent<Rigidbody>().linearVelocity.normalized * controller.maxVelocity;
				}
				else if (speed < controller.minVelocity)
				{
					GetComponent<Rigidbody>().linearVelocity = GetComponent<Rigidbody>().linearVelocity.normalized * controller.minVelocity;
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
		Vector3 velocity = controller.flockVelocity - GetComponent<Rigidbody>().linearVelocity;
		Vector3 follow = controller.target.localPosition - transform.localPosition;

		return (center + velocity + follow * controller.targetAttachment + randomize);
	}
}