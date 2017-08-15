﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {
    public int scoreValue;

    private GameController gameController;
    public GameObject explosion;
    public GameObject playerExplosion;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Could not find GameController script.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject != null)
        {
            // Do nothing if colliding with boundary
            if (other.tag == "Boundary")
            {
                return;
            }

            // Create an explosion on the GameObject
            Instantiate(explosion, transform.position, transform.rotation);

            // If colliding with a player, create an explosion on the player
            if (other.tag == "Player")
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
            }

            gameController.AddScore(scoreValue);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
    }
}