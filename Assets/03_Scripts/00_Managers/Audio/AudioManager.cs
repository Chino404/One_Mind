using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public OldSounds[] sounds;

    private Dictionary<SoundId, OldSounds> soundDict;

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
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        soundDict = new Dictionary<SoundId, OldSounds>();

        foreach (var item in sounds)
        {
            item.source = gameObject.AddComponent<AudioSource>();
            soundDict[item.id] = item;
            item.source.playOnAwake = false;
            item.source.clip = item.clip;
            item.source.volume = item.volume;
            item.source.pitch = item.pitch;
            item.source.loop = item.loop;
            item.source.outputAudioMixerGroup = item.output;


        }

    }
    public void Play(SoundId soundId)
    {

        if (soundDict.TryGetValue(soundId, out OldSounds sound))
        {
            if(soundId != SoundId.Theme) sound.source.pitch = Random.Range(0.8f, 1.2f);

            sound.source.Play();
        }
        else
        {
            Debug.LogWarning($"No se encontró el sonido con ID: <color=yellow>{soundId}</color>");
        }


    }
    public void SetVolume(int SoundId, float vol)
    {
        OldSounds item = sounds[SoundId];

        if (item == null) return;

        item.source.volume = vol;
    }

    public void Stop(SoundId SoundId)
    {
        if (soundDict.TryGetValue(SoundId, out OldSounds sound))
        {
            sound.source.Stop();
        }
        else
        {
            Debug.LogWarning($"No se encontró el sonido con ID: <color=yellow>{SoundId}</color>");
        }
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

//public static class SoundId
//{
//    public const int Jump = 0;
//    public const int OpenDoor = 1;
//    public const int DesactiveWallHolograph = 2;
//    public const int Fall = 3;
//    public const int ButtonDualDoor = 4;
//    public const int IceBreak = 5;
//    public const int Wind = 6;
//    public const int DeathMonkey = 7;
//}
