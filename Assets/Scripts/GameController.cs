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
    public Text healthText;

    private int waveNumber;

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

    private PlayerController playerController;
    private EvolutionController evolutionController;
    private float camDistance;

    private Vector2 bottomCorner, topCorner;

    private float minXSpawn, maxXSpawn, zSpawn;

    /// <summary>
    /// Initialise properties that need to be set during run-time.
    /// </summary>
    private void Start()
    {
        AudioSource audio =  GetComponent<AudioSource>();

        if (audio != null)
        {
            audio.volume = PlayerPrefs.GetFloat("MasterVol");
        }

        gameOver = false;
        restart = false;
        addScoreText.text = "";
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        
        StartCoroutine(SpawnWaves());
        evolutionController = GetComponent<EvolutionController>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        UpdateHealth();

        // Get x and y cooridantes of corners of the screen, based off camera distance
        camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        MinXSpawn = bottomCorner.x;
        MaxXSpawn = topCorner.x;
    }

    /// <summary>
    /// Update the health UI
    /// </summary>
    public void UpdateHealth()
    {
        healthText.text = "Health: " + playerController.health;
    }

    /// <summary>
    /// Mark that the game is over.
    /// </summary>
    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
    }

    /// <summary>
    /// Add to the player's score.
    /// </summary>
    /// <param name="scoreToAdd">The number of points to add to the score.</param>
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScore();
        if (scoreToAdd > 0)
        {
            StartCoroutine(AddScoreMessage("+" + scoreToAdd, 2));
        }
    }

    /// <summary>
    /// Check for input from the player to restart the game / go to the main menu.
    /// </summary>
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

    /// <summary>
    /// Restart the level.
    /// </summary>
    void RestartLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    /// <summary>
    /// Update the score UI
    /// </summary>
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    /// <summary>
    /// Spawn asteroids for the wave.
    /// </summary>
    /// <returns>A WaitForSeconds IEnumerator, allowing the couroutine to wait for some time.</returns>
    IEnumerator SpawnHazards()
    {
        // Spawn hazardCount hazards
        for (int i = 0; i < hazardCount; i++)
        {
            // Randomly select the type of hazard to spawn
            GameObject hazard = hazards[Random.Range(0, hazards.Length)];

            // Set the spawn position to be at a random x value along the top of the screen
            Vector3 spawnPosition = new Vector3(Random.Range(MinXSpawn, MaxXSpawn), spawnValues.y, spawnValues.z);
            Quaternion spawnRotation = Quaternion.identity;

            Instantiate(hazard, spawnPosition, spawnRotation);

            // Wait before spawning next enemy
            yield return new WaitForSeconds(spawnWait);
        }
    }
  
    /// <summary>
    /// Continuously spawns waves until the game is over.
    /// </summary>
    /// <returns>A WaitForSeconds IEnumerator, allowing the couroutine to wait for some time.</returns>
    IEnumerator SpawnWaves () {
        // Wait to spawn first wave
        yield return new WaitForSeconds(startWait);

        WaveNumber = 0;

        StartCoroutine(SpawnHazards());
        
        // Spawn initial population
        StartCoroutine(evolutionController.SpawnInitialPopulation());
        
        while (true)
        {
            if (gameOver)
            {
                break;
            }

            Debug.Log("************* Spawning next wave ************");

            if (WaveNumber != 0 && !gameOver)
            {
                evolutionController.NextGeneration();
                StartCoroutine(SpawnHazards());
            }

            WaveNumber++;

            waveText.text = "Wave: " + WaveNumber;

            // Add wave bonus
            AddScore(WaveNumber * 100);

            // Wait before spawning next wave
            while(!evolutionController.WaveComplete)
            {
                yield return null;
            }


            //yield return new WaitUntil(() => evolutionController.WaveComplete() == true);
            yield return new WaitForSeconds(waveWait);
        
        }
    }
    
    /// <summary>
    /// Display a temporary UI message showing recently added points.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="delay">The time the message should be displayed for.</param>
    /// <returns>A WaitForSeconds IEnumerator, allowing the couroutine to wait for some time.</returns>
    IEnumerator AddScoreMessage(string message, float delay)
    {
        addScoreText.text = message;
        addScoreText.enabled = true;
        StartCoroutine(FadeText(scoreFadeTime, addScoreText));
        yield return new WaitForSeconds(delay);
    }

    /// <summary>
    /// Fade UI text until it is transparent.
    /// </summary>
    /// <param name="fadeTime">The time that the text takes to fade completely.</param>
    /// <param name="text">The text to be faded.</param>
    /// <returns>A null IENumerator, so that the method can be ran as a couroutine.</returns>
    public IEnumerator FadeText(float fadeTime, Text text)
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

    #region Accessors and Mutators
    /// <summary>
    /// Accessors and mutators for minXSpawn
    /// </summary>
    public float MinXSpawn
    {
        get
        {
            return minXSpawn;
        }

        set
        {
            minXSpawn = value;
        }
    }

    /// <summary>
    /// Accessors and mutators for maxXSpawn
    /// </summary>
    public float MaxXSpawn
    {
        get
        {
            return maxXSpawn;
        }

        set
        {
            maxXSpawn = value;
        }
    }

    /// <summary>
    /// Accessors and mutators for waveNumber
    /// </summary>
    public int WaveNumber
    {
        get
        {
            return waveNumber;
        }

        set
        {
            waveNumber = value;
        }
    }
    #endregion
}