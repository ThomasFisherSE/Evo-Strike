using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public GameObject shot;
    public Transform shotSpawn;

    public float minFireRate;
    public float maxFireRate;

    public float delay;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        float fireRate = Random.Range(minFireRate, maxFireRate);
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("Fire", delay, fireRate);
	}
	
	void Fire()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        audioSource.Play();
    }
}
