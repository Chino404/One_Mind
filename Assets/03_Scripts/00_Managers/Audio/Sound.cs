using UnityEngine;
using UnityEngine.Audio;

public enum SoundId
{
    //General
    None,
    Theme,
    SoundLoop,

    //Monkey
    Jump,
    DeathMonkey,
    Fall,

    //Interacts
    OpenDoor,
    ButtonDualDoor,
    NormalPressurePlate,
    IronBars,
    WoodElevator,
    BreakingIce,
    Wind
}

[System.Serializable]
public class Sound
{
    [Header("-> Settings")]
    public string name;
    public AudioClip clip;
    public SoundId id;
    [Space(7)]public CharacterTarget target;
    public AudioMixerGroup output;

    [Space(10), Header("-> Values Sliders")]
    [Range(0f, 1f)] public float maxVolume = 1f;

    [Range(-3,3)]public float maxPitch = 1.2f;
    [Range(-3,3)]public float minPitch = 0.8f;

    [Space(5)]
    [Tooltip("Distancia actual entre el personaje y el sonido")]public float currentDistance;
    [Range(0,30)] public float maxDistance;
    [Range(0,20)] public float minDistance;

    [Space(10), Header("-> Bools")]
    public bool loop;
    public bool playOnAwake;


    [HideInInspector] public AudioSource source;
    [HideInInspector] public AudioSource stereo;
}
