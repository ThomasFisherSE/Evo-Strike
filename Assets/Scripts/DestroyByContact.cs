using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyByContact : MonoBehaviour {
    public int scoreValue;
    private GameController gameController;
    private EvolutionController evolutionController;
    public GameObject explosion;
    public GameObject playerExplosion;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        evolutionController = gameControllerObject.GetComponent<EvolutionController>();

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

        // If colliding with a player, create an explosion on the player
        //if (other.tag == "Player")
        if (other.CompareTag("Player"))
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }

        gameController.AddScore(scoreValue);

        //Debug.Log("DestroyByContact: " + other.gameObject.name + " and " + gameObject.name);
        other.gameObject.SetActive(false);
        //Destroy(other.gameObject);

        if (gameObject.CompareTag("Enemy"))
        {
            evolutionController.AddCompletedEnemy(gameObject.GetComponent<Individual>());
        }

        Destroy(gameObject);
    }
}
