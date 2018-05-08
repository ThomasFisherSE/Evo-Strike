using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {
    public float lifeTime;

    /// <summary>
    /// Destroy the game object the script is attached to after lifeTime has passed.
    /// </summary>
    void Start()
    {
        //Debug.Log("DestroyByTime: " + gameObject.name);

        Destroy(gameObject, lifeTime);
    }
}
