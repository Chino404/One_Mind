using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraControllerMenu : MonoBehaviour
{
    public Transform[] targetPositions;
    public float moveSpeed = 5f;
    int currentTargetIndex = 0;

    void Update()
    {
        // Mover la cámara hacia el objetivo actual
        if (targetPositions.Length > 0)
        {
            var target = targetPositions[currentTargetIndex];
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, moveSpeed * Time.deltaTime);

            // Verificar si se presiona la tecla Shift
            if (Input.GetKeyDown(KeyCode.Q))
            {

                currentTargetIndex = (currentTargetIndex + 1) % targetPositions.Length;
                currentTargetIndex = (currentTargetIndex + 1) % targetPositions.Length;
            }
        }
    }
}