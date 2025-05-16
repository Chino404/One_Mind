using UnityEngine;
using UnityEngine.Audio;

public enum SoundId
{
    //General
    None,
    Theme,

    ____Generic_____,

    OnlyActive,
    Always,
    OnlyDeactive,

    ____Players_____,

    Jump,
    DeathMonkey,
    Fall,
}

[System.Serializable]
public class Sound
{
    //[Header("-> Settings")]
    public CharacterTarget target;
    public AudioMixerGroup output;

    public string name;
    public AudioClip clip;

    public SoundId id;
    [Range(0, 10)]public int indexSound = 0;

    //[Header("-> Values Sliders")]
    [Range(0f, 1f)] public float maxVolume = 1f;
    public AnimationCurve volumeCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Space(10)]
    [Range(-3,3)]public float maxPitch = 1.2f;
    [Range(-3,3)]public float minPitch = 0.8f;

    [Space(10)]
    public bool isNotWithDistance;
    [Range(0,30)] public float maxDistance;
    [Range(0,20)] public float minDistance;
    public Vector3 posSound;
    [HideInInspector,Tooltip("Distancia actual entre el personaje y el sonido")]public float currentDistance;

    //[Space(10), Header("-> Bools")]
    public bool isLoop;
    public bool isPlayOnAwake;


    [HideInInspector] public AudioSource source;
    [HideInInspector] public AudioSource stereo;
}
