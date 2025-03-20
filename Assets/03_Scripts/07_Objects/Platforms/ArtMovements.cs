using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtMovements : MonoBehaviour
{
    [SerializeField] private float fallDistance = 2f; // Distancia que baja
    [SerializeField] private float fallSpeed = 2f; // Velocidad de caída
    [SerializeField] private float returnDelay = 2f; // Tiempo antes de volver a subir
    [SerializeField] private float returnSpeed = 1f; // Tiempo antes de volver a subir

    private Vector3 initialPosition;
    private bool isFalling = false;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(FallAndReturn());
        }
    }

    private IEnumerator FallAndReturn()
    {
        isFalling = true;
        Vector3 targetPosition = initialPosition - new Vector3(0, fallDistance, 0);

        // Baja la plataforma
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(returnDelay);

        // Sube la plataforma
        while (Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        isFalling = false;
    }
}
