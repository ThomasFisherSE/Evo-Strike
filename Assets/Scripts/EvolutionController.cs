using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionController : MonoBehaviour
{
    private GameController gc;

    public int populationSize;
    private int generation = 0;
    public float mutationRate;
    public float crossoverRate;
    public float spawnWait;
    public float enemyStatsWait;

    public GameObject enemyShipPrefab;

    private List<GameObject> enemyShips = new List<GameObject>();
    private List<Individual> livingPopulation = new List<Individual>();
    private List<Individual> prevPopulation = new List<Individual>();
    private int chromosomeLength;

    private Individual fittestIndividual;
    private float bestFitnessScore;

    private int enemiesLeft;
    private bool spawningComplete = false;
    private bool waveComplete;

    public Text enemyStatsTitleText;
    public Text speedText;
    public Text fireRateText;
    public Text dodgeText;
    public Text dodgeFrequencyText;
    public Text timeAliveText;
    public Text survivedText;
    public Text accuracyText;
    public Text shotsOnTargetText;

    public GameObject statsPanel;

    private StreamWriter sw;

    /// <summary>
    /// Prepare attributes which need to be set at run-time, and write the header of the statistics file.
    /// </summary>
    public void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gc = gameControllerObject.GetComponent<GameController>();

        sw = new StreamWriter("statistics.txt");
        sw.WriteLine("------------------------------------------------------------------");
        sw.WriteLine("This file contains the statistics for the last played game.\n" +
            "To retain this data, rename or move the file.");
        sw.WriteLine("------------------------------------------------------------------\n\n");
    }

    /// <summary>
    /// Finish and close the statistics file.
    /// </summary>
    private void OnDestroy()
    {
        sw.WriteLine("--------------------------- End of Game ---------------------------");
        sw.Close();
    }

    /// <summary>
    /// Spawn the initial population of enemies.
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnInitialPopulation()
    {
        generation++;
        enemiesLeft = populationSize;
        Debug.Log("Spawning: Generation " + generation);
       
        sw.WriteLine("-------- Generation " + generation + " --------");

        // Clear the populatuion
        livingPopulation.Clear();

        for (int i = 0; i < populationSize; i++)
        {
            Individual newIndividual = NewIndividual();
            livingPopulation.Add(newIndividual);

            // Wait before spawning next enemy
            yield return new WaitForSeconds(spawnWait);
        }

        spawningComplete = true;
    }

    /// <summary>
    /// Instantiate an enemy ship and pair it with an Individual object.
    /// </summary>
    /// <returns>The newly created individual.</returns>
    private Individual NewIndividual()
    {
        // Set the spawn position to be at a random x value along the top of the screen
        Vector3 spawnPosition = new Vector3(Random.Range(gc.MinXSpawn, gc.MaxXSpawn), 0, gc.spawnValues.z);
        Quaternion spawnRotation = Quaternion.identity;

        // Instantiate enemy ship
        GameObject newEnemy = Instantiate(enemyShipPrefab, spawnPosition, spawnRotation);
        enemyShips.Add(newEnemy);

        // Create individual
        Individual newIndividual = new Individual(newEnemy);
        newIndividual.SetRandomAttributes();
        return newIndividual;
    }

    /// <summary>
    /// Update fitness scores for a population.
    /// </summary>
    /// <param name="population">The list of individuals for which fitness scores will be calculated.</param>
    void UpdateFitnessScores(List<Individual> population)
    {
        fittestIndividual = population[0];
        bestFitnessScore = 0.0f;

        for (int i = 0; i < population.Count; i++)
        {
            Individual currentIndividual = population[i];

            int selectedFitnessFunc = (int)PlayerPrefs.GetFloat("FitnessFunc");

            if (selectedFitnessFunc < 0 || selectedFitnessFunc > (Individual.NB_OF_FUNCTIONS - 1))
            {
                selectedFitnessFunc = Individual.DEFAULT_FUNC;
            }

            Debug.Log("Updating fitness scores using fitness function #" + selectedFitnessFunc);
            
            float currentFitness = currentIndividual.CalculateFitness(selectedFitnessFunc);

            if (currentFitness > bestFitnessScore)
            {
                fittestIndividual = currentIndividual;
                bestFitnessScore = currentIndividual.Fitness;
                //Debug.Log("New best fitness = " + bestFitnessScore);
            }
        }

        Debug.Log("The highest fitness score generation " + generation + " was: " + bestFitnessScore);
        
        sw.WriteLine("Highest Fitness: " + bestFitnessScore);
    }


    /// <summary>
    /// Mark that an enemy has completed the wave (whether it survived or not).
    /// Use this to check if the wave is complete (all enemies completed the wave).
    /// </summary>
    /// <param name="enemy">The enemy that has completed the wave.</param>
    /// <param name="survivedWave">Whether or not the enemy survived.</param>
    public void AddCompletedEnemy(GameObject enemy, bool survivedWave)
    {
        for (int i = 0; i < livingPopulation.Count; i++)
        {
            if (livingPopulation[i].EnemyShip.Equals(enemy))
            {
                livingPopulation[i].Complete(survivedWave);
                prevPopulation.Add(livingPopulation[i]);
                enemiesLeft--;
                //Debug.Log("[AddCompletedEnemy] " + enemiesLeft + " enemies remaining.");
                livingPopulation.Remove(livingPopulation[i]);     
            }
        }

        if (enemiesLeft == 0 && spawningComplete)
        {
            WaveComplete = true;
            //Debug.Log("[AddCompletedEnemy] Wave Complete");
        }

        //Debug.Log("[AddCompletedEnemy] Prev population size = " + prevPopulation.Count);
    }

    /// <summary>
    /// Perform crossover between a father and mother to produce two offspring.
    /// </summary>
    /// <param name="father">The first parent.</param>
    /// <param name="mother">The second parent.</param>
    /// <param name="babyF">A baby that favors the genes of its father.</param>
    /// <param name="babyM">A baby that favors the genes of its mother.</param>
    void Crossover(Individual father, Individual mother, Individual babyF, Individual babyM)
    {
        float rand = UnityEngine.Random.value;

        if (rand < crossoverRate || father == mother)
        {
            /*Debug.Log("Crossover RNG = " + rand + "(>" + crossoverRate + 
                ", so setting baby to equal parent)");*/
            // Just copy entire parent genomes
            babyF = father;
            babyM = mother;
            /*Debug.Log("babyF.Speed = " + babyF.Speed + " | father.Speed = " + father.Speed +
                " | babyM.Speed = " + babyM.Speed + " mother.Speed = " + mother.Speed);*/
        }
        else
        {
            //Debug.Log("Crossover RNG  = " + rand + "(<" + crossoverRate + ", so performing average crossover.");
            // Perform crossover between father and mother for each baby
            babyF.CrossoverAttributes(father, mother);
            babyM.CrossoverAttributes(father, mother);
        }
    }

    /// <summary>
    /// Evolve all the enemies from the previous population.
    /// </summary>
    /// <returns>A WaitForSeconds IEnumerator, allowing the coroutine to wait for some time.</returns>
    private IEnumerator EvolveEnemies()
    {
        int newlyCreatedEnemies = 0;
        spawningComplete = false;
        enemiesLeft = populationSize;

        livingPopulation.Clear();

        List<Individual> potentialParents = new List<Individual>(prevPopulation.Count);

        prevPopulation.ForEach((item) =>
        {
            potentialParents.Add(new Individual(item));
        });

        prevPopulation.Clear();

        //Debug.Log("Potential parents: " + potentialParents.Count);

        int index = 0;

        while (newlyCreatedEnemies < populationSize)
        {
            Individual[] parents = new Individual[2];
            Individual[] babies = new Individual[2];

            // Select 2 parents
            /*
            parents[0] = prevPopulation[newlyCreatedEnemies];
            parents[1] = prevPopulation[newlyCreatedEnemies + 1];
            */

            parents[0] = fittestIndividual;

            //Debug.Log("Number of potential parents: " + potentialParents.Count + " | Index = " + index);

            parents[1] = potentialParents[index];

            // Create 2 new baby individuals
            for (int j = 0; j < babies.Length; j++)
            {
                // Set the spawn position to be at a random x value along the top of the screen
                Vector3 spawnPosition = new Vector3(Random.Range(gc.MinXSpawn, gc.MaxXSpawn), 0, gc.spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;

                GameObject newEnemy = Instantiate(enemyShipPrefab, spawnPosition, spawnRotation);
                babies[j] = new Individual(newEnemy);

                newlyCreatedEnemies++;
             
                yield return new WaitForSeconds(spawnWait);
            }

            // Perform crossover of parents to create 2 babies
            Crossover(parents[0], parents[1], babies[0], babies[1]);

            /*Debug.Log("parents[0] speed = " + parents[0].Speed +
                " | parents[1] speed = " + parents[1].Speed + 
                "\nbabies[0] speed = " + babies[0].Speed +
                " | babies[1] speed = " + babies[1].Speed);*/

            for (int j = 0; j < babies.Length; j++)
            {
                babies[j].MutateAttributes();  
            }

            /*Debug.Log("Mutated babies to speeds of " + 
                babies[0].Speed + " and " + babies[1].Speed + " respectively.)");*/

            // Add the new babies to the new population
            for (int j = 0; j < 2; j++)
            {
                livingPopulation.Add(babies[j]);
            }

            index++;
        }

        spawningComplete = true;
        //Debug.Log("**** Spawning Compete, " + newlyCreatedEnemies + " enemies created. ****");

        potentialParents.Clear();
    }

    /// <summary>
    /// Start the next generation of enemies.
    /// </summary>
    public void NextGeneration()
    {
        //Debug.Log("Updating fitness scores for " + prevPopulation.Count + " enemies.");
        UpdateFitnessScores(prevPopulation);
        StartCoroutine(DisplayMostPowerfulEnemy());

        generation++;
        sw.WriteLine("-------- Generation " + generation + " --------");

        WaveComplete = false;
        StartCoroutine(EvolveEnemies());
    }
    
    /// <summary>
    /// Display the most powerful enemy panel.
    /// </summary>
    /// <returns>A WaitForSeconds IEnumerator, allowing the coroutine to wait for some time.</returns>
    public IEnumerator DisplayMostPowerfulEnemy()
    {
        statsPanel.SetActive(true);

        enemyStatsTitleText.text = "Most Powerful Enemy (Wave " + gc.WaveNumber + "):";

        speedText.text = "Speed: " + (fittestIndividual.Speed*-1).ToString();

        float avgFireRate = (fittestIndividual.MinFireRate + fittestIndividual.MaxFireRate) / 2;
        fireRateText.text = "Fire Rate: " + (1/avgFireRate).ToString();

        dodgeText.text = "Max Dodge: " + fittestIndividual.Dodge.ToString();

        float avgDodgeWait = (fittestIndividual.MinDodgeWait + fittestIndividual.MaxDodgeWait) / 2;
        dodgeFrequencyText.text = "Dodge Freq.: " + avgDodgeWait.ToString();

        timeAliveText.text = "Time Alive: " + fittestIndividual.Lifetime.ToString() + " secs";

        if (fittestIndividual.Survived)
        {
            survivedText.text = "Survived Wave?: Yes";
        } else
        {
            survivedText.text = "Survived Wave?: No";
        }

        accuracyText.text = "Accuracy: " + (fittestIndividual.GetAccuracy()*100).ToString() + "%";

        shotsOnTargetText.text = "Shots On Target: " + fittestIndividual.GetNbShotsOnTarget().ToString();

        yield return new WaitForSeconds(enemyStatsWait);
        statsPanel.SetActive(false);
    }

    /// <summary>
    /// Accessor and mutator for waveComplete attribute.
    /// </summary>
    public bool WaveComplete
    {
        get
        {
            return waveComplete;
        }

        set
        {
            waveComplete = value;
        }
    }
}
