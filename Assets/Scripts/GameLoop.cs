
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(TheGameUI))]
public class GameLoop : MonoBehaviour
{
    // CONSTANT VALUES
    const float DEFAULT_TICK_RATE = 0.25f;        // 1 tick per 4 seconds
    public const int STARTING_POPULATION = 100000;
    const float STARTING_TREASURY = 20f;

    const float STARTING_DEBT = 10f;

    const float DEFAULT_TAX_RATE = 0.1f;              // percent of income that goes to taxes
    const float DEFAULT_CITZ_INCOME = 0.0042f;                 // how many millions of dollars a citizen earns per month ($4200)

    const float DEFAULT_FOOD_MULTIPLIER = 0.114f;          // tons of food per person per month
    const float DEFAULT_WATER_MULTIPLIER = 0.003f;          // millions of gallons of water per person per month
    const float DEFAULT_ENERGY_MULTIPLIER = 0.0006f;        // megawatt-hours of energy per person per month


    const float DEFAULT_IMMIGRATION_RATE = 0.005f;          // population will grow by 0.5% per month due to immigration

    const float DEFAULT_INTEREST_RATE = 0.01f;            // ~5% interest annually

    const float BIRTHRATE = 0.01f;                        // increase in population based on births
    const float DEATHRATE = 0.009f;                         // decrease in population based on deaths

    const float STARVATION_CHANCE = 0.04f;                   // chance that a random citizen will starve if not provided food for the month
    const float DEHYDRATION_CHANCE = 0.06f;                     // chance that a random citizen will die of thirst if not enough water goes around
    const float EXPOSURE_CHANCE = 0.02f;                    // chance that a random citien will die of exposure if not enough power is supplied


    const float STARTING_FUNDING_AMT = 5.0f;

    // Time-related
    public float tickrate;     // speed of ticks/second, with a tick being a month in game
    public float timeScale;
    private float prevTickTime;

    public int months;

    // gameplay-related
    public int population;

    public float immigrationRate;

    public enum GameSpeed
    {
        Slow,
        Normal,
        Fast
    }

    public GameSpeed speed;
    public bool isPaused;



    // approval-related
    private float prevApproval;
    public float approval;  // value between 0 and 1

    const float TAX_APP_FUNC = 2.15f;

    // if taxes are too high or water supply too small, you should be screwed!

    const float WATER_APP_WEIGHT = 0.4f;
    const float FOOD_APP_WEIGHT = 0.30f;
    const float DEBT_APP_WEIGHT = 0.1f;
    const float ENERGY_APP_WEIGHT = 0.2f;

    const float SURPLUS_REQUIREMENT = 1.25f;

    public float citizenIncome;

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

    public float debtFunding;           // how much money goes towards paying off debt each month


    // these values will be enough to cover the starting population's needs


    // UI communication
    TheGameUI ui;

    void Start()
    {
        // time initialization
        tickrate = DEFAULT_TICK_RATE;

        timeScale = 1.0f;
        months = 0;
        // game initialization
        population = STARTING_POPULATION;
        foodDemandMult = DEFAULT_FOOD_MULTIPLIER;
        waterDemandMult = DEFAULT_WATER_MULTIPLIER;
        energyDemandMult = DEFAULT_ENERGY_MULTIPLIER;

        treasury = STARTING_TREASURY;

        approval = 0.5f;

        totalDebt = STARTING_DEBT;
        debtInterestRate = DEFAULT_INTEREST_RATE;
        taxRate = DEFAULT_TAX_RATE;
        citizenIncome = DEFAULT_CITZ_INCOME;

        waterFunding = STARTING_FUNDING_AMT;
        energyFunding = STARTING_FUNDING_AMT;
        foodFunding = STARTING_FUNDING_AMT;

        // initialize handlers
        Handler.gl = this;

        fh = new FoodHandler();
        fh.Initialze();
        fh.funding = foodFunding;


        eh = new EnergyHandler();
        eh.Initialze();
        eh.funding = energyFunding;

        wh = new WaterHandler();
        wh.Initialze();
        wh.funding = waterFunding;

        // UI initialization
        ui = GetComponent<TheGameUI>();

        CalcInitFunding();
        ui.UpdateAllLabels();
        isPaused = true;
        for (int i = 0; i < 5; i++)
        {
            TickNoLose();
        }


        speed = GameSpeed.Normal;
        InstantApproval();
    }

