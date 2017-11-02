using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionController : MonoBehaviour {
    
    public int populationSize;
    public float mutationRate;
    public float crossoverRate;

    public GameObject individual;

    private List<GameObject> population = new List<GameObject>();
    private int chromosomeLength;

    private GameObject fittestIndividual;
    private float bestFitnessScore;

    // Use this for initialization
    void Start () {
        // CreateInitialPopulation();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateInitialPopulation()
    {
        Debug.Log("Creating initial population");

        for (int i = 0; i < populationSize; i++)
        {
            // Get x and y cooridantes of corners of the screen, based off camera distance
            float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
            Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
            Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

            // Set the spawn position to be at a random x value along the top of the screen
            Vector3 spawnPosition = new Vector3(Random.Range(bottomCorner.x, topCorner.x), 0, 31);
            Quaternion spawnRotation = Quaternion.identity;

            population.Add(Instantiate(individual, spawnPosition, spawnRotation));
        }

        StartCoroutine(StartEvolution());
    }

    private IEnumerator StartEvolution()
    {
        /*
        while (true)
        {
            UpdateFitnessScores();

            Debug.Log("evolution");
        }
        */
        yield return new WaitForSeconds(1);
    }

    void UpdateFitnessScores()
    {
        fittestIndividual = population[0];
        bestFitnessScore = 0;

        for (int i = 0; i < populationSize; i++)
        {
            // Find the fittest individual
            if (population[i].GetComponent<Individual>().CalculateFitness() > bestFitnessScore)
            {
                fittestIndividual = population[i];
                bestFitnessScore = population[i].GetComponent<Individual>().GetFitness();
            }
        }
    }

    void Mutate(List<int> bits)
    {
        // Mutate by flipping random bits
        for(int i = 0; i < bits.Count; i++)
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
        } else
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
}
