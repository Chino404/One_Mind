using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraFreeze : MonoBehaviour
{
    public Transform player; // Jugador o cualquier objeto que sigas.
    private Vector3 initialOffset; // Diferencia inicial entre cámara y jugador.

    void Start()
    {
        // Guarda la diferencia inicial entre la cámara y el jugador.
        initialOffset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Mantén la cámara siempre en la misma posición inicial, pero ajustada al jugador.
        transform.position = player.position + initialOffset;
        // Evita cualquier rotación de la cámara.
        transform.rotation = Quaternion.identity;
    }
}
