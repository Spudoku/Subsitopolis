using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
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
        // find ratio of production each resource gets
        float energyRatio = gl.energyDemand == 0f ? Mathf.Infinity : endEnergyDem / gl.energyDemand;
        float waterRatio = gl.waterDemand == 0f ? Mathf.Infinity : endWaterDem / gl.waterDemand;
        float foodRatio = gl.foodDemand == 0f ? Mathf.Infinity : endFoodDem / gl.foodDemand;

        unitsWithEnergy = energyFactor == 0f ? Mathf.Infinity : gl.energyProduction * energyRatio / energyFactor;
        unitsWithWater = waterFactor == 0f ? Mathf.Infinity : gl.waterProduction * waterRatio / waterFactor;
        unitsWithFood = foodFactor == 0f ? Mathf.Infinity : gl.foodProduction * foodRatio / foodFactor;
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
