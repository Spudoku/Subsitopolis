using UnityEngine;


// Water is measured in millions of gallons/month
// water represents drinking water
public class WaterHandler : Handler
{
    public override void Initialze()
    {
        // to produce 1 mil;
        foodFactor = 0f;
        waterFactor = 0.0f;
        energyFactor = 0.5f;        // half a megawatt hour per million gallons water
        maxFunding = 0.001f;        // $1000 per million gallons of water
    }
    public override float GetDemand()
    {
        throw new System.NotImplementedException();
    }

    public override float GetProduction()
    {
        throw new System.NotImplementedException();
    }



    public override void Tick()
    {
        float units = funding / maxFunding;
        HypotheticalUnits();

        float finalUnits = Mathf.Min(unitsWithFood, unitsWithEnergy, unitsWithWater, units);
        //Debug.Log($"[WaterHandler] food units: {unitsWithFood}, units with Energy: {unitsWithEnergy}, units with Water: {unitsWithWater}, funding units: {units}, finalUnits: {finalUnits}");

        // spend resouces!
        endEnergyDem = finalUnits * energyFactor;
        endWaterDem = finalUnits * waterFactor;
        endFoodDem = finalUnits * foodFactor;

        production = finalUnits;

    }
}
