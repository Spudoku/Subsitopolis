using UnityEngine;


// Water is measured in millions of gallons/month
public class WaterHandler : Handler
{
    public override void Initialze()
    {
        foodFactor = 0f;
        waterFactor = 0.0f;
        energyFactor = 0.5f;
    }

    public override void Tick()
    {
        // calculate production based on funding

        // calculate demand based on production

    }
}
