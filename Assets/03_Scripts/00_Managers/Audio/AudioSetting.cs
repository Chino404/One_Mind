using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSetting : MonoBehaviour
{
    public static AudioSetting instance;
    [SerializeField] private Transform _refPlayer;


    [Space(5)]public Sound[] sounds;

    //private Dictionary<SoundId, Sound> soundDict;
    private Dictionary<SoundId, List<Sound>> soundDict;

    [Space(5)] public AudioMixer mixer;
    public const string MUSIC_KEY = "MusicVolume";
    public const string SFX_KEY = "SFXVolume";
    public const string MASTER_KEY = "MasterVolume";

    private void Awake()
    {
        //soundDict = new Dictionary<SoundId, Sound>();
        soundDict = new Dictionary<SoundId, List<Sound>>();

        foreach (var item in sounds)
        {
            item.source = gameObject.AddComponent<AudioSource>();
            //soundDict[item.id] = item;

            if (!soundDict.ContainsKey(item.id)) soundDict[item.id] = new List<Sound>();

            soundDict[item.id].Add(item);

            item.source.playOnAwake = item.isPlayOnAwake;
            item.source.clip = item.clip;
            item.source.volume = 0;
            item.source.pitch = item.maxPitch;
            item.source.loop = item.isLoop;

            item.source.outputAudioMixerGroup = item.output;

            item.source.panStereo = item.target == CharacterTarget.Bongo ? -1 : 1;

            //item.posSound = transform.position;
        }
    }

    private void Start()
    {
        foreach (var sound in sounds)
        {
            _refPlayer = sound.target == CharacterTarget.Bongo ? GameManager.instance.modelBongo.transform : GameManager.instance.modelFrank.transform;

            if (sound.isPlayOnAwake)
            {
                ModifyVolume(sound);
                sound.source.pitch = Random.Range(sound.minPitch, sound.maxPitch);
                sound.source.Play();
            }
        }
    }

    private void Update()
    {
        foreach (var sound in sounds)
        {

            if (sound.isNotWithDistance)
            {
                sound.source.volume = sound.maxVolume;
                return;
            }

            if (Vector3.Distance(transform.position, _refPlayer.position) < sound.maxDistance)
            {
                ModifyVolume(sound);
            }

        }
    }

    #region VOLUME
    private void ModifyVolume(Sound sound)
    {
        if (sound.isNotWithDistance)
        {
            sound.source.volume = sound.maxVolume;
            return;
        }

        sound.currentDistance = Vector3.Distance(transform.position, _refPlayer.position);

        // Calcular el valor normalizado entre 1 (minDistance o más cerca) y 0 (maxDistance o más lejos)
        float normalizedDistance = Mathf.InverseLerp(sound.maxDistance, sound.minDistance, sound.currentDistance);

        float curvedValue = sound.volumeCurve.Evaluate(normalizedDistance);
        //auxValue = normalizedDistance;

        // Aplicar volumen proporcional (si querés)
        sound.source.volume = sound.maxVolume * normalizedDistance;
    }


    public void SetVolume(int SoundId, float vol)
    {
       Sound item = sounds[SoundId];

        if (item == null) return;

        item.source.volume = vol;
    }
#endregion

    public void Play(SoundId soundId, int index = 0)
    {
        //if (soundDict.TryGetValue(soundId, out Sound sound))
        //{
        //    if (soundId != SoundId.Theme) sound.source.pitch = Random.Range(0.8f, 1.2f);

        //    sound.source.Play();
        //}

        if (soundDict.TryGetValue(soundId, out List<Sound> soundList))
        {
            // Intentar encontrar el sonido que tenga ese indexSound específico
            //Sound sound = soundList.Find(s => s.indexSound == index);
            Sound sound = soundList[0];

            foreach (var item in soundList)
            {

                if (item.indexSound == index)
                {
                    sound = item;
                    break;
                }
                else sound = soundList[0];
            }

            if (sound != null)
            {
                if (soundId != SoundId.Theme)
                    sound.source.pitch = Random.Range(sound.minPitch, sound.maxPitch);

                sound.source.Play();
            }
            else
            {
                Debug.LogWarning($"No se encontró el sonido con ID: {soundId} y indexSound: {index}");
            }
        }

        else
        {
            Debug.LogWarning($"No se encontró el sonido con ID: <color=yellow>{soundId}</color>");
        }


    }

    public void Stop(SoundId soundId, int index = 0)
    {
        //if (soundDict.TryGetValue(SoundId, out Sound sound))
        //{
        //    sound.source.Stop();
        //}

        if (soundDict.TryGetValue(soundId, out List<Sound> soundList))
        {
            Sound sound = soundList[0];

            foreach (var item in soundList)
            {
                sound = item.indexSound == index ? item : soundList[0];
            }

            if (sound != null)
            {
                sound.source.Stop();
            }
            else
            {
                Debug.LogWarning($"No se encontró el sonido con ID: <color=yellow>{soundId}</color> y indexSound: {index}");
            }
        }
        else
        {
            Debug.LogWarning($"No se encontró el sonido con ID: <color=yellow>{soundId}</color>");
        }
    }


    private void OnDrawGizmosSelected()
    {
        if(sounds.Length < 1) return;

        foreach (var item in sounds)
        {
            if (item.isNotWithDistance) continue;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, item.maxDistance);
            //Gizmos.DrawWireSphere(item.posSound, item.maxDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, item.minDistance);
            //Gizmos.DrawWireSphere(item.posSound, item.minDistance);
        }

    }
}