    void Update()
    {
        // goal: make ticks occur once every (1/tickrate) / time.timeScale seconds seconds
        // if timescale = 0.5, result should be 2/tickrate seconds between ticks
        // if timescale = 2, result should be 0.5/tickrate seconds between ticks

        if (isPaused)
        {
            timeScale = 0;
        }
        else
        {
            timeScale = speed switch
            {
                GameSpeed.Slow => 0.5f,
                GameSpeed.Fast => 2.0f,
                // should also be GameSpeed.Normal
                _ => 1.0f,
            };
        }
        Time.timeScale = Mathf.Clamp(timeScale, 0.5f, 2f);
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
        //Debug.Log($"EnergyFinding: {energyFunding}, waterFunding: {waterFunding}, foodFunding: {foodFunding}");
        months++;
        // update revenue and debt
        UpdateFinances();
        TickNoLose();


        UpdatePopulation();
        // calculate demand for each resource


        // if there isn't enough water or food, people die off and/or emmigrate


        // if there isn't enough energy, people emmigrate


        // update approval
        CalcApproval();

        // higher approval rate and surpluses of resources increases immigration

        //Debug.Log("Month " + months + ": births = " + births + ", deaths = " + deaths);
        Debug.Log($"Month: {months}, approval: {approval}; population: {population}");

        // losing condition: approval falls below 20% or population is less than 2000
        if ((approval < 0.2f || population < STARTING_POPULATION * 0.2f) && months > 5)
        {
            EndGame();
        }
    }

    private void TickNoLose()
    {
        Debug.Log("TickNoLose is being called!");
        ui.UpdatePauseButton();
        // get food, water and energy production from respective handlers

        // get demand from energy, water and food

        // get production from energy, water and food
        UpdateDemand();

        UpdateProduction();



        // update UI
        ui.Tick();
    }
    private void EndGame()
    {
        Debug.Log("End Game Triggered!");
        timeScale = 0f;
        isPaused = true;

        // play losing sequence
        ui.OnLose();
    }

    private void UpdateDemand()
    {
        eh.GetDemand();
        wh.GetDemand();
        fh.GetDemand();
        foodDemand = population * foodDemandMult + eh.endFoodDem + wh.endFoodDem + fh.endFoodDem;
        waterDemand = population * waterDemandMult + eh.endWaterDem + wh.endWaterDem + fh.endWaterDem;
        energyDemand = population * energyDemandMult + eh.endEnergyDem + wh.endEnergyDem + fh.endEnergyDem;
    }
    private void UpdateProduction()
    {
        eh.GetProduction();
        wh.GetProduction();
        fh.GetProduction();

        energyProduction = eh.production;
        waterProduction = wh.production;
        foodProduction = fh.production;

        // calculate demand
        // each citizen eats foodDemandMult tons of food each month

    }
    private void UpdateFinances()
    {
        string financialReport = $"===FINANCIAL REPORT===\n";

        float taxRevenue = population * citizenIncome * taxRate;
        financialReport += $"Taxes collected: ${taxRevenue}M;\n";
        //Debug.Log("Taxes collected: " + taxRevenue);
        treasury += taxRevenue;


        float debtIncrease = totalDebt * debtInterestRate;
        totalDebt += debtIncrease;
        financialReport += $"Debt incrased by ${debtIncrease}M this month;\n";

        // update funding
        eh.funding = energyFunding;
        wh.funding = waterFunding;
        fh.funding = foodFunding;
        // subtract production expenses from treasury
        SpendFromTreasury(energyFunding);
        SpendFromTreasury(waterFunding);
        SpendFromTreasury(foodFunding);

        financialReport += $"Total spent on funding: ${energyFunding + waterFunding + foodFunding}M;\n";
        // pay off debt
        if (debtFunding > totalDebt)
        {
            SpendFromTreasury(totalDebt);
            totalDebt = 0;

        }
        else
        {
            totalDebt -= debtFunding;
            SpendFromTreasury(debtFunding);
        }

        financialReport += "===END REPORT===";
        ui.AddNewsEvent(financialReport);
    }

    // if treasury runs out, increase totalDebt by leftover 'amount'
    private void SpendFromTreasury(float amount)
    {

        if (treasury < amount)
        {
            totalDebt += amount - treasury;
            treasury = 0;
        }
        else
        {
            treasury -= amount;
        }
    }

    // calculates and assigns funding to each resource
    private void CalcInitFunding()
    {
        energyFunding = 5.0f;
        waterFunding = 2.0f;
        foodFunding = 1.0f;
    }
    private void UpdatePopulation()
    {
        string populationReport = "===POPULATION REPORT===\n";
        // change population based on births and deaths
        int births = (int)(population * BIRTHRATE * Random.Range(0.75f, 1.25f));
        int deaths = CalcDeaths();

        populationReport += $"Births: {births};\nTotal deaths: {deaths};\n";
        population -= deaths;
        population += births;

        // calcuate how many people immigrate/emmigrate
        int immigrants = CalcNetImmigration();
        if (immigrants >= 0)
        {
            populationReport += $"Net immigration: {immigrants};\n";
        }
        else
        {
            populationReport += $"Net emmigration: {Mathf.Abs(immigrants)};\n";
        }
        population += immigrants;
        if (population < 0)
        {
            population = 0;
        }

        populationReport += "===END REPORT===";
        ui.AddNewsEvent(populationReport);
    }
    private int CalcNetImmigration()
    {
        return (int)((0.03885 * Mathf.Log(approval, Mathf.Exp(1)) + DEFAULT_IMMIGRATION_RATE * 5f) * population);
    }

