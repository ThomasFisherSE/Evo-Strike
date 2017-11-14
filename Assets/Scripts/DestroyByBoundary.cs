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
                // Debug.Log("DestroyByBoundary: " + other.gameObject.name);
                Destroy(other.gameObject);
            }
        }
    }
}
