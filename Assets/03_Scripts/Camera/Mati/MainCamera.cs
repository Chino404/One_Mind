using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Camera _cam;
    [SerializeField] Transform _target;

    [Header("Cursor")]
    [SerializeField] CursorLockMode _lockMode = CursorLockMode.Locked;
    [SerializeField] bool _isCursorVisible;

    
    private void Start()
    {
        Cursor.lockState = _lockMode;
        Cursor.visible = _isCursorVisible;

        transform.forward = _target.forward;
    }

    private void FixedUpdate()
    {
        transform.position = _target.position;//Actualizar la posicion del socket

    }
}
