using UnityEngine;


// Food is measured in millions tons/month
public class FoodHandler : Handler
{


    public override void Initialze()
    {
        foodFactor = 0f;
        waterFactor = 0.9f;
        energyFactor = 0.4f;

    }

    public override void Tick()
    {
        // calculate production based on funding

        // calculate demand based on production

    }
}
