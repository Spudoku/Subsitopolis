using System.Collections.Generic;
using UnityEngine;

public abstract class Handler
{
    public static GameLoop gl;              // the GameLoop that this object communicates with
    public float production;                // references to how much is produced
    public List<int> inputs;                // numbers that affect demand of <blank> 

    public abstract void Tick();
}
