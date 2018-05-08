using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPowerUp : MonoBehaviour
{
    public int powerUpScore = 100;
    public float fireRateMultiplier = 2.5f;
    public int healthBoost = 1;

    public float boostTimer = 5.0f; // in seconds

    public AudioClip pickUpSound;

    private GameObject player;
    private GameController gc;
    private PlayerController pc;

    private bool isTriggered = false;
 
    /// <summary>
    /// Initialize properties that should be set during run-time.
    /// </summary>
    void Start()
    {
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            pc = player.GetComponent<PlayerController>();
        }
    }

    /// <summary>
    /// Handle collisions with other objects with colliders.
    /// </summary>
    /// <param name="other">The object that has been collided with.</param>
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return; // Only do something if colliding with player's ship
        }

        if (isTriggered) // Only trigger once
            return;

        isTriggered = true;
        //Debug.Log("Picked up " + gameObject.name + " power up");

        AudioSource audioSrc = player.GetComponent<AudioSource>();
        audioSrc.PlayOneShot(pickUpSound);
            
        Destroy(gameObject);

        switch (gameObject.name)
        {
            case "+FireRate(Clone)":
                Debug.Log("Fire rate increased by x" + fireRateMultiplier + " for " + boostTimer + "s.");
                // Increase fire rate
                pc.FireRateBoost(fireRateMultiplier, boostTimer);
                break;
            case "+Health(Clone)":
                Debug.Log("Health increased.");
                pc.health += healthBoost;
                gc.UpdateHealth();
                break;
            case "+Points(Clone)":
                Debug.Log("+" + powerUpScore + " points.");
                gc.AddScore(powerUpScore);
                break;
        }
    }
}
