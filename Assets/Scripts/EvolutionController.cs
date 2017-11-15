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

    public GameObject individual;

    private List<Individual> newPopulation = new List<Individual>();
    private List<Individual> livingPopulation = new List<Individual>();
    private List<Individual> prevPopulation = new List<Individual>();
    private int chromosomeLength;

    private Individual fittestIndividual;
    private float bestFitnessScore;

    public IEnumerator CreateInitialPopulation()
    {
        Debug.Log("Creating initial population");
        generation++;

        // Get x and y cooridantes of corners of the screen, based off camera distance
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        for (int i = 0; i < populationSize; i++)
        {
            // Set the spawn position to be at a random x value along the top of the screen
            Vector3 spawnPosition = new Vector3(Random.Range(bottomCorner.x, topCorner.x), 0, 31);
            Quaternion spawnRotation = Quaternion.identity;

            livingPopulation.Add(Instantiate(individual, spawnPosition, spawnRotation).GetComponent<Individual>());

            // Wait before spawning next enemy
            yield return new WaitForSeconds(spawnWait);
        }
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
                Individual individual = population[i].GetComponent<Individual>();

                if (individual.CalculateFitness() > bestFitnessScore)
                {
                    fittestIndividual = population[i];
                    bestFitnessScore = population[i].GetComponent<Individual>().GetFitness();
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
                babies[i] = new Individual();
                babies[i] = new Individual();
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

    private IEnumerator SpawnPopulation()
    {
        // Clear the populatuion
        livingPopulation.Clear();

        // Get x and y cooridantes of corners of the screen, based off camera distance
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        for (int i = 0; i < populationSize; i++)
        {
            // Set the spawn position to be at a random x value along the top of the screen
            Vector3 spawnPosition = new Vector3(Random.Range(bottomCorner.x, topCorner.x), 0, 31);
            Quaternion spawnRotation = Quaternion.identity;

            livingPopulation.Add(Instantiate(individual, spawnPosition, spawnRotation).GetComponent<Individual>());

            // Wait before spawning next enemy
            yield return new WaitForSeconds(spawnWait);
        }
    }

    public void NextGeneration()
    {
        //UpdateFitnessScores(prevPopulation);

        CrossoverAndMutate();
        
        StartCoroutine(SpawnPopulation());

        generation++;
        Debug.Log("Generation " + generation + " spawned.");
    }
}
