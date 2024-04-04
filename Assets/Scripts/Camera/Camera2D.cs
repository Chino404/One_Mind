using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    [SerializeField] Transform _target;
    [Header("Smoothing Values")]
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeed;

    Vector3 _offset, _desiredPos, _smoothPos;

    private void Start()
    {
        _offset = transform.position;

    }

    private void Update()
    {
        _desiredPos.x = _target.position.x+_offset.x;
        _desiredPos.y = _offset.y;
        _desiredPos.z = _offset.z;
        _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeed);
        transform.position = _smoothPos;
       
    }
}
