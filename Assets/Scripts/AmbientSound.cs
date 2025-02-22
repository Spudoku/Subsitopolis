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
    [SerializeField] private AudioSource additions;
    [SerializeField] private AudioSource crowds;


    private float additionFrequency;

    [SerializeField] private AudioClip cityAmbienceLoop;
    [SerializeField] private AudioClip crowdAmbienceLoop;

    [SerializeField] private List<AudioClip> additionList = new();

    // timing stuff


    void Start()
    {
        masterVolume = 1f;

    }

    // Update is called once per frame
    void Update()
    {

    }

    // fades a given clip in and out
    private IEnumerator PlayAmbientClip(AudioClip clip)
    {
        yield return null;
    }

    private void CalculateAdditionFrequency()
    {


    }


    // update the volume based on whether the game is paused or the help menu is active
    public void UpdateVolume()
    {

    }

}
