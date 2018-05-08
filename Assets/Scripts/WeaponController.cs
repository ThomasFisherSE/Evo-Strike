using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public GameObject shot;
    public Transform shotSpawn;

    public Vector2 fireRate;

    public float delay;

    private AudioSource audioSource;

    private int nbShots;
    private int nbShotsOnTarget;

    private float accuracy; // Accuracy between 0 and 1
    
    /// <summary>
    /// Initialize properties that should be set during run-time.
    /// Start automatic firing.
    /// </summary>
    void Start()
    {
        float myFireRate = Random.Range(fireRate.x, fireRate.y);
        nbShots = 0;
        nbShotsOnTarget = 0;
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.volume = PlayerPrefs.GetFloat("MasterVol");
        }

        InvokeRepeating("Fire", delay, myFireRate);
    }

    /// <summary>
    /// Fire a projectile.
    /// </summary>
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

    /// <summary>
    /// Mark that a shot was on target, and update the accuracy.
    /// </summary>
    public void OnTargetShot()
    {
        nbShotsOnTarget++;

        accuracy = nbShotsOnTarget / (float)nbShots;
        //Debug.Log("nbShotsOnTarget = " + nbShotsOnTarget + " | nbShots = " + nbShots + "\nShot on target, new accuracy = " + accuracy);
    }

    /// <summary>
    /// Accessor for the accuracy attribute.
    /// </summary>
    /// <returns>The value of accuracy.</returns>
    public float GetAccuracy()
    {
        return accuracy;
    }

    /// <summary>
    /// Accessor for the nbShotsOnTarget attribute.
    /// </summary>
    /// <returns>The value of nbShotsOnTarget</returns>
    public int GetNbShotsOnTarget()
    {
        return nbShotsOnTarget;
    }
}
