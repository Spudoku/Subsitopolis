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

        maxFunding = 0.5f;           // $0.50 per kilowatt/hour; half a million dollars per month

    }

    public override void Tick()
    {

        // produce as much as possible given factors
        // figure out how many megawatt-hours can be produced given the resources


        // example: 100 million a month should yield 200 million megawatt hours a month
    }
}
