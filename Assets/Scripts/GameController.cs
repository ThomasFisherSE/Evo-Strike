using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public Text scoreText;
    private int score;

    public float scoreFadeTime = 1;

    public Text addScoreText;
    public Text restartText;
    public Text gameOverText;
    public Text waveText;

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

    private EvolutionController evolutionController;

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
        evolutionController = GetComponent<EvolutionController>();
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScore();
        if (scoreToAdd > 0)
        {
            StartCoroutine(AddScoreMessage("+" + scoreToAdd, 2));
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
        // Get x and y cooridantes of corners of the screen, based off camera distance
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        // Wait to spawn first wave
        yield return new WaitForSeconds(startWait);

        int waveNumber = 0;
        
        evolutionController.CreateInitialPopulation();

        /*
        while (true)
        {
            waveNumber++;

            waveText.text = "Wave: " + waveNumber;

            // Add wave bonus
            AddScore((int) Mathf.Pow(2, waveNumber));

            // Spawn hazardCount hazards
            for (int i = 0; i < hazardCount; i++)
            {
                // Randomly select the type of hazard to spawn
                GameObject hazard = hazards[Random.Range(0,hazards.Length)];
                
                // Set the spawn position to be at a random x value along the top of the screen
                Vector3 spawnPosition = new Vector3(Random.Range(bottomCorner.x, topCorner.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;

                Instantiate(hazard, spawnPosition, spawnRotation);
                
                // Wait before spawning next wave
                yield return new WaitForSeconds(spawnWait);
            }

            if (Random.value <= powerUpChance)
            {
                //Debug.Log("Spawned power up");
                GameObject powerUp = powerUps[Random.Range(0, powerUps.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(bottomCorner.x, topCorner.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(powerUp, spawnPosition, spawnRotation);
            }

            yield return new WaitForSeconds(waveWait);
        }
        */
	}

    IEnumerator AddScoreMessage(string message, float delay)
    {
        addScoreText.text = message;
        addScoreText.enabled = true;
        StartCoroutine(FadeTextToZeroAlpha(scoreFadeTime, addScoreText));
        yield return new WaitForSeconds(delay);
    }

    public IEnumerator FadeTextToZeroAlpha(float fadeTime, Text text)
    {
        // Set text color's red, green, blue, and alpha components
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

        while (text.color.a > 0.0f)
        {
            // Subtract from the alpha value of the text color based on time passed and fade time
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }
}
