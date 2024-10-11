using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Vector3 lastPosition;   // Última posición registrada de la plataforma
    private Quaternion lastRotation; // Última rotación registrada de la plataforma

    private Vector3 platformMovement; // Movimiento de la plataforma entre frames
    private Quaternion platformRotation; // Cambio de rotación de la plataforma entre frames

    void Start()
    {
        // Guardamos la posición y rotación inicial de la plataforma
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void Update()
    {
        // Calculamos cuánto se ha movido la plataforma en este frame
        platformMovement = transform.position - lastPosition;

        // Calculamos el cambio de rotación de la plataforma
        platformRotation = transform.rotation * Quaternion.Inverse(lastRotation);

        // Actualizamos la última posición y rotación
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    public Vector3 GetPlatformMovement()
    {
        // Devuelve el movimiento de la plataforma
        return platformMovement;
    }

    public Quaternion GetPlatformRotation()
    {
        // Devuelve la rotación de la plataforma
        return platformRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Characters>())
        {
            collision.gameObject.GetComponent<Characters>().currentPlatform = this;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Characters>())
        {
            collision.gameObject.GetComponent<Characters>().currentPlatform = null;
        }
    }

}
