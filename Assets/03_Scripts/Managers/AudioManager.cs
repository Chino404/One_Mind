using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("-------- Audio Source --------")]
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioSource _SFXSource;
    [SerializeField] AudioSource _monkeySFX;

    [Space(25),Header("-------- Audio Clip --------")]
    public AudioClip soundtrack;
    public AudioClip mushroom;
    public AudioClip poof;
    public AudioClip hitFrog;
    public AudioClip explosion;
    public AudioClip doorOpen;
    public AudioClip wallHoloraphActive;

    [Header("Monkey Audio")]
    public AudioClip hitMonkey;
    [Tooltip("Sonido al mover el bastón")]public AudioClip swoosh;
    public AudioClip attackSpin;
    public AudioClip jump;
    [Tooltip("Caer al vacío")]public AudioClip fallIntoTheVoid;


    private void Awake()
    {
        if(!instance) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _musicSource.clip = soundtrack;
        _musicSource.Play();
    }

    public void StopAll()
    {
        _SFXSource?.Stop();
        _monkeySFX?.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        _SFXSource?.PlayOneShot(clip);
    }
    public void StopSFX()
    {
        _SFXSource?.Stop();
    }

    public void PlayMonkeySFX(AudioClip clip) => _monkeySFX?.PlayOneShot(clip);

    public void StopMonkeySFX() => _monkeySFX?.Stop();


    public bool ExecuteClipMonkey(AudioClip clip)
    {
        var audio = _monkeySFX;
        audio.clip = clip;

        return audio.isPlaying;
    }
}
