using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler SharedInstance;

    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        // Create a new list of pooled objects
        pooledObjects = new List<GameObject>();

        // Instantiate amountToPool game objects, and set them as inactive
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            // Add the instantiated objects to the list of pooled objects
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        // Try to find an object that is not currently active, and return it
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        
        // If all objects in pooledObjects were active, return null
        return null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
