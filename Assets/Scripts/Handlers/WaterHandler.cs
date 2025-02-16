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

    public override void Tick()
    {
        // calculate production based on funding

        // calculate demand based on production

    }
}
