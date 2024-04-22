using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappeable : MonoBehaviour, IObserverGrappeable
{
    public Transform grappPoint;
    public float velocidadRotacion = 30.0f; // Velocidad de rotación gradual en grados por segundo
    float _anguloRotacion;

    [SerializeField] private Quaternion rotacionOriginal;
    [SerializeField] private Quaternion rotacionFinal;

    Quaternion _rotacionActual;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IObservableGrapp>() != null)
            other.gameObject.GetComponent<IObservableGrapp>().Subscribe(this);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IObservableGrapp>() != null)
            other.gameObject.GetComponent<IObservableGrapp>().Unsubscribe(this);
    }

    private void Start()
    {
        // Almacenar la rotación original del objeto al inicio
        rotacionOriginal = transform.rotation;

        // Calcular la rotación final (volver a la rotación original)
        rotacionFinal = Quaternion.identity;
    }


    public Transform ReturnPosition()
    {
        var dir = Input.GetAxisRaw("Horizontal");

        if(dir != 0)
        {
            // Calcular el ángulo de rotación en función del tiempo transcurrido
            _anguloRotacion = dir * velocidadRotacion * Time.deltaTime;

            // Crear un quaternion de rotación gradual en el eje Z
            _rotacionActual = Quaternion.Euler(0, 0, _anguloRotacion);

            // Aplicar la rotación gradual al quaternion actual del objeto
            transform.rotation *= _rotacionActual;
            rotacionFinal = transform.rotation;
        }

        else
        {

            transform.rotation = Quaternion.Lerp(rotacionFinal, rotacionOriginal, 2);
        }

        return grappPoint;
    }
}
