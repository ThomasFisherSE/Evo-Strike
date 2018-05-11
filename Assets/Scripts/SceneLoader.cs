using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public const int MAIN_MENU = 0, GAME = 1, CREDITS = 2, OPTIONS = 3;

    private int currentIdx;
    private static int prevIdx = 0;

    /// <summary>
    /// Initialize properties that should be set during run-time.
    /// Set the scene volume to the player's chosen volume.
    /// </summary>
    private void Start()
    {
        currentIdx = SceneManager.GetActiveScene().buildIndex;

        AudioSource audioSrc = GetComponent<AudioSource>();

        if (audioSrc != null)
        {
            audioSrc.volume = PlayerPrefs.GetFloat("MasterVol");
        }
    }

    /// <summary>
    /// Check for navigation button input
    /// </summary>
    private void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            LoadScene(prevIdx);
        }

        if (Input.GetButton("Submit"))
        {
            if (currentIdx == MAIN_MENU)
            {
                LoadScene(GAME);
            }
        }
    }

    /// <summary>
    /// Load a scene.
    /// </summary>
    /// <param name="level">The index of the scene to load.</param>
    public void LoadScene(int level)
    {
        prevIdx = currentIdx;
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
