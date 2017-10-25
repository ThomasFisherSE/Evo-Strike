using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionController : MonoBehaviour {
    
    public int populationSize;
    public float mutationRate;
    public float crossoverRate;

    private List<Individual> population = new List<Individual>();
    private int chromosomeLength;

    private Individual fittestIndividual;
    private float bestFitnessScore;

    // Use this for initialization
    void Start () {
        // CreateInitialPopulation();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateInitialPopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Individual enemy = new Individual();
            population.Add(enemy);
        }
    }

    void UpdateFitnessScores()
    {
        fittestIndividual = population[0];
        bestFitnessScore = 0;

        for (int i = 0; i < populationSize; i++)
        {
            // Find the fittest individual
            if (population[i].CalculateFitness() > bestFitnessScore)
            {
                fittestIndividual = population[i];
                bestFitnessScore = population[i].GetFitness();
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
