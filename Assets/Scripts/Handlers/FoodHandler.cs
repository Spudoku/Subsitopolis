using UnityEngine;


// Food is measured in tons/month
public class FoodHandler : Handler
{
    public override void Initialze()
    {
        // to produce 1 ton of food, you need:
        foodFactor = 0;             // 0 million tons of food
        waterFactor = 0.01f;         // 10000 gallons of water
        energyFactor = 0.005f;        // 0.5 megawatt-hours of energy
        maxFunding = 0.0001f;           // $100 dollars per ton of food



    }



    // public override void Tick()
    // {
    //     float units = funding / maxFunding;
    //     HypotheticalUnits();

    //     float finalUnits = Mathf.Min(unitsWithFood, unitsWithEnergy, unitsWithWater, units);
    //     //Debug.Log($"[FoodHandler] food units: {unitsWithFood}, units with Energy: {unitsWithEnergy}, units with Water: {unitsWithWater}, funding units: {units}, finalUnits: {finalUnits}");

    //     // spend resouces!
    //     endEnergyDem = finalUnits * energyFactor;
    //     endWaterDem = finalUnits * waterFactor;
    //     endFoodDem = finalUnits * foodFactor;

    //     production = finalUnits;

    // }
}
