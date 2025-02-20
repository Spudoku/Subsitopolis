


using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(TheGameUI))]
public class GameLoop : MonoBehaviour
{
    // CONSTANT VALUES
    const float DEFAULT_TICK_RATE = 0.25f;        // 1 tick per 4 seconds
    const int STARTING_POPULATION = 10000;
    const float STARTING_TREASURY = 10f;

    const float STARTING_DEBT = 1f;

    const float DEFAULT_TAX_RATE = 0.1f;              // percent of income that goes to taxes
    const float DEFAULT_CITZ_INCOME = 0.0042f;                 // how many millions of dollars a citizen earns per month ($4200)

    const float DEFAULT_FOOD_MULTIPLIER = 0.114f;          // tons of food per person per month
    const float DEFAULT_WATER_MULTIPLIER = 0.003f;          // millions of gallons of water per person per month
    const float DEFAULT_ENERGY_MULTIPLIER = 0.0006f;        // megawatt-hours of energy per person per month


    const float DEFAULT_IMMIGRATION_RATE = 0.005f;          // population will grow by 0.5% per month due to immigration

    const float DEFAULT_INTEREST_RATE = 0.0042f;            // ~5% interest annually

    const float BIRTHRATE = 0.01f;                        // increase in population based on births
    const float DEATHRATE = 0.009f;                         // decrease in population based on deaths





    const float STARTING_FUNDING_AMT = 2.0f;

    // Time-related
    public float tickrate;     // speed of ticks/second, with a tick being a month in game
    public float timeScale;
    private float prevTickTime;

    public int months;

    // gameplay-related
    public int population;

    public float immigrationRate;


    // approval-related
    private float prevApproval;
    public float approval;  // value between 0 and 1

    const float DEFAULT_APPROVAL_DELTA_MULT = 0.01f;

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


        eh = new EnergyHandler();
        eh.Initialze();

        wh = new WaterHandler();
        wh.Initialze();

        // UI initialization
        ui = GetComponent<TheGameUI>();

        CalcInitFunding();
        ui.UpdateAllLabels();
        Tick();
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
        //Debug.Log($"EnergyFinding: {energyFunding}, waterFunding: {waterFunding}, foodFunding: {foodFunding}");
        months++;
        // get food, water and energy production from respective handlers

        // get demand from energy, water and food

        // get production from energy, water and food
        // fh.Tick();
        // wh.Tick();
        // eh.Tick();
        UpdateDemand();

        UpdateProduction();



        // calculate demand for each resource

        // update revenue and debt
        UpdateFinances();
        // if there isn't enough water or food, people die off and/or emmigrate


        // if there isn't enough energy, people emmigrate


        // update approval
        CalcApproval();

        // higher approval rate and surpluses of resources increases immigration

        // change population based on births and deaths
        int births = (int)(population * BIRTHRATE * Random.Range(0.75f, 1.25f));
        int deaths = (int)(population * DEATHRATE * Random.Range(0.75f, 1.25f));
        population -= deaths;
        population += births;
        //Debug.Log("Month " + months + ": births = " + births + ", deaths = " + deaths);


        // update UI
        ui.Tick();


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

        //Application.Quit();
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
        float taxRevenue = population * citizenIncome * taxRate;

        //Debug.Log("Taxes collected: " + taxRevenue);
        treasury += taxRevenue;



        totalDebt += totalDebt * debtInterestRate;


        // update funding
        eh.funding = energyFunding;
        wh.funding = waterFunding;
        fh.funding = foodFunding;
        // subtract production expenses from treasury
        SpendFromTreasury(energyFunding);
        SpendFromTreasury(waterFunding);
        SpendFromTreasury(foodFunding);

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
    }

    // void OnGUI()
    // {

    //     GUI.Label(new Rect(10, 10, 300, 20), $"Month: {months}");
    //     GUI.Label(new Rect(10, 30, 300, 40), $"Treasury: ${treasury}M");
    //     GUI.Label(new Rect(10, 50, 300, 70), $"Population: {population}");
    //     GUI.Label(new Rect(10, 70, 300, 90), $"Approval Rating: {Mathf.Round(approval * 100f)}%");
    //     GUI.Label(new Rect(10, 90, 300, 110), $"Total Debt: ${Round.RoundToPlaces(totalDebt, 2)}M");

    //     GUI.Label(new Rect(10, 300, 300, 310), $"Food production: {foodProduction}");
    //     GUI.Label(new Rect(10, 320, 300, 330), $"Food demand: {foodDemand}");
    //     GUI.Label(new Rect(10, 340, 300, 350), $"Water production: {waterProduction}");
    //     GUI.Label(new Rect(10, 360, 300, 370), $"Water demand: {waterDemand}");
    //     GUI.Label(new Rect(10, 380, 300, 390), $"Energy production: {energyProduction}");
    //     GUI.Label(new Rect(10, 400, 300, 410), $"Energy demand: {energyDemand}");
    // }


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

    private void CalcNetImmigration()
    {

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
            + Mathf.Min(population * citizenIncome * taxRate / totalDebt, 1) * DEBT_APP_WEIGHT;
        hypoApprov = Mathf.Min(hypoApprov, maxApproval);

        approval = Mathf.Lerp(prevApproval, hypoApprov, 0.5f);
        Debug.Log($"Approval is: {Round.RoundToPlaces(approval, 2)} from {Round.RoundToPlaces(prevApproval, 2)}");

    }
}
