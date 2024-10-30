using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualDoor : MonoBehaviour, ITransparency
{
    private Animator _animator;

    [SerializeField, Tooltip("El marco de la puerta")] private Renderer _objectTransparency;
    private int _idOpacity = Shader.PropertyToID("_Opacity");
    private float _actualValueAlpha;

    private void Awake()
    {    
        _animator = GetComponent<Animator>();
        _actualValueAlpha = 1;
    }

    public void OpenTheDoor()
    {
        //OldAudioManager.instance.PlaySFX(OldAudioManager.instance.doorOpen);
        _animator.SetTrigger("Open");
        AudioManager.instance.Play(SoundId.OpenDoor);

    }

    public void Fade(float t)
    {
        
    }

    IEnumerator InterpolateFade(float durationFade)
    {
        float elapsedTime = 0;

        while (elapsedTime < durationFade)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationFade;

            // Interpolar el valor alfa
            //float alpha = Mathf.Lerp(_iniColor.a, _valueAlpha, t);
            float alpha = Mathf.Lerp(1, 0, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            //_material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, alpha);


            yield return null;
        }

        // Asegúrate de que el color final sea el correcto
        //_material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, _valueAlpha);
    }

    public void Appear(float t)
    {
        
    }
}
