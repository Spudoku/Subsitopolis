using UnityEngine;


// Food is measured in tons/month
public class FoodHandler : Handler
{
    public override void Initialze()
    {
        // to produce 1 ton of food, you need:
        foodFactor = 0;             // 0 million tons of food
        waterFactor = 0.0131f;         // 10000 gallons of water
        energyFactor = 0.05f;        // 0.5 megawatt-hours of energy
        maxFunding = 0.0001f;           // $100 dollars per ton of food



    }

    new public void GetDemand()
    {
        //Debug.Log("[Food handler]");

        base.GetDemand();
        //Debug.Log($"[Food-Demand] I'm contributing {endEnergyDem}mwh, {endWaterDem} millions of gallons of water, and {endFoodDem} tons of food to overall demand!");
    }

    new public void GetProduction()
    {
        base.GetProduction();
        //Debug.Log($"[Food-Production] units with energy: {unitsWithEnergy}, units with water: {unitsWithWater}, units with food: {unitsWithFood}, units with funding: {unitsWithFunding}, production: {production}");
    }

}