    private int CalcDeaths()
    {
        // ...natural causes
        int deathCount = (int)(population * DEATHRATE * Random.Range(0.75f, 1.25f));

        int foodDeaths = 0;
        int thirstDeaths = 0;
        int exposureDeaths = 0;
        string deathReport = $"===Death Toll===\nNatural Causes: {deathCount};\n";
        // ...based on thirst
        if (waterDemand > waterProduction)
        {
            // estimate how many people die from starvation
            // units of food that are behind
            float deficit = waterDemand - waterProduction;
            // number of people affected
            float thirstaffected = deficit / DEFAULT_WATER_MULTIPLIER;
            if (thirstaffected > population)
            {
                thirstaffected = population;
            }
            thirstDeaths = (int)(thirstaffected * DEHYDRATION_CHANCE);
            Debug.Log($"[Thirst deaths] {thirstaffected} people were affected, {thirstDeaths} people died");
            if (thirstDeaths > population)
            {
                thirstDeaths = population;
            }

            if (thirstDeaths > 0)
            {
                deathReport += $"Deaths from thirst: {thirstDeaths};\n";
            }
        }
        // ...based on starvation
        if (foodDemand > foodProduction)
        {
            // estimate how many people die from starvation
            // units of food that are behind
            float deficit = foodDemand - foodProduction;
            // number of people affected
            // TODO: change this logic to be hungerAffected = decfict * (demand_from_population / total_demand)
            float hungerAffected = deficit / DEFAULT_FOOD_MULTIPLIER;
            if (hungerAffected > population - thirstDeaths)
            {
                hungerAffected = population - thirstDeaths;
            }
            foodDeaths += (int)(hungerAffected * STARVATION_CHANCE);
            Debug.Log($"[Hunger deaths] {hungerAffected} people were affected, {foodDeaths} people died");

            if (foodDeaths > population - thirstDeaths)
            {
                foodDeaths = population - thirstDeaths;
            }

            if (foodDeaths > 0)
            {
                deathReport += $"Deaths from hunger: {foodDeaths};\n";
            }
        }

        // ...based on exposure
        if (energyDemand > energyProduction)
        {
            // estimate how many people die from starvation
            // units of food that are behind
            float deficit = energyDemand - energyProduction;
            // number of people affected
            float energyAffected = deficit / DEFAULT_ENERGY_MULTIPLIER;
            if (energyAffected > population - thirstDeaths - foodDeaths)
            {
                energyAffected = population - thirstDeaths - foodDeaths;
            }
            exposureDeaths += (int)(energyAffected * EXPOSURE_CHANCE);
            Debug.Log($"[Exposure deaths] {energyAffected} people were affected, {exposureDeaths} people died");
            if (exposureDeaths > population - thirstDeaths - foodDeaths)
            {
                exposureDeaths = population - thirstDeaths - foodDeaths;
            }

            if (exposureDeaths > 0)
            {
                deathReport += $"Deaths from exposure: {exposureDeaths};\n";
            }
        }

        deathCount += foodDeaths + thirstDeaths + exposureDeaths;
        if (deathCount > population)
        {
            deathCount = population;
        }
        deathReport += "===END REPORT===";
        ui.AddNewsEvent(deathReport);
        return deathCount;
    }



    // calculate based on tax rate, debt/tax income ratio and resource production/demand ratio
    // idea: if taxes are too high, there's a maximum approval that can be reached (100% tax rate would be 0%)
    // idea: food, water and energy production/demand ratios need to be at least 2x for optimal approval
    private void CalcApproval()
    {
        prevApproval = approval;
        float maxApproval = TAX_APP_FUNC - Mathf.Pow(TAX_APP_FUNC, taxRate);
        float hypoApprov = Mathf.Min(foodProduction / foodDemand, 1) * FOOD_APP_WEIGHT
            + Mathf.Min(waterProduction / waterDemand, 1) * WATER_APP_WEIGHT
            + Mathf.Min(energyProduction / energyDemand, 1) * ENERGY_APP_WEIGHT
            + Mathf.Min(treasury / totalDebt, 1) * DEBT_APP_WEIGHT;
        hypoApprov = Mathf.Min(hypoApprov, maxApproval);

        approval = Mathf.Lerp(prevApproval, hypoApprov, 0.5f);
        Debug.Log($"Approval is: {Round.RoundToPlaces(approval, 2)} from {Round.RoundToPlaces(prevApproval, 2)}");

    }

    // CalcApproval but no lerping!
    private void InstantApproval()

    {
        prevApproval = approval;
        float maxApproval = TAX_APP_FUNC - Mathf.Pow(TAX_APP_FUNC, taxRate);
        float hypoApprov = Mathf.Min(foodProduction / foodDemand, 1) * FOOD_APP_WEIGHT
            + Mathf.Min(waterProduction / waterDemand, 1) * WATER_APP_WEIGHT
            + Mathf.Min(energyProduction / energyDemand, 1) * ENERGY_APP_WEIGHT
            + Mathf.Min(population * citizenIncome * taxRate / totalDebt, 1) * DEBT_APP_WEIGHT;
        approval = Mathf.Min(hypoApprov, maxApproval);


    }
    public void TogglePause()
    {
        isPaused = !isPaused;

    }

}

