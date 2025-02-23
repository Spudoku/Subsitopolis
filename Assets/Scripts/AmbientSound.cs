using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSound : MonoBehaviour
{
    public GameLoop gameLoop;
    public TheGameUI gameUI;
    public HelpUI helpUI;

    public float masterVolume;
    public float cityAmbienceVolumne;


    public float crowdAmbienceVolume;

    [SerializeField] private AudioSource ambient;

    [SerializeField] private AudioSource quietAmbience;
    [SerializeField] private AudioSource additions;

    [SerializeField] private float addition_frequency;                      // 1 every addition_frequency seconds per STARTING_POPULATION on average
    [SerializeField] private float addition_pop_factor;


    [SerializeField] private List<AudioClip> additionList = new();

    // timing stuff
    private float prevAddition;
    private float nextAddition;


    void Start()
    {
        masterVolume = 1f;
        quietAmbience.Play();
        ambient.Play();

        nextAddition += Random.Range(0, addition_frequency);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameLoop.isPaused)
        {
            masterVolume = 0.5f;
        }
        else
        {
            masterVolume = 1f;

        }

        quietAmbience.volume = masterVolume;
        float ambientVolTest = Mathf.Log(gameLoop.population, 500000);
        Debug.Log($"[AmbientNoise] ambientVolTest = {ambientVolTest}");
        ambient.volume = masterVolume * Mathf.Clamp(ambientVolTest, 0, 1f) * 0.8f;
        //additions.volume = masterVolume;
        // insert some log function
        if (Time.time > nextAddition)
        {
            // play random sound
            additions.volume = masterVolume * Random.Range(0.1f, 1.25f);
            additions.PlayOneShot(additionList[Random.Range(0, additionList.Count)]);

            nextAddition = Time.time + CalcAdditionInterval();
        }
    }

    private float CalcAdditionInterval()
    {

        return ((addition_frequency * Mathf.Log(addition_pop_factor)) / Mathf.Log(gameLoop.population)) * Random.Range(0.75f, 1.25f);
    }



}
