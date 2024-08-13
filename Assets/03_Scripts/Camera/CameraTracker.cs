using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform[] _pointsTarget;
    [SerializeField] private Transform _point;
    private Transform _target;

    [Header("Smoothing Values")]
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeed = 0.075f;

    Vector3 _offset, _desiredPos, _smoothPos;

    private void Start()
    {
        if (_target == null) Debug.LogWarning("FALTA TARGET");

        _point = _pointsTarget[0];

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();

        transform.position = _pointsTarget[0].position;

        _target = GameManager.instance.assignedPlayer;

        // Inicializar offset como la diferencia entre la posición de la cámara y el target
        _offset = transform.position - _target.position;

    }

    private void Update()
    {
        _target = GameManager.instance.assignedPlayer;
    }

    private void FixedUpdate()
    {
        if (_target == null) return;
        //transform.position = target.position + _offset;

        //_desiredPos = target.position + _offset;
        //_smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeed);
        //transform.position = _smoothPos;
        
        SetPositionAndRotation(_pointsTarget[0]);

    }

    private void SetPositionAndRotation(Transform point)
    {
        _desiredPos = _target.position + _offset;
        _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeed);

        transform.SetPositionAndRotation(_smoothPos, point.rotation);
        Debug.DrawLine(transform.position, _target.position, Color.green);
    }

    public void TransicionPoint()
    {

    }

}
