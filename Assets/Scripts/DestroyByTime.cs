using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {
    public float lifeTime;

    void Start()
    {
        Debug.Log("DestroyByTime: " + gameObject.name);
        Destroy(gameObject, lifeTime);
    }
}
