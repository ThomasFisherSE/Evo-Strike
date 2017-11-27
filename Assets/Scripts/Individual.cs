using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual {
    private EvasiveManeuver evasiveManeuver;
    private Mover mover;
    private WeaponController weaponController;

    // Evasive Maneuver attribute boundaries
    private int minDodge = 0, maxDodge = 15;
    private float minManeuverTime = 0.01f, maxManeuverTime = 2.0f;
    private float minManeuverWait = 0.0f, maxManeuverWait = 2.0f;

    // Mover attribute boundaries
    private float minSpeed = -15.0f, maxSpeed = -3f;

    // Weapon Controller attribute boundaries
    private float minFireRate = 0.1f, maxFireRate = 2.0f;

    private float fitness;

    public Individual(GameObject gameObject)
    {
        SetAttributes(gameObject);
    }

    public Individual()
    {

    }

    public void SetAttributes(GameObject enemyShip)
    {
        if (enemyShip == null)
        {
            return;
        }

        EvasiveManeuver = enemyShip.GetComponent<EvasiveManeuver>();
        Mover = enemyShip.GetComponent<Mover>();
        WeaponController = enemyShip.GetComponent<WeaponController>();

        // Set random attributes:
        RandomDodge(MinDodge, MaxDodge);
        
        RandomManeuverTime(MinManeuverTime, MaxManeuverTime);
        
        RandomManeuverWait(MinManeuverWait, MaxManeuverWait);

        RandomVerticalSpeed(MinSpeed, MaxSpeed);
        
        RandomFireRate(MinFireRate, MaxFireRate);
    }

    public void RandomDodge(float minDodge, float maxDodge)
    {
        EvasiveManeuver.dodge = Random.Range(minDodge, maxDodge); ;
    }

    public void RandomManeuverTime(float minManeuverTime, float maxManeuverTime)
    {
        float myMinManeuverTime = Random.Range(minManeuverTime, maxManeuverTime);
        float myMaxManeuverTime = Random.Range(myMinManeuverTime, maxManeuverTime);
        EvasiveManeuver.maneuverTime = new Vector2(myMinManeuverTime, myMaxManeuverTime);
    }

    public void RandomManeuverWait(float minManeuverWait, float maxManeuverWait)
    {
        float myMinManeuverWait = Random.Range(minManeuverWait, maxManeuverWait);
        float myMaxManeuverWait = Random.Range(myMinManeuverWait, maxManeuverWait);
        EvasiveManeuver.maneuverWait = new Vector2(minManeuverWait, maxManeuverWait);
    }

    public void RandomVerticalSpeed(float minSpeed, float maxSpeed)
    {
        Mover.setSpeed(Random.Range(minSpeed, maxSpeed));
    }

    public void RandomFireRate(float minFireRate, float maxFireRate)
    {
        float myMinFireRate = Random.Range(minFireRate, maxFireRate);
        float myMaxFireRate = Random.Range(myMinFireRate, maxFireRate);
        WeaponController.fireRate = new Vector2(myMinFireRate, myMaxFireRate);
    }

    private void OnDestroy()
    {
        GameObject gc = GameObject.Find("Game Controller");

        if (gc != null)
        {
            EvolutionController ec = gc.GetComponent<EvolutionController>();
            ec.AddCompleteIndividual(this);
        }       
    }

    public float CalculateFitness()
    {
        // Calculate fitness here

        return Fitness;
    }

    public int MinDodge
    {
        get
        {
            return minDodge;
        }

        set
        {
            minDodge = value;
        }
    }

    public int MaxDodge
    {
        get
        {
            return maxDodge;
        }

        set
        {
            maxDodge = value;
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

    public float MinSpeed
    {
        get
        {
            return minSpeed;
        }

        set
        {
            minSpeed = value;
        }
    }

    public float MaxSpeed
    {
        get
        {
            return maxSpeed;
        }

        set
        {
            maxSpeed = value;
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

    public EvasiveManeuver EvasiveManeuver
    {
        get
        {
            return evasiveManeuver;
        }

        set
        {
            evasiveManeuver = value;
        }
    }

    public Mover Mover
    {
        get
        {
            return mover;
        }

        set
        {
            mover = value;
        }
    }

    public WeaponController WeaponController
    {
        get
        {
            return weaponController;
        }

        set
        {
            weaponController = value;
        }
    }
}
