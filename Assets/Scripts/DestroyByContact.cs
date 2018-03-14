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

    void OnTriggerEnter(Collider other)
    {
        // Do nothing if colliding with boundary / enemy / powerup / controller
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("PowerUp") || other.CompareTag("Controller"))
        {
            return;
        }

        if (explosion != null)
        {
            // Create an explosion on the GameObject
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (gameObject.CompareTag("Enemy"))
        {
            if (gameObject != null)
            {
                evolutionController.AddCompletedEnemy(gameObject, false);
            }

            //gameObject.SetActive(false);
            //Debug.Log(gameObject + " disabled.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log(gameObject + " destroyed.");
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
