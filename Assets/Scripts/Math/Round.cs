using System;
using UnityEngine;

class Round
{
    public static float RoundToPlaces(float val, int places)
    {
        return (float)(Mathf.Round(val * Mathf.Pow(10f, places)) * (1f / Math.Pow(10f, places)));
    }
}