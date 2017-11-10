using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : MonoBehaviour {
    private EvasiveManeuver evasiveManeuver;
    private Mover mover;
    private WeaponController weaponController;

    // Evasive Maneuver attribute boundaries
    public int minDodge = 0, maxDodge = 15;
    public float minManeuverTime = 0.01f, maxManeuverTime = 2.0f;
    public float minManeuverWait = 0.0f, maxManeuverWait = 2.0f;

    // Mover attribute boundaries
    public float minSpeed = -20.0f, maxSpeed = -0.1f;

    // Weapon Controller attribute boundaries
    public float minFireRate = 0.1f, maxFireRate = 2.0f;

    private float fitness;
    
	void Start () {
        evasiveManeuver = GetComponent<EvasiveManeuver>();
        mover = GetComponent<Mover>();
        weaponController = GetComponent<WeaponController>();

        // Set random attributes:
        SetDodge(Random.Range(minDodge, maxDodge));
        // evasiveManeuver.dodge = Random.Range(minDodge, maxDodge);

        float myMinManeuverTime = Random.Range(minManeuverTime, maxManeuverTime);
        SetManeuverTime(myMinManeuverTime, Random.Range(myMinManeuverTime, maxManeuverTime));
        // evasiveManeuver.maneuverTime = new Vector2(myMinManeuverTime, Random.Range(myMinManeuverTime, maxManeuverTime));

        float myMinManeuverWait = Random.Range(minManeuverWait, maxManeuverWait);
        SetManeuverWait(myMinManeuverWait, Random.Range(myMinManeuverWait, maxManeuverWait));
        // evasiveManeuver.maneuverWait = new Vector2(myMinManeuverWait, Random.Range(myMinManeuverWait, maxManeuverWait));

        SetSpeed(Random.Range(minSpeed, maxSpeed));
        // mover.setSpeed(Random.Range(minSpeed, maxSpeed));

        float myMinFireRate = Random.Range(minFireRate, maxFireRate);
        float myMaxFireRate = Random.Range(myMinFireRate, maxFireRate);
        SetFireRate(myMinFireRate, myMaxFireRate);
        // weaponController.minFireRate = myMinFireRate;
        // weaponController.maxFireRate = myMaxFireRate;
	}

    public void SetDodge(float dodge)
    {
        evasiveManeuver.dodge = dodge;
    }

    public void SetManeuverTime(float minManeuverTime, float maxManeuverTime)
    {
        evasiveManeuver.maneuverTime = new Vector2(minManeuverTime, maxManeuverTime);
    }

    public void SetManeuverWait(float minManeuverWait, float maxManeuverWait)
    {
        evasiveManeuver.maneuverWait = new Vector2(minManeuverWait, maxManeuverWait);
    }

    public void SetSpeed(float speed)
    {
        mover.setSpeed(speed);
    }

    public void SetFireRate(float minFireRate, float maxFireRate)
    {
        weaponController.fireRate = new Vector2(minFireRate, maxFireRate);
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

        return fitness;
    }

    public float GetFitness()
    {
        return fitness;
    }
}
