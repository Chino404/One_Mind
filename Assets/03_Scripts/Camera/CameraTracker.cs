using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [Header("Components")]
    public Transform _target;

    [Header("Smoothing Values")]
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeed = 0.075f;

    Vector3 _offset, _desiredPos, _smoothPos;

    private void Start()
    {
        _offset = transform.position;
        //_target = GameManager.instance.playerGM.transform;
    }

    private void FixedUpdate()
    {
        //transform.position = _target.position + _offset;
        _desiredPos = _target.position/* + _offset*/;
        _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeed);
        transform.position = _smoothPos;
    }
}
