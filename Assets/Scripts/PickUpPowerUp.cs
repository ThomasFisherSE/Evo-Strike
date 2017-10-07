using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPowerUp : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Picked up " + gameObject.name + " power up");
            Destroy(gameObject);

            switch (gameObject.name)
            {
                case "+FireRate(Clone)":
                    Debug.Log("Fire rate increased");
                    // Increase fire rate by 10%
                    PlayerController.fireRate /= 1.1f;
                    break;
            }
        }
    }
}
