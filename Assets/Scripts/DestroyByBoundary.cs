using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shot"))
        {
            other.gameObject.SetActive(false);
        }
        else
        {
            if (!other.CompareTag("Controller"))
            {
                if (other.CompareTag("Enemy"))
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
    }
}
