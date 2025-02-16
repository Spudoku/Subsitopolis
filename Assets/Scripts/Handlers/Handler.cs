using System.Collections.Generic;
using UnityEngine;

public abstract class Handler
{
    public static GameLoop gl;              // the GameLoop that this object communicates with
    public float production;                // references to how much is produced
    public float funding;                   // in $millions/month


    // how much funding is needed for optimal production
    protected float maxFunding;     // maximum funding based on demand

    // how much the ratio of production / demand affects production of this resource. Between 0 and 1.
    // 0 does not affect it at all, 1 has an enormous effect

    protected float foodFactor;
    protected float waterFactor;
    protected float energyFactor;

    public abstract void Initialze();
    public abstract void Tick();
}
