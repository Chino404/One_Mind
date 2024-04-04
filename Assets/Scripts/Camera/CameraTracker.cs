using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [SerializeField]Transform _target;

    [Header("Smoothing Values")]
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeed;

    Vector3 _offset, _desiredPos, _smoothPos;
  
    private void Start()
    {
        _offset = transform.position;

    }

    private void FixedUpdate()
    {
        _desiredPos = _target.position + _offset;
        _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeed);
        transform.position = _smoothPos;
    }

    //void UpdateSpringArm()
    //{
    //    _desiredPos = -transform.forward;

    //    if (_isCamBlocked)
    //    {
    //        Vector3 dirTest = (_camRayHit.point - transform.position) + (_camRayHit.normal * _hitOffset);

    //        if (dirTest.sqrMagnitude <= Mathf.Pow(_minDistance, 2))
    //        {
    //            transform.position+= _desiredPos * _minDistance;
    //        }

    //        else
    //        {
    //           transform.position += dirTest;
    //        }
    //    }

    //    else
    //    {
    //         transform.position += _desiredPos * _maxDistance;
    //    }

    //    _cam.transform.position = _camPos;
    //    _cam.transform.LookAt(transform.position);
    //}
}
