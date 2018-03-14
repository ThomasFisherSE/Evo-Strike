using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public GameObject shot;
    public Transform shotSpawn;

    public Vector2 fireRate;
    //public float minFireRate;
    //public float maxFireRate;

    public float delay;

    private AudioSource audioSource;

    private int nbShots;
    private int nbShotsOnTarget;

    private float accuracy; // Accuracy between 0 and 1

    // Use this for initialization
    void Start()
    {
        float myFireRate = Random.Range(fireRate.x, fireRate.y);
        nbShots = 0;
        nbShotsOnTarget = 0;
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("Fire", delay, myFireRate);
    }

    void Fire()
    {
        if (this.gameObject.activeSelf)
        {
            nbShots++;

            GameObject projectile = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

            projectile.GetComponent<Projectile>().SetWeaponController(this);

            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }

    public void OnTargetShot()
    {
        nbShotsOnTarget++;

        accuracy = nbShotsOnTarget / (float)nbShots;

        //Debug.Log("nbShotsOnTarget = " + nbShotsOnTarget + " | nbShots = " + nbShots + "\nShot on target, new accuracy = " + accuracy);
    }

    public float GetAccuracy()
    {
        return accuracy;
    }

    public int GetNbShotsOnTarget()
    {
        return nbShotsOnTarget;
    }
}
