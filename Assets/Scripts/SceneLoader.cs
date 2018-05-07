using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    private void Start()
    {
        AudioSource audioSrc = GetComponent<AudioSource>();

        if (audioSrc != null)
        {
            audioSrc.volume = PlayerPrefs.GetFloat("MasterVol");
        }
    }

    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
