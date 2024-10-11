using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Wind : MonoBehaviour
{
    [SerializeField, Range(0.08f, 0.2f),Tooltip("Fuerza del viento")] private float _forceWind = 0.1f;
    private bool _playerDetected;
    [SerializeField, Tooltip("Siempre activo")] private bool _alwaysActive;

    [Space(10),SerializeField, Range(0,5f),Tooltip("Tiempo activo")] private float _timeActive;
    [SerializeField, Range(0,5f),Tooltip("Tiempo desactivado")] private float _timeDeactive;

    private Collider _myCollider;
    private Characters _player;

    private void Awake()
    {
        _myCollider = GetComponent<Collider>();
        _myCollider.isTrigger = true;


        if(!_alwaysActive) _myCollider.enabled = false;
    }

    private void Start()
    {
        if(!_alwaysActive) StartCoroutine(ApplyWind());
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
            Debug.Log("Viento activado");

            yield return new WaitForSeconds(_timeActive);

            _myCollider.enabled = false;
            Debug.Log("Viento desactivado");

            yield return new WaitForSeconds(_timeDeactive);
        }
    }
}
