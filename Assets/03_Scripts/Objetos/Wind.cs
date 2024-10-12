using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(AudioSource))]
public class Wind : MonoBehaviour
{
    [SerializeField, Range(0.08f, 0.6f),Tooltip("Fuerza del viento")] private float _forceWind = 0.1f;
    private bool _playerDetected;
    [SerializeField, Tooltip("Siempre activo")] private bool _alwaysActive;

    [Space(10),SerializeField, Range(0,5f),Tooltip("Tiempo activo")] private float _timeActive;
    [SerializeField, Range(0,5f),Tooltip("Tiempo desactivado")] private float _timeDeactive;

    private Collider _myCollider;
    private AudioSource _audioSource;
    private Characters _player;

    //Provisorio hasta tener animaciones
    private Renderer _myRenderer;

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        _audioSource = GetComponent<AudioSource>();
        _myCollider = GetComponent<Collider>();

        _myCollider.isTrigger = true;

        if (!_alwaysActive)
        {
            _myCollider.enabled = false;
            _myRenderer.enabled = false;
        }
    }

    private void Start()
    {
        if (!_alwaysActive) StartCoroutine(ApplyWind());
        //else AudioManager.instance.Play(SoundId.Wind);
        else
        {
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    private void Update()
    {
       if(_playerDetected) _player.ApplyForce(_forceWind, transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Characters>())
        {
            //other.GetComponent<Characters>().ApplyForce(_forceWind, transform.forward);

            if(!_player)_player = other.GetComponent<Characters>();
            _playerDetected = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            _playerDetected = false;
        }
    }

    IEnumerator ApplyWind()
    {
        while (true)
        {
            _myCollider.enabled = true;
            _myRenderer.enabled = true;

            //AudioManager.instance.Play(SoundId.Wind);
            _audioSource.Play();
            Debug.Log("Viento activado");

            yield return new WaitForSeconds(_timeActive);

            _myCollider.enabled = false;
            _myRenderer.enabled = false;
            _playerDetected = false;

            //AudioManager.instance.Stop(SoundId.Wind);
            _audioSource.Stop();
            Debug.Log("Viento desactivado");

            yield return new WaitForSeconds(_timeDeactive);
        }
    }
}
