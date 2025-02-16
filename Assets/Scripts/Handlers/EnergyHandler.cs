using UnityEngine;


// energy is measured in megawatt hours/month
public class EnergyHandler : Handler
{
    public override void Initialze()
    {
        foodFactor = 0f;
        waterFactor = 0.2f;
        energyFactor = 0.0f;
    }

    public override void Tick()
    {
        // calculate production based on funding

        // calculate demand based on production

    }
}
