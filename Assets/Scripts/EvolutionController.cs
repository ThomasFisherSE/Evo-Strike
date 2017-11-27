using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Individual newIndividual = new Individual();
        newIndividual.SetAttributes(newEnemy);

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
        chromosomeLength = father.Count;
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

    private void individualToBits(Individual i, List<int> bits)
    {
        byte[] bytes = System.BitConverter.GetBytes(i.Mover.speed);

        bits = Enumerable.Range(0, bytes.Length / 4)
                         .Select(bit => System.BitConverter.ToInt32(bytes, bit * 4))
                         .ToList();
    }

    private void CrossoverAndMutate()
    {
        int newEnemiesCount = 0;

        while (newEnemiesCount < populationSize - 1)
        {
            Individual[] parents = new Individual[2];
            Individual[] babies = new Individual[2];

            parents[0] = livingPopulation[newEnemiesCount];
            parents[1] = livingPopulation[newEnemiesCount + 1];

            for (int i = 0; i < babies.Length; i++)
            {
                babies[i] = new Individual(enemyShips[i]);
            }

            /* 
            List<int>[] pBits = new List<int>[parents.Length];
            List<int>[] b = new List<int>[babies.Length];

            for (int i = 0; i < parents.Length; i++)
            {
                individualToBits(parents[i], pBits[i]);
                
                Debug.Log("p[" + i + "] = " + string.Join(",", pBits[i].Select(n => n.ToString()).ToArray()));
            }

            for (int i = 0; i <babies.Length; i++)
            {
                b[i] = new List<int>();
            }

            Crossover(pBits[0], pBits[1], b[0], b[1]);
            
            Debug.Log("b[0]: " + string.Join(",", b[0].Select(n => n.ToString()).ToArray()) + " b[1]: " + string.Join(",", b[1].Select(n => n.ToString()).ToArray()));

            for (int i = 0; i < babies.Length; i++)
            {
                // Mutate(babies[i]);
            }
            */

            for (int i = 0; i < 2; i++)
            {
                newPopulation.Add(babies[i]);
            }
            
            newEnemiesCount += babies.Length;
        }
    }

    public void NextGeneration()
    {
        //UpdateFitnessScores(prevPopulation);

        CrossoverAndMutate();
        
        StartCoroutine(SpawnPopulation());
    }
}
