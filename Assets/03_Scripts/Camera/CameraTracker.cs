using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [Header("Components")]
    public Transform target;
    [SerializeField]private Transform[] _pointsTarget;

    [Header("Smoothing Values")]
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeed = 0.075f;

    public Vector3 _offset, _desiredPos, _smoothPos;

    private void Start()
    {
        //target = GameManager.instance.assignedPlayer;

        SetPositionAndRotation(_pointsTarget[0]);

        //_offset = transform.position;
    }

    private void Update()
    {
        target = GameManager.instance.assignedPlayer;
    }

    private void FixedUpdate()
    {
        //transform.position = target.position + _offset;
        //_desiredPos = target.position + _offset;
        //_desiredPos = target.position + _offset;
        //_smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeed);
        //transform.position = _smoothPos;
        
        SetPositionAndRotation(_pointsTarget[0]);

    }

    private void SetPositionAndRotation(Transform point)
    {
        transform.SetLocalPositionAndRotation(point.position, point.rotation);
        Debug.DrawLine(transform.position, target.position, Color.green);
    }

}
