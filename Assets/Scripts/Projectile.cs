using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed = 5;

    public Transform target;

    public float onTargetDistance = 5.0f; // Distance from target that is required for shot to be considered 'on target'

    private Rigidbody rb;
    private WeaponController weaponController;

    private float closestDistance = 999f;
    
    /// <summary>
    /// Initialize properties that should be set during run-time.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    /// <summary>
    /// Update the closest distance of the projectile to its target.
    /// </summary>
    private void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance < closestDistance)
        {
            closestDistance = distance;
        }
    }

    /// <summary>
    /// Set a new weapon controller.
    /// </summary>
    /// <param name="wc">The new weapon controller.</param>
    public void SetWeaponController(WeaponController wc)
    {
        weaponController = wc;
    }

    /// <summary>
    /// Set a new speed value.
    /// </summary>
    /// <param name="s">The new speed value.</param>
    public void SetSpeed(float s)
    {
        speed = s;

        if (rb != null && transform != null)
            rb.velocity = transform.forward * speed;
    }

    /// <summary>
    /// Check if the shot was on target.
    /// </summary>
    void OnDestroy()
    {
        if (closestDistance < onTargetDistance)
        {
            weaponController.OnTargetShot();
            //Debug.Log("Shot on target.");
        }
    }
}
