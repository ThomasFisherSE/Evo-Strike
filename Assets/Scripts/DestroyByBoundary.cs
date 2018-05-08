using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

    /// <summary>
    /// Handle collisions with other objects
    /// </summary>
    /// <param name="other">The collider of the object that is being collided with.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shot"))
        {
            other.gameObject.SetActive(false);
            return;
        }

        if (other.CompareTag("Controller"))
        {
            return;
        }

        if (other.CompareTag("EnemyShip"))
        {
            GameObject gameControllerObject = GameObject.FindWithTag("GameController");
            EvolutionController ec = gameControllerObject.GetComponent<EvolutionController>();
            ec.AddCompletedEnemy(other.gameObject, true);
            //Debug.Log("Enemy completed by hitting boundary.");
        }

        //Debug.Log("DestroyByBoundary: " + other.gameObject.name);
        Destroy(other.gameObject);
    }
}
