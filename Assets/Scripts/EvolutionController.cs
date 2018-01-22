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
    private List<Individual> newPopulation = new List<Individual>();
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
            createIndividual();

            // Wait before spawning next enemy
            yield return new WaitForSeconds(spawnWait);
        }
    }

    // Instantiate enemy ship and pair it with an Individual object.
    private void createIndividual()
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
            // Find the fittest individual
            if (population[i] != null)
            {
                // Individual individual = population[i].GetComponent<Individual>();
                Individual individual = population[i];

                if (individual.CalculateFitness() > bestFitnessScore)
                {
                    fittestIndividual = population[i];
                    // bestFitnessScore = population[i].GetComponent<Individual>().GetFitness();
                    bestFitnessScore = population[i].Fitness;
                }
            }
        }
    }

    public void AddCompletedEnemy(Individual enemy)
    {
        prevPopulation.Add(enemy);
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

    private void CrossoverAndMutate()
    {
        int newlyCreatedEnemies = 0;

        while (newlyCreatedEnemies < populationSize - 1)
        {
            Individual[] parents = new Individual[2];
            Individual[] babies = new Individual[2];
            
            // Select 2 parents
            parents[0] = prevPopulation[newlyCreatedEnemies];
            parents[1] = prevPopulation[newlyCreatedEnemies + 1];

            // Create 2 new baby individuals
            for (int i = 0; i < babies.Length; i++)
            {
                // Set the spawn position to be at a random x value along the top of the screen
                Vector3 spawnPosition = new Vector3(Random.Range(gc.MinXSpawn, gc.MaxXSpawn), 0, gc.spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;

                GameObject newEnemy = Instantiate(enemyShipPrefab, spawnPosition, spawnRotation);
                babies[i] = new Individual(newEnemy);
            }

            // Perform crossover of parents to create 2 babies
            Crossover(parents[0], parents[1], babies[0], babies[1]);
            
            for (int i = 0; i < babies.Length; i++)
            {
                babies[i].MutateAttributes();
            }

            // Add the new babies to the new population
            for (int i = 0; i < 2; i++)
            {
                newPopulation.Add(babies[i]);
            }
            
            // Add the new babies to the number of new enemies
            newlyCreatedEnemies += babies.Length;
        }
    }

    public void NextGeneration()
    {
        //UpdateFitnessScores(prevPopulation);

        CrossoverAndMutate();
        
        //StartCoroutine(SpawnPopulation());
    }
}
