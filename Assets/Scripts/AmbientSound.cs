using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public float masterVolume;
    public float cityAmbienceVolumne;
    public float crowdAmbienceVolume;

    [SerializeField] private AudioClip cityAmbienceLoop;
    [SerializeField] private AudioClip crowdAmbienceLoop;

    [SerializeField] private List<AudioClip> additions = new();

    // timing stuff


    void Start()
    {
        masterVolume = 1f;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
