using UnityEngine;

public abstract class Handler
{
    public static GameLoop gl;              // the GameLoop that this object communicates with
    public float production;                // references to how much is produced
    public float funding;                   // in $millions/month


    // how much funding is needed for optimal production
    // higher values do not improve production
    public float maxFunding;     // maximum funding based on demand

    // how much of each resource is needed for optimal production of 1 unit of this resource
    protected float foodFactor;
    protected float waterFactor;
    protected float energyFactor;

    // These values update and are accessed at the end of each tick
    public float endFoodDem;
    public float endWaterDem;
    public float endEnergyDem;

    // how many units can be produced based on food/water/energyFactor
    protected float unitsWithFood;
    protected float unitsWithWater;
    protected float unitsWithEnergy;

    protected float unitsWithFunding;



    public abstract void Initialze();
    //public abstract void Tick();

    public void GetDemand()
    {
        if (gl == null)
        {
            Debug.Log("GL is NULL!");
        }
        // calculate demand:
        // get maximum number of units possible given current funding
        // update demand accordingly
        unitsWithFunding = funding / maxFunding;
        endFoodDem = foodFactor * unitsWithFunding;
        endWaterDem = waterFactor * unitsWithFunding;
        endEnergyDem = energyFactor * unitsWithFunding;
        // Debug.Log($"[Handler-Demand] I'm contributing {endEnergyDem}mwh, {endWaterDem} millions of gallons of water, and {endFoodDem} tons of food to overall demand!");
    }

    public void GetProduction()
    {
        if (gl == null)
        {
            Debug.Log("GL is NULL!");
        }
        // calculate units with food, water, energy
        // units with energy
        if (gl.energyProduction > gl.energyDemand)
        {
            unitsWithEnergy = unitsWithFunding;
        }
        else
        {
            // calculate ratio of energy production to get
            //
            //unitsWithEnergy = energyFactor == 0f ? Mathf.Infinity : gl.energyProduction * energyRatio / energyFactor;
            if (gl.energyDemand == 0f || energyFactor == 0f || endEnergyDem == 0)
            {
                unitsWithEnergy = Mathf.Infinity;
            }
            else
            {
                unitsWithEnergy = gl.energyProduction * (endEnergyDem / gl.energyDemand) / energyFactor;
            }

        }
        // units with water
        if (gl.waterProduction > gl.waterDemand)
        {
            unitsWithWater = unitsWithFunding;
        }
        else
        {
            if (gl.waterDemand == 0f || waterFactor == 0f)
            {
                unitsWithWater = Mathf.Infinity;
            }
            else
            {
                unitsWithWater = gl.waterProduction * (endWaterDem / gl.waterDemand) / waterFactor;
            }
        }
        // units with food
        if (gl.foodProduction > gl.foodDemand)
        {
            unitsWithFood = unitsWithFunding;
        }
        else
        {
            if (gl.foodDemand == 0f || foodFactor == 0f)
            {
                unitsWithFood = Mathf.Infinity;
            }
            else
            {
                unitsWithFood = gl.foodProduction * (endFoodDem / gl.foodDemand) / foodFactor;
            }
        }

        // if production is greater than demand, then just
        //Debug.Log($"[Handler-Production] units with energy: {unitsWithEnergy}, units with water: {unitsWithWater}, units with food: {unitsWithFood}, units with funding: {unitsWithFunding}");
        production = Mathf.Min(unitsWithEnergy, unitsWithWater, unitsWithFood, unitsWithFunding);
    }

    // protected void HypotheticalUnits()
    // {
    //     if (gl != null)
    //     {
    //         if (foodFactor == 0)
    //         {
    //             unitsWithFood = Mathf.Infinity;
    //         }
    //         else
    //         {
    //             unitsWithFood = Mathf.Max((gl.foodProduction - gl.foodDemand) * foodFactor, 0); ;
    //         }

    //         if (waterFactor == 0)
    //         {
    //             unitsWithWater = Mathf.Infinity;
    //         }
    //         else
    //         {
    //             unitsWithWater = Mathf.Max((gl.waterProduction - gl.waterDemand) * waterFactor, 0);
    //         }

    //         if (energyFactor == 0)
    //         {
    //             unitsWithEnergy = Mathf.Infinity;
    //         }
    //         else
    //         {
    //             unitsWithEnergy = Mathf.Max((gl.energyProduction - gl.energyDemand) * energyFactor, 0);
    //         }



    //     }

    // }
}
