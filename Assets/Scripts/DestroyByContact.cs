using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyByContact : MonoBehaviour
{
    public int scoreValue;
    private GameController gameController;
    private EvolutionController evolutionController;
    private PlayerController playerController;
    public GameObject explosion;
    public GameObject playerExplosion;
    public GameObject playerDamaged;

    public GameObject[] powerUps;

    public float powerUpRate = 1.0f;

    private bool isTriggered = false;

    /// <summary>
    /// Prepare class properties that need to be set during run-time
    /// </summary>
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        evolutionController = gameControllerObject.GetComponent<EvolutionController>();

        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }
        else
        {
            Debug.Log("Could not find player object. It may have been destroyed.");
        }

        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Could not find GameController script.");
        }

    }

    /// <summary>
    /// Handle collisions with other objects
    /// </summary>
    /// <param name="other">The collider of the object that is being collided with.</param>
    void OnTriggerEnter(Collider other)
    {
        // Do nothing if colliding with boundary / enemy / powerup / controller / if already triggered
        if (other.CompareTag("Boundary") || other.tag.Contains("Enemy") || other.CompareTag("PowerUp") || other.CompareTag("Controller") || isTriggered)
        {
            //Debug.Log(gameObject.name + " collided with " + other.gameObject.name + ". Nothing was destroyed.");
            return;
        }
        
        // Do nothing if two shots collide
        if (gameObject.tag.Contains("Shot") && other.tag.Contains("Shot")) {
            return;
        }

        // Otherwise, triggered
        isTriggered = true;

        if (explosion != null)
        {
            // Create an explosion on the GameObject
            Instantiate(explosion, transform.position, transform.rotation);
        }

        // Enemy ship collision
        if (gameObject.CompareTag("EnemyShip"))
        {
            if (gameObject != null)
            {
                evolutionController.AddCompletedEnemy(gameObject, false);

                // Roll for power-up
                float rand = Random.value;

                if (rand < powerUpRate)
                {
                    int powerUpId = Random.Range(0, powerUps.Length);
                    Instantiate(powerUps[powerUpId], transform.position, transform.rotation);
                }
                //Debug.Log("Rolled for power up, got " + rand + ", needed under " + powerUpRate);
            }

            //gameObject.SetActive(false);
            //Debug.Log(gameObject + " destroyed.");
            Destroy(gameObject);
        }
        else
        {
            //Debug.Log(gameObject + " destroyed.");
            Destroy(gameObject);
        }

        // Player collision
        if (other.CompareTag("Player"))
        {
            playerController.health--;
            gameController.UpdateHealth();

            if (playerController.health <= 0)
            {
                Debug.Log("Player destroyed.");
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                
                gameController.AddScore(scoreValue);
                other.gameObject.SetActive(false);
                gameController.GameOver();
            } else
            {
                Instantiate(playerDamaged, transform.position, transform.rotation);
            }

            gameObject.SetActive(false);

            return;
        }

        gameController.AddScore(scoreValue);

        //Debug.Log("DestroyByContact: " + other.gameObject.name + " and " + gameObject.name);

        other.gameObject.SetActive(false);
        //Destroy(other.gameObject);
    }
}
