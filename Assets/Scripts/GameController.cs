using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public Text scoreText;
    private int score;

    public float scoreFadeTime;

    public Text addScoreText;
    public Text restartText;
    public Text gameOverText;

    private bool gameOver;
    private bool restart;

    public GameObject[] powerUps;
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float powerUpChance;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    private void Start()
    {
        gameOver = false;
        restart = false;
        addScoreText.text = "";
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
        if (newScoreValue > 0)
        {
            StartCoroutine(AddScoreMessage("+" + newScoreValue, 2));
        }
    }

    void Update()
    {
        if (gameOver)
        {
            restartText.text = "Press 'R' to Restart!\n(or 'M' for Main Menu)";
            restart = true;
        }

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                SceneManager.LoadScene(0);
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
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        yield return new WaitForSeconds(startWait);

        while (true)
        {

            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0,hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(bottomCorner.x, topCorner.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;

                Instantiate(hazard, spawnPosition, spawnRotation);

                yield return new WaitForSeconds(spawnWait);
            }

            if (Random.value <= powerUpChance)
            {
                Debug.Log("Spawned power up");
                GameObject powerUp = powerUps[Random.Range(0, powerUps.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(bottomCorner.x, topCorner.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(powerUp, spawnPosition, spawnRotation);
            }
            
            yield return new WaitForSeconds(waveWait);
        }
	}

    IEnumerator AddScoreMessage(string message, float delay)
    {
        addScoreText.text = message;
        addScoreText.enabled = true;
        //StartCoroutine(FadeTextToZeroAlpha(5f, addScoreText));
        StartCoroutine(FadeTextToZeroAlpha(scoreFadeTime, addScoreText));
        yield return new WaitForSeconds(delay);
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
