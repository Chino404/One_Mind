using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundId
{
    //General
    None,
    Theme,

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
public class Sounds
{
    public string name;
    public SoundId id;
    public AudioClip clip;
    [Range(0f, 2f)]public float volume = 1f;
    public float pitch = 1f;
    public bool loop;
    public AudioMixerGroup output;
    [HideInInspector] public AudioSource source;
}