using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvolutionController : MonoBehaviour
{
    private GameController gc;

    public int populationSize;
    private int generation = 0;
    public float mutationRate;
    public float crossoverRate;
    public float spawnWait;

    public GameObject enemyShipPrefab;

    private List<GameObject> enemyShips = new List<GameObject>();
    private List<Individual> livingPopulation = new List<Individual>();
    private List<Individual> prevPopulation = new List<Individual>();
    private int chromosomeLength;

    private Individual fittestIndividual;
    private float bestFitnessScore;

    public void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gc = gameControllerObject.GetComponent<GameController>();
    }

    public IEnumerator SpawnPopulation()
    {
        generation++;
        Debug.Log("Spawning: Generation " + generation);

        // Clear the populatuion
        livingPopulation.Clear();

        for (int i = 0; i < populationSize; i++)
        {
            CreateIndividual();

            // Wait before spawning next enemy
            yield return new WaitForSeconds(spawnWait);
        }
    }

    // Instantiate enemy ship and pair it with an Individual object.
    private void CreateIndividual()
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

        livingPopulation.Add(newIndividual);
    }

    void UpdateFitnessScores(List<Individual> population)
    {
        fittestIndividual = population[0];
        bestFitnessScore = 0;

        for (int i = 0; i < population.Count; i++)
        {
            Individual currentIndividual = population[i];

            if (currentIndividual.CalculateFitness() > bestFitnessScore)
            {
                fittestIndividual = currentIndividual;
                bestFitnessScore = currentIndividual.Fitness;
            }
        }

        Debug.Log("Highest Fitness Score of Wave: " + bestFitnessScore);
    }

    public void AddCompletedEnemy(GameObject enemy)
    {
        for (int i = 0; i < livingPopulation.Count; i++)
        {
            if (livingPopulation[i].EnemyShip.Equals(enemy))
            {
                livingPopulation[i].Complete();
                prevPopulation.Add(livingPopulation[i]);
            }
        }

    }

    void Remove(Individual individual)
    {
        livingPopulation.Remove(individual);
    }

    void Crossover(Individual father, Individual mother, Individual babyF, Individual babyM)
    {
        if (UnityEngine.Random.value > crossoverRate || father == mother)
        {
            // Just copy entire parent genomes
            babyF = father;
            babyM = mother;
        }
        else
        {
            // Perform crossover between father and mother for each baby
            babyF.CrossoverAttributes(father, mother);
            babyM.CrossoverAttributes(father, mother);
        }
    }

    private void EvolveEnemies()
    {
        int newlyCreatedEnemies = 0;

        livingPopulation.Clear();

        while (newlyCreatedEnemies < populationSize - 1)
        {
            Individual[] parents = new Individual[2];
            Individual[] babies = new Individual[2];

            // Select 2 parents
            /*
            parents[0] = prevPopulation[newlyCreatedEnemies];
            parents[1] = prevPopulation[newlyCreatedEnemies + 1];
            */
            parents[0] = fittestIndividual;
            parents[1] = prevPopulation[newlyCreatedEnemies];

            // Create 2 new baby individuals
            for (int j = 0; j < babies.Length; j++)
            {
                // Set the spawn position to be at a random x value along the top of the screen
                Vector3 spawnPosition = new Vector3(Random.Range(gc.MinXSpawn, gc.MaxXSpawn), 0, gc.spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;

                GameObject newEnemy = Instantiate(enemyShipPrefab, spawnPosition, spawnRotation);
                babies[j] = new Individual(newEnemy);

                newlyCreatedEnemies++;
            }

            // Perform crossover of parents to create 2 babies
            Crossover(parents[0], parents[1], babies[0], babies[1]);

            for (int j = 0; j < babies.Length; j++)
            {
                babies[j].MutateAttributes();
            }

            // Add the new babies to the new population
            for (int j = 0; j < 2; j++)
            {
                livingPopulation.Add(babies[j]);
            }
        }

        Debug.Log(livingPopulation.Count + " enemies created.");

        prevPopulation.Clear();
    }

    public void NextGeneration()
    {
        UpdateFitnessScores(prevPopulation);

        EvolveEnemies();
    }
}
