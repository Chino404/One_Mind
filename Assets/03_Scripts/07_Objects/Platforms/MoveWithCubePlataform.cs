using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithCubePlataform : MonoBehaviour
{
    public Transform cube;
    private Vector3 _startCubePosition;

    private void Start()
    {
        _startCubePosition = cube.position;
    }

    private void FixedUpdate()
    {
        Vector3 movement = cube.position - _startCubePosition;
        transform.position += movement;
        _startCubePosition = cube.position;
    }
}


