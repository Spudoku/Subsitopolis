using System.Collections;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    // CONSTANT VALUES
    const float DEFAULT_TICK_RATE = 0.25f;        // 1 tick per 4 seconds
    const int STARTING_POPULATION = 10000;
    const float STARTING_TREASURY = 10000000f;

    const float DEFAULT_FOOD_MULTIPLIER = 1.0f;
    const float DEFAULT_WATER_MULTIPLIER = 1.0f;
    const float DEFAULT_ENERGY_MULTIPLIER = 1.0f;

    const float IMMIGRATION_MULTIPLIER = 0.0005f;

    const float BIRTHRATE = 0.001f;                        // increase in population based on births
    const float DEATHRATE = 0.0009f;                         // decrease in population based on deaths

    // Time-related
    public float tickrate;     // speed of ticks/second, with a tick being a month in game
    public float timeScale;
    private float prevTickTime;

    public int months;

    // gameplay-related
    public int population;

    public float immigrationRate;
    public float approval;  // value between 0 and 1

    // RESOURCES
    // food
    public float foodDemand;
    private float foodDemandMult;
    public float foodProduction;
    public float netFood;

    // water
    public float waterDemand;
    private float waterDemandMult;
    public float waterProduction;

    // energy
    public float energyDemand;
    private float energyDemandMult;
    public float energyProduction;


    // Handlers
    FoodHandler fh;
    WaterHandler wh;
    EnergyHandler eh;

    // FINANCIAL
    // all monetary values are in millions of dollars
    public float treasury;          // money stored
    public float taxRate;
    public float totalDebt;
    public float debtInterestRate;      // rate by how much TotalDebt increases each tick

    public float foodFunding;
    public float waterFunding;
    public float energyFunding;

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

        treasury = STARTING_TREASURY;

        approval = 0.5f;
        // initialize handlers
        fh = new FoodHandler();
        fh.Initialze();

        eh = new EnergyHandler();
        eh.Initialze();

        wh = new WaterHandler();
        wh.Initialze();
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

        months++;
        // get food, water and energy production from respective handlers
        fh.Tick();
        wh.Tick();
        eh.Tick();

        UpdateProduction();

        //  only thing that affects food is population
        foodDemand = population * foodDemandMult;


        // calculate demand for each resource

        // update revenue and debt

        // if there isn't enough water or food, people die off and/or emmigrate


        // if there isn't enough energy, people emmigrate


        // update approval


        // higher approval rate increases immigration

        // change population based on births and deaths
        int births = 0;
        int deaths = 0;
        population -= deaths;
        population += births;
        Debug.Log("Month " + months + ": births = " + births + ", deaths = " + deaths + ", total population =  " + population);


        // losing condition: approval falls below 20% or population is less than 2000
        if (approval < 0.2f || population < STARTING_POPULATION * 0.2f)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        timeScale = 0;
        // play losing sequence


        // go to start scene
    }


    private void UpdateProduction()
    {
        foodProduction = fh.production;
        waterProduction = wh.production;
        energyProduction = eh.production;

        // calculate demand
        // each citizen eats foodDemandMult tons of food each month
        foodDemand = population * foodDemandMult;
        waterDemand = population * waterDemandMult;
        energyDemand = population * energyDemandMult;
    }
}
