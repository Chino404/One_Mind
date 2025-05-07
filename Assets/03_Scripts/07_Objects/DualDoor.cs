using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualDoor : MonoBehaviour, ITransparency
{
    //private Animator _animator;
    private bool _isActiveTransparency;

    [SerializeField, Tooltip("El marco de la puerta")] private Renderer _objectTransparency;
    private int _idOpacity = Shader.PropertyToID("_Opacity");

    [SerializeField, Range(0.5f, 2f), Tooltip("Fijarse en el valor de la opacidad del Shader")] private float _startValueAlpha = 2f;
    [SerializeField, Range(0, 0.9f), Tooltip("El valor final de la opacidad")]private float _endValueAlpha;

    [Header("DOOR ANIMATIONS")]
    [SerializeField] private GameObject[] _doors;
    [SerializeField] private float _animDuration=2f;
     private float _animTime=0f;
    private bool _isOpen;
    private bool _isClosing;

    [HideInInspector] public bool doorCanClose;
    public DualPressurePlate myPressurePlate;

    private void Awake()
    {
        //_animator = GetComponent<Animator>();
        myPressurePlate = transform.parent.GetComponentInChildren<DualPressurePlate>();
    }

    public void OpenTheDoor()
    {
        //OldAudioManager.instance.PlaySFX(OldAudioManager.instance.doorOpen);
        //_animator.SetTrigger("Open");
        AudioManager.instance.Play(SoundId.OpenDoor);
        _animTime = 0f;
        _isClosing = false;
        _isOpen = true;

    }

    public void CloseTheDoor()
    {
        //AudioManager.instance.Play(SoundId.OpenDoor);
        _animTime = 0f;
        _isOpen = false;
        myPressurePlate.ActionCompleted = false;
        myPressurePlate.Deactive();
        _isClosing = true;
    }

    private void Update()
    {
        if (_isOpen && _animTime < _animDuration)
        {
            _animTime += Time.deltaTime;
            float actualAngle = Mathf.Lerp(0, 80, _animTime / _animDuration);
            float otherAngle = Mathf.Lerp(0, -80, _animTime / _animDuration);
            _doors[0].transform.localRotation = Quaternion.Euler(0, actualAngle, 0);
            _doors[1].transform.localRotation = Quaternion.Euler(0, otherAngle, 0);
        }

        if (_isClosing && _animTime < _animDuration)
        {
            _animTime += Time.deltaTime;
            float actualAngle = Mathf.Lerp(90, 0, _animTime / _animDuration);
            float otherAngle = Mathf.Lerp(-90, 0, _animTime / _animDuration);
            _doors[0].transform.localRotation = Quaternion.Euler(0, actualAngle, 0);
            _doors[1].transform.localRotation = Quaternion.Euler(0, otherAngle, 0);
            
        }

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
