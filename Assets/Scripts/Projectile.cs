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
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance < closestDistance)
        {
            closestDistance = distance;
            //Debug.Log(this + " closest distance: " + closestDistance);
        }
    }

    public void SetWeaponController(WeaponController wc)
    {
        weaponController = wc;
    }

    public void SetSpeed(float s)
    {
        speed = s;

        if (rb != null && transform != null)
            rb.velocity = transform.forward * speed;
    }

    void OnDestroy()
    {
        if (closestDistance < onTargetDistance)
        {
            weaponController.OnTargetShot();
        }
    }
}
