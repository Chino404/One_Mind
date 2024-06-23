using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraControllerMenu : MonoBehaviour
{
    public Transform[] targetPositions;
    public float moveSpeed = 5f;
    private int currentTargetIndex = 0;

    void Update()
    {
        // Mover la c�mara hacia el objetivo actual
        if (targetPositions.Length > 0)
        {
            var target = targetPositions[currentTargetIndex];
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, moveSpeed * Time.deltaTime);
        }
    }

    // M�todo para cambiar el objetivo de la c�mara
    public void SetTargetPosition(int index)
    {
        if (index >= 0 && index < targetPositions.Length)
        {
            currentTargetIndex = index;
        }
        else
        {
            Debug.LogError("�ndice fuera de rango: " + index);
        }
    }
}