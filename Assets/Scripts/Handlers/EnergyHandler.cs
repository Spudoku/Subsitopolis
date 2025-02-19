using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


// energy is measured in megawatt hours/month
public class EnergyHandler : Handler
{

    public override void Initialze()
    {


        // to produce 1 megawatt hour of energy, you need:
        foodFactor = 0f;
        waterFactor = 0f;
        energyFactor = 0f;

        maxFunding = 0.05f;


    }

    new public void GetDemand()
    {
        Debug.Log("[Energy handler]");
        base.GetDemand();
        Debug.Log($"[Energy-Demand] I'm contributing {endEnergyDem}mwh, {endWaterDem} millions of gallons of water, and {endFoodDem} tons of food to overall demand!");
    }

    new public void GetProduction()
    {
        base.GetProduction();
        Debug.Log($"[Energy-Production] units with energy: {unitsWithEnergy}, units with water: {unitsWithWater}, units with food: {unitsWithFood}, units with funding: {unitsWithFunding}, production: {production}");
    }




    // public override void Tick()
    // {

    //     // produce as much as possible given factors
    //     // figure out how many megawatt-hours can be produced given the resources
    //     // step 1: estimate how many units can be produced by funding/maxFunding
    //     float units = funding / maxFunding;
    //     HypotheticalUnits();

    //     float finalUnits = Mathf.Min(unitsWithFood, unitsWithEnergy, unitsWithWater, units);
    //     //Debug.Log($"[EnergyHandler] food units: {unitsWithFood}, units with Energy: {unitsWithEnergy}, units with Water: {unitsWithWater}, funding units: {units}, finalUnits: {finalUnits}");

    //     // spend resouces!
    //     endEnergyDem = finalUnits * energyFactor;
    //     endWaterDem = finalUnits * waterFactor;
    //     endFoodDem = finalUnits * foodFactor;

    //     production = finalUnits;


    //     // example: 100 million a month should yield 200 million megawatt hours a month
    // }
}
