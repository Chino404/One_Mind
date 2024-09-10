using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sounds[] sounds;

    [SerializeField] AudioMixer _mixer;
    public const string MUSIC_KEY = "MusicVolume";
    public const string SFX_KEY = "SFXVolume";
    public const string MASTER_KEY = "MasterVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (var item in sounds)
        {
            item.source = gameObject.AddComponent<AudioSource>();
            item.source.clip = item.clip;
            item.source.volume = item.volume;
            item.source.pitch = item.pitch;
            item.source.loop = item.loop;
            item.source.outputAudioMixerGroup = item.output;
        }

    }
    public void Play(int SoundId)
    {
        Sounds item = sounds[SoundId];

        if (item == null) return;
        item.source.Play();

    }
    public void SetVolume(int SoundId, float vol)
    {
        Sounds item = sounds[SoundId];

        if (item == null) return;

        item.source.volume = vol;
    }

    public void Stop(int SoundId)
    {
        Sounds item = sounds[SoundId];

        if (item == null || !item.source.isPlaying) return;

        item.source.Stop();
    }


    void LoadVolume()
    {
        float _musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float _sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float _masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        //_mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(_musicVolume) * 20);
       // _mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(_sfxVolume) * 20);
       // _mixer.SetFloat(VolumeSettings.MIXER_MASTER, Mathf.Log10(_masterVolume) * 20);
    }
}
public static class SoundId
{
    public const int Jump = 0;
    public const int Open_Door = 1;
    public const int Desactive_Wall_Holograph = 2;
}
