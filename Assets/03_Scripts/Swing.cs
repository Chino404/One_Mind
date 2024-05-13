using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public float velocidadRotacion = 30.0f; // Velocidad de rotación gradual en grados por segundo
    public float duracion; // Duración en segundos sobre la cual deseas aplicar la rotación gradual
    float _anguloRotacion;



    [SerializeField]private Quaternion rotacionOriginal;
    [SerializeField]private Quaternion rotacionFinal;
    [SerializeField]private float tiempoPasado = 0.0f;

    Quaternion _rotacionActual;

    private void Start()
    {
        // Almacenar la rotación original del objeto al inicio
        rotacionOriginal = transform.rotation;

        // Calcular la rotación final (volver a la rotación original)
        rotacionFinal = Quaternion.identity;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            // Calcular el ángulo de rotación en función del tiempo transcurrido
            _anguloRotacion = velocidadRotacion * Time.deltaTime;

            // Crear un quaternion de rotación gradual en el eje Z
            _rotacionActual = Quaternion.Euler(0, 0, _anguloRotacion);

            // Aplicar la rotación gradual al quaternion actual del objeto
            transform.rotation *= _rotacionActual;
            rotacionFinal = transform.rotation;
            tiempoPasado += Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.K))
        {
            _anguloRotacion = -velocidadRotacion * Time.deltaTime;
            _rotacionActual = Quaternion.Euler(0, 0, _anguloRotacion);
            transform.rotation *= _rotacionActual;
            rotacionFinal = transform.rotation;
            tiempoPasado += Time.deltaTime;
        }

        else
        {
            if(transform.rotation != rotacionOriginal)
            {
                // Calcular la fracción de tiempo restante
                float fraccionTiempo = tiempoPasado / duracion;

                Quaternion rotacionInterpolada = Quaternion.Lerp(rotacionFinal, rotacionOriginal, duracion);

                //Aplicar la rotación interpolada al objeto
                transform.rotation = rotacionInterpolada;
            }

        }
    }
}


