using System.Collections;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    const float DEFAULT_TICK_RATE = 0.1429f;        // 1 tick per 7 seconds
    //const float SLOW_TICK_RATE = 0.0714f;
    //const float FAST_TICK_RATE = 0.2857f;
    public float tickrate;     // speed of ticks/second, with a tick being a month in game



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tickrate = DEFAULT_TICK_RATE;

    }

    void Update()
    {
        // goal: make ticks occur once every (1/tickrate) * time.timeScale seconds seconds
        // 
    }



    // execute a single tick in the game
    // a tick represents one game month
    private void tick()
    {
        Debug.Log("Executing a tick at " + Time.time);
    }
}
