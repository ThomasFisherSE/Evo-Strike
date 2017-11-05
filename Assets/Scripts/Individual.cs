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
        evasiveManeuver.dodge = Random.Range(minDodge, maxDodge);

        float myMinManeuverTime = Random.Range(minManeuverTime, maxManeuverTime);
        evasiveManeuver.maneuverTime = new Vector2(myMinManeuverTime, Random.Range(myMinManeuverTime, maxManeuverTime));

        float myMinManeuverWait = Random.Range(minManeuverWait, maxManeuverWait);
        evasiveManeuver.maneuverWait = new Vector2(myMinManeuverWait, Random.Range(myMinManeuverWait, maxManeuverWait));

        mover.speed = Random.Range(minSpeed, maxSpeed);

        float myMinFireRate = Random.Range(minFireRate, maxFireRate);
        float myMaxFireRate = Random.Range(myMinFireRate, maxFireRate);
        weaponController.minFireRate = myMinFireRate;
        weaponController.maxFireRate = myMaxFireRate;
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
