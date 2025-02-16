using System.Collections.Generic;
using UnityEngine;

public abstract class Handler
{
    public static GameLoop gl;              // the GameLoop that this object communicates with
    public float production;                // references to how much is produced
    public float funding;                   // in $millions/month


    // how much funding is needed for optimal production
    // higher values do not improve production
    protected float maxFunding;     // maximum funding based on demand

    // how much of each resource is needed for optimal production of 1 unit of this resource
    protected float foodFactor;
    protected float waterFactor;
    protected float energyFactor;

    public abstract void Initialze();
    public abstract void Tick();
}
