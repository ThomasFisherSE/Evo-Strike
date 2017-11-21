using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionController : MonoBehaviour
{

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
        // Get x and y cooridantes of corners of the screen, based off camera distance
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        // Set the spawn position to be at a random x value along the top of the screen
        Vector3 spawnPosition = new Vector3(Random.Range(bottomCorner.x, topCorner.x), 0, 31);
        Quaternion spawnRotation = Quaternion.identity;

        // Instantiate enemy ship
        GameObject newEnemy = Instantiate(enemyShipPrefab, spawnPosition, spawnRotation);
        enemyShips.Add(newEnemy);

        // Create individual
        Individual newIndividual = new Individual(newEnemy);

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
                    bestFitnessScore = population[i].GetFitness();
                }
            }
        }
    }

    public void AddCompleteIndividual(Individual individual)
    {
        prevPopulation.Add(individual);
    }

    void Remove(Individual individual)
    {
        livingPopulation.Remove(individual);
    }

    void Mutate(List<int> bits)
    {
        // Mutate by flipping random bits
        for (int i = 0; i < bits.Count; i++)
        {
            if (UnityEngine.Random.value < mutationRate)
            {
                // Flip the 'i'th bit in bits
                bits[i] = bits[i] == 0 ? 1 : 0;
            }
        }
    }

    void Crossover(List<int> father, List<int> mother, List<int> babyF, List<int> babyM)
    {
        if (UnityEngine.Random.value > crossoverRate || father == mother)
        {
            // Just copy entire parent genome
            babyF.AddRange(father);
            babyM.AddRange(mother);
        }
        else
        {
            System.Random rnd = new System.Random();

            // Choose point at which genes come from mother instead of father
            int crossoverPoint = rnd.Next(0, chromosomeLength - 1);

            // Set new genes before crossover point
            for (int i = 0; i < crossoverPoint; i++)
            {
                babyF.Add(father[i]);
                babyM.Add(mother[i]);
            }

            // Set new genes after crossover point
            for (int i = crossoverPoint; i < mother.Count; i++)
            {
                babyF.Add(mother[i]);
                babyM.Add(father[i]);
            }
        }
    }

    private void CrossoverAndMutate()
    {
        int newEnemiesCount = 0;

        while (newEnemiesCount < populationSize)
        {
            Individual[] parents = new Individual[2];
            Individual[] babies = new Individual[2];
            
            for (int i = 0; i < babies.Length; i++)
            {
                babies[i] = new Individual(enemyShips[i]);
                babies[i] = new Individual(enemyShips[i]);
            }

            //Crossover(parents[0], parents[1], babies[0], babies[1]);
            // Mutate(babies[0]);
            // Mutate(babies[1]);

            for (int i = 0; i < 2; i++)
            {
                newPopulation.Add(babies[i]);
            }
            

            newEnemiesCount += 2;
        }
    }

    public void NextGeneration()
    {
        //UpdateFitnessScores(prevPopulation);

        CrossoverAndMutate();
        
        StartCoroutine(SpawnPopulation());
    }
}
