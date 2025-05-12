using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSetting : MonoBehaviour
{
    public static AudioSetting instance;
    [SerializeField] private Transform _refPlayer;
    public float auxValue;

    [Space(5)]public Sound[] sounds;

    private Dictionary<SoundId, Sound> soundDict;

    [Space(5),SerializeField] AudioMixer _mixer;
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

        soundDict = new Dictionary<SoundId, Sound>();

        foreach (var item in sounds)
        {
            item.source = gameObject.AddComponent<AudioSource>();
            soundDict[item.id] = item;
            item.source.playOnAwake = item.playOnAwake;

            item.source.clip = item.clip;
            item.source.volume = item.maxVolume;
            item.source.pitch = item.maxPitch;
            item.source.loop = item.loop;

            item.source.outputAudioMixerGroup = item.output;


            item.source.panStereo = item.target == CharacterTarget.Bongo ? -1 : 1;
        }
    }

    private void Start()
    {
        foreach (var item in sounds)
        {
            _refPlayer = item.target == CharacterTarget.Bongo ? GameManager.instance.modelBongo.transform : GameManager.instance.modelFrank.transform;

            if (item.playOnAwake)
            {
                item.source.pitch = Random.Range(item.minPitch, item.maxPitch);
                item.source.Play();
            }
        }
    }

    private void Update()
    {
        foreach (var sound in sounds)
        {
            if (Vector3.Distance(transform.position, _refPlayer.position) < sound.maxDistance)
            {
                ModifyVolume(sound);
            }

        }
    }

    private void ModifyVolume(Sound sound)
    {
        sound.currentDistance = Vector3.Distance(transform.position, _refPlayer.position);

        auxValue = 1- Mathf.Clamp01(sound.currentDistance / sound.maxDistance);

    }

    public void Play(SoundId soundId)
    {

        if (soundDict.TryGetValue(soundId, out Sound sound))
        {
            if (soundId != SoundId.Theme) sound.source.pitch = Random.Range(0.8f, 1.2f);

            sound.source.Play();
        }

        else
        {
            Debug.LogWarning($"No se encontró el sonido con ID: <color=yellow>{soundId}</color>");
        }
    }

    public void SetVolume(int SoundId, float vol)
    {
       Sound item = sounds[SoundId];

        if (item == null) return;

        item.source.volume = vol;
    }

    public void Stop(SoundId SoundId)
    {
        if (soundDict.TryGetValue(SoundId, out Sound sound))
        {
            sound.source.Stop();
        }
        else
        {
            Debug.LogWarning($"No se encontró el sonido con ID: <color=yellow>{SoundId}</color>");
        }
    }


    private void OnDrawGizmosSelected()
    {
        if(sounds.Length < 0) return;

        foreach (var item in sounds)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, item.maxDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, item.minDistance);
        }

    }
}
