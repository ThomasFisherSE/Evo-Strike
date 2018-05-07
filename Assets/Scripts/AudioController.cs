using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    
    /*
     * Prepare the audio source on the game object the script is attached to
     */
	void Start () {
        AudioSource audioSrc = GetComponent<AudioSource>();

        if (audioSrc != null)
        {
            audioSrc.volume = PlayerPrefs.GetFloat("MasterVol");
        }
	}
}
