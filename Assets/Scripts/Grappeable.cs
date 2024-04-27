using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grappeable : MonoBehaviour, IObserverGrappeable
{
    [SerializeField] private Transform _grappPoint;
    [SerializeField]private float _speedRotation; // Velocidad de rotación gradual en grados por segundo
    public float timerForMaxVelocity;
    private float _timerSpeed;
    public float timeToOrigin;
    private float _timerRotation;

    public float maxVelocity = 400f;
    public float minVelocity = 250f;

    private bool _enganchado;
    float _anguloRotacion;

    [SerializeField] private Quaternion rotacionOriginal;
    [SerializeField] private Quaternion rotacionFinal;

    Quaternion _rotacionActual;


    private void Start()
    {
        // Almacenar la rotación original del objeto al inicio
        rotacionOriginal = transform.rotation;

        // Calcular la rotación final (volver a la rotación original)
        rotacionFinal = Quaternion.identity;

        _speedRotation = minVelocity;

        EventManager.Subscribe("Rotate", ExecuteRotate);
        EventManager.Subscribe("StopRotate", StopRotate);

    }


    public Transform ReturnPosition()
    {
        return _grappPoint;
    }

    public void ExecuteRotate(params object[] parameters)
    {

        var dir = (float)parameters[0];

        if (dir != 0 && _enganchado)
        {
            _timerRotation = 0;
            _timerSpeed += Time.deltaTime;
            float t = Mathf.Clamp01(_timerSpeed / timerForMaxVelocity);
            if (_timerSpeed >= timerForMaxVelocity) t = timerForMaxVelocity;

            _speedRotation = Mathf.Lerp(minVelocity, maxVelocity, t);

            // Calcular el ángulo de rotación en función del tiempo transcurrido
            _anguloRotacion = dir * _speedRotation * Time.deltaTime;

            // Crear un quaternion de rotación gradual en el eje Z
            _rotacionActual = Quaternion.Euler(0, 0, _anguloRotacion);

            // Aplicar la rotación gradual al quaternion actual del objeto
            transform.rotation *= _rotacionActual;
            rotacionFinal = transform.rotation;
        }

        else
        {
            _timerSpeed = 0;
            _speedRotation = minVelocity;

            //_timerRotation += Time.deltaTime;
            //float t = Mathf.Clamp01(_timerRotation / timeToOrigin);
            //if(_timerRotation >= timeToOrigin) t = timeToOrigin;

            //transform.rotation = Quaternion.Lerp(rotacionFinal, rotacionOriginal, t);
            transform.rotation = rotacionOriginal;

        }

    }

    private void StopRotate(params object[] parameters)
    {
        _enganchado = false;
        _timerSpeed = 0;
        _speedRotation = minVelocity;
        transform.rotation = rotacionOriginal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IObservableGrapp>() != null)
        {
            other.gameObject.GetComponent<IObservableGrapp>().Subscribe(this);
            _enganchado = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IObservableGrapp>() != null)
        {
            other.gameObject.GetComponent<IObservableGrapp>().Unsubscribe(this);
            _enganchado = false;
        }
    }

}
