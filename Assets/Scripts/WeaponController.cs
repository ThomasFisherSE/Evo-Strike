using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public GameObject shot;
    public Transform shotSpawn;

    public Vector2 fireRate;
    //public float minFireRate;
    //public float maxFireRate;

    public float delay;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        float myFireRate = Random.Range(fireRate.x, fireRate.y);
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("Fire", delay, myFireRate);
	}
	
	void Fire()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
