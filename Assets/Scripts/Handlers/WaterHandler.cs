using Unity.VisualScripting;
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
        energyFactor = 0.005f;        // half a megawatt hour per million gallons water
        maxFunding = 0.001f;        // $1000 per million gallons of water
    }

    new public void GetDemand()
    {
        //Debug.Log("[Water handler]");
        base.GetDemand();
        //Debug.Log($"[water-Demand] I'm contributing {endEnergyDem}mwh, {endWaterDem} millions of gallons of water, and {endFoodDem} tons of food to overall demand!");
    }

    new public void GetProduction()
    {
        base.GetProduction();
        //Debug.Log($"[Water-Production] units with energy: {unitsWithEnergy}, units with water: {unitsWithWater}, units with food: {unitsWithFood}, units with funding: {unitsWithFunding}, production: {production}");
    }


}
