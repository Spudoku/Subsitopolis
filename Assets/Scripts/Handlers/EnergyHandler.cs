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

        maxFunding = 0.005f;


    }

    new public void GetDemand()
    {
        //Debug.Log("[Energy handler]");
        base.GetDemand();
        //Debug.Log($"[Energy-Demand] I'm contributing {endEnergyDem}mwh, {endWaterDem} millions of gallons of water, and {endFoodDem} tons of food to overall demand!");
    }

    new public void GetProduction()
    {
        base.GetProduction();
        //Debug.Log($"[Energy-Production] units with energy: {unitsWithEnergy}, units with water: {unitsWithWater}, units with food: {unitsWithFood}, units with funding: {unitsWithFunding}, production: {production}");
    }


}
