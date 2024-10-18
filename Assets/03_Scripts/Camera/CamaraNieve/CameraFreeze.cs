using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraFreeze : MonoBehaviour
{
    public Transform player; // Jugador o cualquier objeto que sigas.
    private Vector3 initialOffset; // Diferencia inicial entre c�mara y jugador.

    void Start()
    {
        // Guarda la diferencia inicial entre la c�mara y el jugador.
        initialOffset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Mant�n la c�mara siempre en la misma posici�n inicial, pero ajustada al jugador.
        transform.position = player.position + initialOffset;
        // Evita cualquier rotaci�n de la c�mara.
        transform.rotation = Quaternion.identity;
    }
}
