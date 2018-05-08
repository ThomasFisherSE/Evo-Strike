using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    /// <summary>
    /// Initialize properties that should be set during run-time.
    /// Set the scene volume to the player's chosen volume.
    /// </summary>
    private void Start()
    {
        AudioSource audioSrc = GetComponent<AudioSource>();

        if (audioSrc != null)
        {
            audioSrc.volume = PlayerPrefs.GetFloat("MasterVol");
        }
    }

    /// <summary>
    /// Load a scene.
    /// </summary>
    /// <param name="level">The index of the scene to load.</param>
    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    /// <summary>
    /// Exit the game.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
