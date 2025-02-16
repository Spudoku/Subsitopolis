using UnityEngine;


// Food is measured in millions tons/month
public class FoodHandler : Handler
{


    public override void Initialze()
    {
        // to produce 1 million tons of food, you need:
        foodFactor = 0;             // 0 million tons of food
        waterFactor = 3.0f;         // 1 million gallons of water
        energyFactor = 1.0f;        // 1 megawatt-hour of energy
        maxFunding = 2f;           // 2 million dollars per ton of food



    }

    public override void Tick()
    {
        // calculate production based on funding and food, water and energy factors

        // calculate demand based on production

    }
}
