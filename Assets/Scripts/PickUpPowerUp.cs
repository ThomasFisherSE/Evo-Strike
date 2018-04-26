using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPowerUp : MonoBehaviour
{
    public AudioClip pickUpSound;

    private GameObject player;
    private GameController gc;
    private PlayerController pc;
    private AudioSource audioSrc;

    // Use this for initialization
    void Start()
    {
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindWithTag("Player");
        audioSrc = GetComponent<AudioSource>();

        if (player != null)
        {
            pc = player.GetComponent<PlayerController>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Picked up " + gameObject.name + " power up");

            AudioSource audioSrc = player.GetComponent<AudioSource>();
            audioSrc.PlayOneShot(pickUpSound);
            
            Destroy(gameObject);

            switch (gameObject.name)
            {
                case "+FireRate(Clone)":
                    Debug.Log("Fire rate increased");
                    // Increase fire rate by 10%
                    pc.fireRate *= 1.1f;
                    break;
                case "+Health":
                    Debug.Log("Health increased.");
                    pc.health++;
                    gc.UpdateHealth();
                    break;
            }
        }
    }
}
