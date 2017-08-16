using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public Text scoreText;
    private int score;

    public Text restartText;
    public Text gameOverText;

    private bool gameOver;
    private bool restart;

    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    private void Start()
    {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }
        }
    }

    void RestartLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    IEnumerator SpawnWaves () {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0,hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;

                Instantiate(hazard, spawnPosition, spawnRotation);

                yield return new WaitForSeconds(spawnWait);
            }

            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'R' to Restart!";
                restart = true;
                break;
            }
        }
	}
}
