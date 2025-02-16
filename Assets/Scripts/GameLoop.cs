using System.Collections;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    // CONSTANT VALUES
    const float DEFAULT_TICK_RATE = 0.1429f;        // 1 tick per 7 seconds
    const int STARTING_POPULATION = 10000;

    const float DEFAULT_FOOD_MULTIPLIER = 1.0f;
    const float DEFAULT_WATER_MULTIPLIER = 1.0f;
    const float DEFAULT_ENERGY_MULTIPLIER = 1.0f;

    const float BIRTHRATE = 0.01f;                        // increase in population based on births
    const float DEATHRATE = 0.009f;                         // decrease in population based on deaths

    // Time-related
    public float tickrate;     // speed of ticks/second, with a tick being a month in game
    public float timeScale;
    private float prevTickTime;

    public int months;

    // gameplay-related
    public int population;


    public float approval;  // value between 0 and 1

    // RESOURCES
    // food
    public float foodDemand;
    private float foodDemandMult;
    public float foodProduction;

    // water
    public float waterDemand;
    private float waterDemandMult;
    public float waterProduction;

    // energy
    public float energyDemand;
    private float energyDemandMult;
    public float energyProduction;

    // FINANCIAL
    // all monetary values are in millions of dollars
    public float treasury;          // money stored
    public float taxRate;
    public float totalDebt;
    public float debtInterestRate;      // rate by how much TotalDebt increases each tick

    // Start is called when the game starts playing (i.e., as soon as MainScene is loaded)
    void Start()
    {
        // time initialization
        tickrate = DEFAULT_TICK_RATE;
        prevTickTime = Time.time;
        timeScale = 1.0f;
        months = 0;
        // game initialization
        population = STARTING_POPULATION;
        foodDemandMult = DEFAULT_FOOD_MULTIPLIER;
        waterDemandMult = DEFAULT_WATER_MULTIPLIER;
        energyDemandMult = DEFAULT_ENERGY_MULTIPLIER;

        // initialize handlers

    }

    void Update()
    {
        // goal: make ticks occur once every (1/tickrate) / time.timeScale seconds seconds
        // if timescale = 0.5, result should be 2/tickrate seconds between ticks
        // if timescale = 2, result should be 0.5/tickrate seconds between ticks
        Time.timeScale = Mathf.Clamp(timeScale, 0f, 2f);
        if (Time.time > prevTickTime + (1 / tickrate / Time.timeScale))
        {
            Tick();
            prevTickTime = Time.time;
        }

    }



    // execute a single tick in the game
    // a tick represents one game month
    private void Tick()
    {
        Debug.Log("Executing a tick at " + Time.time);
        months++;
        // get food, water and energy production from respective handlers

        //  only thing that affects food is population
        foodDemand = population * foodDemandMult;
        // calculate demand

        // update revenue and debt

        // if there isn't enough water or food, people die off and/or emmigrate

        // if there isn't enough energy, people emmigrate

        // update approval

        // higher approval rate increases immigration

    }

    private void EndGame()
    {
        // go to start scene
    }
}
