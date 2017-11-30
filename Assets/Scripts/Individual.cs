using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual
{
    private EvasiveManeuver evasiveManeuver;
    private Mover mover;
    private WeaponController weaponController;

    private GameObject enemyShip;

    // Genes
    private float speed, dodge, minManeuverTime, maxManeuverTime,
        minManeuverWait, maxManeuverWait, minFireRate, maxFireRate;

    private float fitness;

    // Evasive Maneuver attribute boundaries
    private int minDodgeBound = 0, maxDodgeBound = 15;
    private float minManeuverTimeBound = 0.01f, maxManeuverTimeBound = 2.0f;
    private float minManeuverWaitBound = 0.0f, maxManeuverWaitBound = 2.0f;

    // Mover attribute boundaries
    private float minSpeedBound = -15.0f, maxSpeedBound = -3f;

    // Weapon Controller attribute boundaries
    private float minFireRateBound = 0.1f, maxFireRateBound = 2.0f;

    public Individual(GameObject enemyShipObject)
    {
        enemyShip = enemyShipObject;
        if (enemyShip != null)
        {
            evasiveManeuver = enemyShip.GetComponent<EvasiveManeuver>();
            mover = enemyShip.GetComponent<Mover>();
            weaponController = enemyShip.GetComponent<WeaponController>();

            SetRandomAttributes();
        }
    }

    private float CrossoverPoint(float x, float y)
    {
        return (x + y) / 2;
    }

    public float CalculateFitness()
    {
        // Calculate fitness here

        return Fitness;
    }

    public void CrossoverAttributes(Individual father, Individual mother)
    {
        SetSpeed(CrossoverPoint(father.Speed, mother.Speed));

        SetDodge(CrossoverPoint(father.Dodge, mother.Dodge));

        SetManeuverTimeRange(
            CrossoverPoint(father.MinManeuverTime, mother.MinManeuverTime),
            CrossoverPoint(father.MaxManeuverTime, mother.MaxManeuverTime));

        SetManeuverWaitRange(
            CrossoverPoint(father.MinManeuverWait, mother.MinManeuverWait),
            CrossoverPoint(father.MaxManeuverWait, mother.MaxManeuverWait));

        SetFireRateRange(
            CrossoverPoint(father.MinFireRate, mother.MinFireRate),
            CrossoverPoint(father.MaxFireRate, mother.MaxFireRate));
    }

    public void SetRandomAttributes()
    {
        if (enemyShip == null)
        {
            return;
        }

        // Set random attributes:
        RandomDodge(MinDodgeBound, MaxDodgeBound);

        RandomManeuverTime(MinManeuverTimeBound, MaxManeuverTimeBound);

        RandomManeuverWait(MinManeuverWaitBound, MaxManeuverWaitBound);

        RandomVerticalSpeed(MinSpeedBound, MaxSpeedBound);

        RandomFireRate(MinFireRateBound, MaxFireRateBound);
    }

    public void RandomDodge(float minDodge, float maxDodge)
    {
        SetDodge(Random.Range(minDodge, maxDodge));
    }

    public void RandomManeuverTime(float minManeuverTime, float maxManeuverTime)
    {
        float myMinManeuverTime = Random.Range(minManeuverTime, maxManeuverTime);
        float myMaxManeuverTime = Random.Range(myMinManeuverTime, maxManeuverTime);
        SetManeuverTimeRange(myMinManeuverTime, myMaxManeuverTime);
    }

    public void RandomManeuverWait(float minManeuverWait, float maxManeuverWait)
    {
        float myMinManeuverWait = Random.Range(minManeuverWait, maxManeuverWait);
        float myMaxManeuverWait = Random.Range(myMinManeuverWait, maxManeuverWait);
        SetManeuverWaitRange(myMinManeuverWait, myMaxManeuverWait);
    }

    public void RandomVerticalSpeed(float minSpeed, float maxSpeed)
    {
        SetSpeed(Random.Range(minSpeed, maxSpeed));
    }

    public void RandomFireRate(float minFireRate, float maxFireRate)
    {
        float myMinFireRate = Random.Range(minFireRate, maxFireRate);
        float myMaxFireRate = Random.Range(myMinFireRate, maxFireRate);
        SetFireRateRange(myMinFireRate, myMaxFireRate);
    }

    /** Accessors and Mutators **/

    public void SetSpeed(float s)
    {
        mover.setSpeed(s);
        Speed = s;
    }

    public void SetDodge(float d)
    {
        evasiveManeuver.dodge = d;
        Dodge = d;
    }

    public void SetManeuverTimeRange(float min, float max)
    {
        evasiveManeuver.maneuverTime = new Vector2(min, max);
        MinManeuverTime = min;
        MaxManeuverTime = max;
    }

    public void SetManeuverWaitRange(float min, float max)
    {
        evasiveManeuver.maneuverWait = new Vector2(min, max);
        MinManeuverWait = min;
        MaxManeuverWait = max;
    }

    public void SetFireRateRange(float min, float max)
    {
        weaponController.fireRate = new Vector2(min, max);
        MinFireRate = min;
        MaxFireRate = max;
    }

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

    public float MinManeuverTimeBound
    {
        get
        {
            return minManeuverTimeBound;
        }

        set
        {
            minManeuverTimeBound = value;
        }
    }

    public float MaxManeuverTimeBound
    {
        get
        {
            return maxManeuverTimeBound;
        }

        set
        {
            maxManeuverTimeBound = value;
        }
    }

    public float MinManeuverWaitBound
    {
        get
        {
            return minManeuverWaitBound;
        }

        set
        {
            minManeuverWaitBound = value;
        }
    }

    public float MaxManeuverWaitBound
    {
        get
        {
            return maxManeuverWaitBound;
        }

        set
        {
            maxManeuverWaitBound = value;
        }
    }

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

    public float MinManeuverTime
    {
        get
        {
            return minManeuverTime;
        }

        set
        {
            minManeuverTime = value;
        }
    }

    public float MaxManeuverTime
    {
        get
        {
            return maxManeuverTime;
        }

        set
        {
            maxManeuverTime = value;
        }
    }

    public float MinManeuverWait
    {
        get
        {
            return minManeuverWait;
        }

        set
        {
            minManeuverWait = value;
        }
    }

    public float MaxManeuverWait
    {
        get
        {
            return maxManeuverWait;
        }

        set
        {
            maxManeuverWait = value;
        }
    }

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
}