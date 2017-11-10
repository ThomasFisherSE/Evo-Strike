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

    private List<GameObject> livingPopulation = new List<GameObject>();
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

            livingPopulation.Add(Instantiate(individual, spawnPosition, spawnRotation));

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

    void Remove(GameObject individual)
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

    void Crossover(List<int> father, List<int> mother, List<int> baby)
    {
        if (UnityEngine.Random.value > crossoverRate || father == mother)
        {
            // Just copy from one of the parents
            baby.AddRange(father);
        }
        else
        {
            System.Random rnd = new System.Random();

            // Choose point at which genes come from mother instead of father
            int crossoverPoint = rnd.Next(0, chromosomeLength - 1);

            for (int i = 0; i < crossoverPoint; i++)
            {
                // Take genes from father
                baby[i] = father[i];

            }

            for (int i = crossoverPoint; i < mother.Count; i++)
            {
                // Take genes from mother
                baby[i] = mother[i];
            }
        }
    }

    private void CrossoverStage()
    {

    }

    private void MutationStage()
    {

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

            livingPopulation.Add(Instantiate(individual, spawnPosition, spawnRotation));

            // Wait before spawning next enemy
            yield return new WaitForSeconds(spawnWait);
        }
    }

    public void NextGeneration()
    {
        UpdateFitnessScores(prevPopulation);

        CrossoverStage();
        MutationStage();
        
        StartCoroutine(SpawnPopulation());

        generation++;
        Debug.Log("Generation " + generation + " spawned.");
    }
}
