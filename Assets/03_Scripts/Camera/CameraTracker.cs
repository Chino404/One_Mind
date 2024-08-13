using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public static CameraTracker Instance;

    [Header("Components")]
    [SerializeField] private Transform _point;

    private Transform _target;

    [Header("Smoothing Values")]
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeedPosition = 0.075f;
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeedRotation = 0.075f;

    Vector3 _desiredPos, _smoothPos;
    Quaternion _smoothRot;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        if(_point == null)
        {
            Debug.LogError("ASIGNAR PUNTO A LA CÁMARA");
            return;
        }

        StartCoroutine(Wait());

    }

    IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();

        transform.position = _point.position;

        _target = GameManager.instance.assignedPlayer;
        if (_target == null) Debug.LogError("FALTA TARGET");

    }

    private void Update()
    {
        _target = GameManager.instance.assignedPlayer;
    }

    private void FixedUpdate()
    {
        if (_target == null || _point == null) return;     
        
        SetPositionAndRotationTarget();
    }

    private void SetPositionAndRotationTarget()
    {

        // Calcular la posición deseada relativa al punto
        _desiredPos = _target.position + (_point.position - _target.position);

        _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeedPosition);

        _smoothRot = Quaternion.Lerp(transform.rotation, _point.rotation, _smoothSpeedRotation);

        transform.SetPositionAndRotation(_smoothPos, _smoothRot);

        Debug.DrawLine(transform.position, _target.position, Color.green);
    }

    public void TransicionPoint(Transform newPoint)
    {
        _point = newPoint;
    }

}
