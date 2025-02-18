using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Handler
{
    public static GameLoop gl;              // the GameLoop that this object communicates with
    public float production;                // references to how much is produced
    public float funding;                   // in $millions/month


    // how much funding is needed for optimal production
    // higher values do not improve production
    protected float maxFunding;     // maximum funding based on demand

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



    public abstract void Initialze();
    public abstract void Tick();

    public abstract float GetDemand();
    public abstract float GetProduction();

    protected void HypotheticalUnits()
    {
        if (gl != null)
        {
            if (foodFactor == 0)
            {
                unitsWithFood = Mathf.Infinity;
            }
            else
            {
                unitsWithFood = Mathf.Max((gl.foodProduction - gl.foodDemand) * foodFactor, 0); ;
            }

            if (waterFactor == 0)
            {
                unitsWithWater = Mathf.Infinity;
            }
            else
            {
                unitsWithWater = Mathf.Max((gl.waterProduction - gl.waterDemand) * waterFactor, 0);
            }

            if (energyFactor == 0)
            {
                unitsWithEnergy = Mathf.Infinity;
            }
            else
            {
                unitsWithEnergy = Mathf.Max((gl.energyProduction - gl.energyDemand) * energyFactor, 0);
            }



        }

    }
}
