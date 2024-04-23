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
    private float _timmer;

    //public float velocidadTemporal;

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
    }


    public Transform ReturnPosition()
    {
        return _grappPoint;
    }

    public void ExecuteRotate(params object[] parameters)
    {
        //var dir = Input.GetAxisRaw("Horizontal");

        var dir = (float)parameters[0];

        if (dir != 0 && _enganchado)
        {
            //if (_timmer < timerForMaxVelocity)
            //{
            //    //velocidadTemporal = maxVelocity  / (timerForMaxVelocity / _timmer);
            //    //_speedRotation = velocidadTemporal;


            //    _timmer += Time.deltaTime;
            //}

            _speedRotation = Mathf.Lerp(minVelocity, maxVelocity, timerForMaxVelocity);

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
            _timmer = 0;
            _speedRotation = minVelocity;
            transform.rotation = Quaternion.Lerp(rotacionFinal, rotacionOriginal, 2);
        }

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
