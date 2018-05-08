using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual
{
    #region Constants
    public const float MUTATION_MULT = 0.1f;

    public const int LIFETIME_FUNC = 0, SURVIVAL_FUNC = 1, ACCURACY_FUNC = 2, NB_ON_TARGET_FUNC = 3, ACCURATE_LONG_LIFE_FUNC = 4;
    #endregion
    #region Properties
    private DodgeAI dodgeAI;
    private Mover mover;
    private WeaponController weaponController;

    private GameObject enemyShip;

    // Genes
    private float speed, dodge, minDodgeTime, maxDodgeTime,
        minDodgeWait, maxDodgeWait, minFireRate, maxFireRate;

    private float fitness;

    // DodgeAI attribute boundaries
    private int minDodgeBound = 0, maxDodgeBound = 10;
    private float minDodgeTimeBound = 0.1f, maxDodgeTimeBound = 2.0f;
    private float minDodgeWaitBound = 0.1f, maxDodgeWaitBound = 3.0f;

    // Mover attribute boundaries
    private float minSpeedBound = -6.0f, maxSpeedBound = -2f;

    // Weapon Controller attribute boundaries
    private float minFireRateBound = 1.5f, maxFireRateBound = 4.0f;

    private float creationTime, lifetime;

    private float accuracy;
    private int nbShotsOnTarget;

    private bool survived;
    private Individual item;
    #endregion

    #region Constructors

    /// <summary>
    /// Create an individual from an enemy ship prefab.
    /// </summary>
    /// <param name="enemyShipObject">The enemy ship to create an individual from.</param>
    public Individual(GameObject enemyShipObject)
    {
        EnemyShip = enemyShipObject;
        if (EnemyShip != null)
        {
            dodgeAI = EnemyShip.GetComponent<DodgeAI>();
            mover = EnemyShip.GetComponent<Mover>();
            weaponController = EnemyShip.GetComponent<WeaponController>();

            SetRandomAttributes();
        }

        creationTime = Time.time;
    }

    /// <summary>
    /// Create an individual given another individual
    /// </summary>
    /// <param name="copy">The individual to copy.</param>
    public Individual(Individual copy)
    {
        CopyGenes(copy);
    }
#endregion

    /// <summary>
    /// Copy genes from an individual
    /// </summary>
    /// <param name="src">The individual to copy genes from.</param>
    public void CopyGenes(Individual src)
    {
        Speed = src.Speed;

        Dodge = src.Dodge;

        MinDodgeTime = src.MinDodgeTime;
        MaxDodgeTime = src.MaxDodgeTime;

        MinDodgeWait = src.MinDodgeWait;
        MaxDodgeWait = src.MaxDodgeWait;

        MinFireRate = src.MinFireRate;
        MaxFireRate = src.MaxFireRate;
    }

    /// <summary>
    /// Mark that the individual has completed the wave.
    /// </summary>
    /// <param name="survivedWave">Whether the the individual survived or not.</param>
    public void Complete(bool survivedWave)
    {
        Lifetime = Time.time - creationTime;
        Survived = survivedWave;
    }

    /// <summary>
    /// Calculate the crossover point between two values.
    /// </summary>
    /// <param name="x">The first value.</param>
    /// <param name="y">The second value.</param>
    /// <returns>The calculated crossover point between x and y.</returns>
    private float CrossoverPoint(float x, float y)
    {
        return (x + y) / 2; // basic mean
    }

    /// <summary>
    /// Perform crossover between two parents, keeping attributes within set bounds.
    /// </summary>
    /// <param name="father">The first parent.</param>
    /// <param name="mother">The second parent.</param>
    public void CrossoverAttributes(Individual father, Individual mother)
    {
        SetSpeed(System.Math.Min(CrossoverPoint(father.Speed, mother.Speed), maxSpeedBound));

        SetDodge(System.Math.Min(CrossoverPoint(father.Dodge, mother.Dodge), maxDodgeBound));

        SetDodgeTimeRange(
            CrossoverPoint(father.MinDodgeTime, mother.MinDodgeTime),
            CrossoverPoint(father.MaxDodgeTime, mother.MaxDodgeTime));

        SetDodgeWaitRange(
            CrossoverPoint(father.MinDodgeWait, mother.MinDodgeWait),
            CrossoverPoint(father.MaxDodgeWait, mother.MaxDodgeWait));

        SetFireRateRange(
            CrossoverPoint(father.MinFireRate, mother.MinFireRate),
            System.Math.Min(CrossoverPoint(father.MaxFireRate, mother.MaxFireRate), maxFireRateBound));
    }

    /// <summary>
    /// Mutate the attributes of an individual
    /// </summary>
    public void MutateAttributes()
    {
        RandomVerticalSpeed(speed - speed*MUTATION_MULT, speed + speed*MUTATION_MULT);

        RandomDodge(dodge - dodge*MUTATION_MULT, dodge + dodge*MUTATION_MULT);
        
        RandomDodgeTime(minDodgeTime - minDodgeTime * MUTATION_MULT, maxDodgeTime + maxDodgeTime * MUTATION_MULT);
        RandomDodgeWait(minDodgeWait - minDodgeWait * MUTATION_MULT, maxDodgeWait + maxDodgeWait * MUTATION_MULT);
        RandomFireRate(minFireRate - minFireRate * MUTATION_MULT, maxFireRate + maxFireRate * MUTATION_MULT);
    }

    #region Fitness Functions

    /// <summary>
    /// Calculate fitness based on lifetime.
    /// </summary>
    /// <returns>The calculated fitness score.</returns>
    public float LifetimeFunc()
    {
        return Lifetime;
    }
    
    /// <summary>
    /// Calculate fitness based on survival.
    /// </summary>
    /// <returns>The calculated fitness score.</returns>
    public float SurvivalFunc()
    {
        return System.Convert.ToInt32(Survived);
    }

    /// <summary>
    /// Calculate fitness based on accuracy.
    /// </summary>
    /// <returns>The calculated fitness score.</returns>
    public float AccuracyFunc()
    {
        return GetAccuracy();
    }

    /// <summary>
    /// Calculate fitness based on the number of shots on target.
    /// </summary>
    /// <returns>The calculated fitness score.</returns>
    public float NbOnTargetFunc()
    {
        return GetNbShotsOnTarget();
    }

    /// <summary>
    /// Get the accuracy of the individual.
    /// </summary>
    /// <returns>The accuracy value of the individual.</returns>
    public float GetAccuracy()
    {
        accuracy = weaponController.GetAccuracy();
        return accuracy;
    }

    /// <summary>
    /// Get the number of shots on target the individual made.
    /// </summary>
    /// <returns>The number of shots on target of the individual.</returns>
    public float GetNbShotsOnTarget()
    {
        nbShotsOnTarget = weaponController.GetNbShotsOnTarget();
        return nbShotsOnTarget;
    }

    /// <summary>
    /// Calculate fitness based on lifetime and accuracy.
    /// </summary>
    /// <returns>The calculated fitness score.</returns>
    public float AccurateLongLifeFunc()
    {
        return GetAccuracy() * Lifetime;
    }

    /// <summary>
    /// Calculate the fitness of the individual.
    /// </summary>
    /// <param name="selectedFunction">The index of the selected fitness function.</param>
    /// <returns>The calculated fitness score.</returns>
    public float CalculateFitness(int selectedFunction)
    {
        // Use the selected fitness function (from method parameters)

        switch (selectedFunction)
        {
            case LIFETIME_FUNC:
                Fitness = LifetimeFunc();
                break;

            case SURVIVAL_FUNC:
                Fitness =SurvivalFunc();
                break;

            case ACCURACY_FUNC:
                Fitness = AccuracyFunc();
                break;

            case NB_ON_TARGET_FUNC:
                Fitness = NbOnTargetFunc();
                break;

            case ACCURATE_LONG_LIFE_FUNC:
                Fitness = AccurateLongLifeFunc();
                break;

            // Default fitness function
            default:
                Fitness = LifetimeFunc();
                break;
        }

        return Fitness;
    }

#endregion

    #region Set Random Attributes

    /// <summary>
    /// Set attributes randomly.
    /// </summary>
    public void SetRandomAttributes()
    {
        if (EnemyShip == null)
        {
            return;
        }

        // Set random attributes:
        RandomDodge(MinDodgeBound, MaxDodgeBound);

        RandomDodgeTime(MinDodgeTimeBound, MaxDodgeTimeBound);

        RandomDodgeWait(MinDodgeWaitBound, MaxDodgeWaitBound);

        RandomVerticalSpeed(MinSpeedBound, MaxSpeedBound);

        RandomFireRate(MinFireRateBound, MaxFireRateBound);
    }

    /// <summary>
    /// Set dodge randomly within a range.
    /// </summary>
    /// <param name="minDodge">The minimum dodge value.</param>
    /// <param name="maxDodge">The maximum dodge value.</param>
    public void RandomDodge(float minDodge, float maxDodge)
    {
        SetDodge(Random.Range(minDodge, maxDodge));
    }

    /// <summary>
    /// Set dodge time randomly within a range.
    /// </summary>
    /// <param name="minDodgeTime">The minimum dodge time.</param>
    /// <param name="maxDodgeTime">The maximum dodge time.</param>
    public void RandomDodgeTime(float minDodgeTime, float maxDodgeTime)
    {
        float myMinDodgeTime = Random.Range(minDodgeTime, maxDodgeTime);
        float myMaxDodgeTime = Random.Range(myMinDodgeTime, maxDodgeTime);
        SetDodgeTimeRange(myMinDodgeTime, myMaxDodgeTime);
    }

    /// <summary>
    /// Set dodge wait randomly within a range.
    /// </summary>
    /// <param name="minDodgeWait">The minimum dodge wait.</param>
    /// <param name="maxDodgeWait">The maximum dodge wait.</param>
    public void RandomDodgeWait(float minDodgeWait, float maxDodgeWait)
    {
        float myMinDodgeWait = Random.Range(minDodgeWait, maxDodgeWait);
        float myMaxDodgeWait = Random.Range(myMinDodgeWait, maxDodgeWait);
        SetDodgeWaitRange(myMinDodgeWait, myMaxDodgeWait);
    }

    /// <summary>
    /// Set speed randomly within a range.
    /// </summary>
    /// <param name="minSpeed">The minimum speed value.</param>
    /// <param name="maxSpeed">The maximum speed value.</param>
    public void RandomVerticalSpeed(float minSpeed, float maxSpeed)
    {
        SetSpeed(Random.Range(minSpeed, maxSpeed));
    }

    /// <summary>
    /// Set fire rate randomly within a range.
    /// </summary>
    /// <param name="minFireRate">The minimum fire rate value.</param>
    /// <param name="maxFireRate">The maximum fire rate value.</param>
    public void RandomFireRate(float minFireRate, float maxFireRate)
    {
        float myMinFireRate = Random.Range(minFireRate, maxFireRate);
        float myMaxFireRate = Random.Range(myMinFireRate, maxFireRate);
        SetFireRateRange(myMinFireRate, myMaxFireRate);
    }
#endregion

    #region Accessors and Mutators

    /// <summary>
    /// Set a new speed value.
    /// </summary>
    /// <param name="s">The new speed value.</param>
    public void SetSpeed(float s)
    {
        mover.SetSpeed(s);
        Speed = s;
    }

    /// <summary>
    /// Set a new dodge value.
    /// </summary>
    /// <param name="d">The new dodge value.</param>
    public void SetDodge(float d)
    {
        dodgeAI.maxDodgeAmount = d;
        Dodge = d;
    }

    /// <summary>
    /// Set a new dodge time range.
    /// </summary>
    /// <param name="min">The new minimum dodge time.</param>
    /// <param name="max">The new maximum dodge time.</param>
    public void SetDodgeTimeRange(float min, float max)
    {
        dodgeAI.dodgeTime = new Vector2(min, max);
        MinDodgeTime = min;
        MaxDodgeTime = max;
    }

    /// <summary>
    /// Set a new dodge wait range.
    /// </summary>
    /// <param name="min">The new minimum dodge wait.</param>
    /// <param name="max">The new maximum dodge wait.</param>
    public void SetDodgeWaitRange(float min, float max)
    {
        dodgeAI.dodgeWait = new Vector2(min, max);
        MinDodgeWait = min;
        MaxDodgeWait = max;
    }

    /// <summary>
    /// Set a new fire rate range.
    /// </summary>
    /// <param name="min">The new minimum fire rate.</param>
    /// <param name="max">The new maximum fire rate.</param>
    public void SetFireRateRange(float min, float max)
    {
        weaponController.fireRate = new Vector2(min, max);
        MinFireRate = min;
        MaxFireRate = max;
    }

    #region Field Accesors and Mutators
    /// <summary>
    /// Accessor and Mutator for minDodgeBound.
    /// </summary>
    public int MinDodgeBound
    {
        get
        {
            return minDodgeBound;
        }

        set
        {
            minDodgeBound = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for maxDodgeBound.
    /// </summary>
    public int MaxDodgeBound
    {
        get
        {
            return maxDodgeBound;
        }

        set
        {
            maxDodgeBound = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for minDodgeTimeBound.
    /// </summary>
    public float MinDodgeTimeBound
    {
        get
        {
            return minDodgeTimeBound;
        }

        set
        {
            minDodgeTimeBound = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for maxDodgeTimeBound.
    /// </summary>
    public float MaxDodgeTimeBound
    {
        get
        {
            return maxDodgeTimeBound;
        }

        set
        {
            maxDodgeTimeBound = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for minDodgeWaitBound.
    /// </summary>
    public float MinDodgeWaitBound
    {
        get
        {
            return minDodgeWaitBound;
        }

        set
        {
            minDodgeWaitBound = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for maxDodgeWaitBound.
    /// </summary>
    public float MaxDodgeWaitBound
    {
        get
        {
            return maxDodgeWaitBound;
        }

        set
        {
            maxDodgeWaitBound = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for minSpeedBound.
    /// </summary>
    public float MinSpeedBound
    {
        get
        {
            return minSpeedBound;
        }

        set
        {
            minSpeedBound = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for maxSpeedBound.
    /// </summary>
    public float MaxSpeedBound
    {
        get
        {
            return maxSpeedBound;
        }

        set
        {
            maxSpeedBound = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for minFireRateBound.
    /// </summary>
    public float MinFireRateBound
    {
        get
        {
            return minFireRateBound;
        }

        set
        {
            minFireRateBound = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for maxFireRateBound.
    /// </summary>
    public float MaxFireRateBound
    {
        get
        {
            return maxFireRateBound;
        }

        set
        {
            maxFireRateBound = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for speed.
    /// </summary>
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for dodge.
    /// </summary>
    public float Dodge
    {
        get
        {
            return dodge;
        }

        set
        {
            dodge = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for minDodgeTime.
    /// </summary>
    public float MinDodgeTime
    {
        get
        {
            return minDodgeTime;
        }

        set
        {
            minDodgeTime = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for maxDodgeTime.
    /// </summary>
    public float MaxDodgeTime
    {
        get
        {
            return maxDodgeTime;
        }

        set
        {
            maxDodgeTime = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for minDodgeWait.
    /// </summary>
    public float MinDodgeWait
    {
        get
        {
            return minDodgeWait;
        }

        set
        {
            minDodgeWait = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for minDodgeWait.
    /// </summary>
    public float MaxDodgeWait
    {
        get
        {
            return maxDodgeWait;
        }

        set
        {
            maxDodgeWait = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for minFireRate.
    /// </summary>
    public float MinFireRate
    {
        get
        {
            return minFireRate;
        }

        set
        {
            minFireRate = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for maxFireRate.
    /// </summary>
    public float MaxFireRate
    {
        get
        {
            return maxFireRate;
        }

        set
        {
            maxFireRate = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for fitness.
    /// </summary>
    public float Fitness
    {
        get
        {
            return fitness;
        }

        set
        {
            fitness = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for enemyShip.
    /// </summary>
    public GameObject EnemyShip
    {
        get
        {
            return enemyShip;
        }

        set
        {
            enemyShip = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for lifetime.
    /// </summary>
    public float Lifetime
    {
        get
        {
            return lifetime;
        }

        set
        {
            lifetime = value;
        }
    }

    /// <summary>
    /// Accessor and Mutator for survived.
    /// </summary>
    public bool Survived
    {
        get
        {
            return survived;
        }

        set
        {
            survived = value;
        }
    }
    #endregion
    #endregion
}