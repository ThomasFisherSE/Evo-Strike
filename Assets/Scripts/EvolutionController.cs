using System.Collections;
using System.Collections.Generic;
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
    public Text maneuverabilityText;
    public Text timeAliveText;
    public Text survivedText;
    public Text accuracyText;
    public Text shotsOnTargetText;

    public GameObject statsPanel;

    public void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gc = gameControllerObject.GetComponent<GameController>();
    }

    public IEnumerator SpawnInitialPopulation()
    {
        generation++;
        enemiesLeft = populationSize;
        Debug.Log("Spawning: Generation " + generation);

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

    // Instantiate enemy ship and pair it with an Individual object.
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

    void UpdateFitnessScores(List<Individual> population)
    {
        fittestIndividual = population[0];
        bestFitnessScore = 0.0f;

        for (int i = 0; i < population.Count; i++)
        {
            Individual currentIndividual = population[i];

            float currentFitness = currentIndividual.CalculateFitness(Individual.ACCURATE_LONG_LIFE_FUNC);
            
            if (currentFitness > bestFitnessScore)
            {
                fittestIndividual = currentIndividual;
                bestFitnessScore = currentIndividual.Fitness;
                //Debug.Log("New best fitness = " + bestFitnessScore);
            }
        }

        Debug.Log("The highest fitness score of the wave was: " + bestFitnessScore);
    }

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

    public void NextGeneration()
    {
        //Debug.Log("Updating fitness scores for " + prevPopulation.Count + " enemies.");
        UpdateFitnessScores(prevPopulation);
        StartCoroutine(DisplayMostPowerfulEnemy());
        
        WaveComplete = false;
        StartCoroutine(EvolveEnemies());
    }
    
    public IEnumerator DisplayMostPowerfulEnemy()
    {
        statsPanel.SetActive(true);

        enemyStatsTitleText.text = "Most Powerful Enemy (Wave " + gc.WaveNumber + "):";

        speedText.text = "Speed: " + (fittestIndividual.Speed*-1).ToString();

        float avgFireRate = (fittestIndividual.MinFireRate + fittestIndividual.MaxFireRate) / 2;
        fireRateText.text = "Fire Rate: " + (1/avgFireRate).ToString();

        dodgeText.text = "Dodge Level: " + fittestIndividual.Dodge.ToString();

        float avgManeuverability = (fittestIndividual.MinManeuverWait + fittestIndividual.MaxManeuverWait) / 2;
        maneuverabilityText.text = "Maneuverability Level: " + avgManeuverability.ToString();

        timeAliveText.text = "Time Spent Alive: " + fittestIndividual.Lifetime.ToString() + " seconds";

        if (fittestIndividual.Survived)
        {
            survivedText.text = "Survived Wave?: Yes";
        } else
        {
            survivedText.text = "Survived Wave?: No";
        }

        accuracyText.text = "Accuracy: " + fittestIndividual.GetAccuracy().ToString() + "%";

        shotsOnTargetText.text = "Shots On Target: " + fittestIndividual.GetNbShotsOnTarget().ToString();

        yield return new WaitForSeconds(enemyStatsWait);
        statsPanel.SetActive(false);
    }

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
