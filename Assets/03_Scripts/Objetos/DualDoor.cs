using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualDoor : MonoBehaviour, ITransparency
{
    private Animator _animator;
    private bool _isActiveTransparency;

    [SerializeField, Tooltip("El marco de la puerta")] private Renderer _objectTransparency;
    private int _idOpacity = Shader.PropertyToID("_Opacity");

    [SerializeField, Range(0.5f, 2f), Tooltip("Fijarse en el valor de la opacidad del Shader")] private float _startValueAlpha = 2f;
    [SerializeField, Range(0, 0.9f), Tooltip("El valor final de la opacidad")]private float _endValueAlpha;

    private void Awake()
    {    
        _animator = GetComponent<Animator>();
    }

    public void OpenTheDoor()
    {
        //OldAudioManager.instance.PlaySFX(OldAudioManager.instance.doorOpen);
        _animator.SetTrigger("Open");
        AudioManager.instance.Play(SoundId.OpenDoor);

    }

    public void Fade(float t)
    {
        if(!_isActiveTransparency && _objectTransparency)
        {
            _isActiveTransparency = true;

            StartCoroutine(InterpolateFadeShader(t));

            //Debug.Log("Fade");
        }
    }

    IEnumerator InterpolateFadeShader(float durationFade)
    {
        float elapsedTime = 0;

        while (elapsedTime < durationFade)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationFade;

            // Interpolar el valor alfa
            float alpha = Mathf.Lerp(1, _endValueAlpha, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            _objectTransparency.material.SetFloat(_idOpacity, alpha);


            yield return null;
        }

        // Asegúrate de que el color final sea el correcto

        _objectTransparency.material.SetFloat(_idOpacity, _endValueAlpha);
    }

    public void Appear(float t)
    {
        if (_isActiveTransparency && _objectTransparency)
        {
            _isActiveTransparency = false;

            StartCoroutine(InterpolateVisibleShader(t));
            //Debug.Log("Appear");
        }
    }

    IEnumerator InterpolateVisibleShader(float durationFade)
    {
        float elapsedTime = 0;

        while (elapsedTime < durationFade)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationFade;

            // Interpolar el valor alfa
            float alpha = Mathf.Lerp(_endValueAlpha, _startValueAlpha, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            _objectTransparency.material.SetFloat(_idOpacity, alpha);


            yield return null;
        }

        // Asegúrate de que el color final sea el correcto
        _objectTransparency.material.SetFloat(_idOpacity, _startValueAlpha);
    }
}
