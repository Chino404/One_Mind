using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    //private Animator _animatorCamera;

    //private void Awake()
    //{
    //    _animatorCamera = GetComponent<Animator>();
    //}

    //private void Start()
    //{
    //    GameManager.instance.camerasPlayers[0] = gameObject;
    //    GameManager.instance._animCamMonkey = _animatorCamera;
    //}

    [Header("Components")]
    [SerializeField] Camera _cam;
    [SerializeField] Transform _target;

    [Header("Cursor")]
    [SerializeField] CursorLockMode _lockMode = CursorLockMode.Locked;
    [SerializeField] bool _isCursorVisible;

    [Header("Physics")]
    [SerializeField] float _hitOffset = 0.2f;

    [Header("Settings")]
    [Range(0.01f, 1f)] [SerializeField] float _detectionRadius = 0.1f;
    [Range(0.125f, 2f)] [SerializeField] float _minDistance = 0.25f;
    [Range(2f, 10f)] [SerializeField] float _maxDistance = 3f;


    float _mouseX, _mouseY;
    Vector3 _dir, _camPos;

    Ray _camRay;
    RaycastHit _camRayHit;
    bool _isCamBlocked;

    private void Start()
    {
        Cursor.lockState = _lockMode;
        Cursor.visible = _isCursorVisible;

        transform.forward = _target.forward;
        _mouseX = transform.eulerAngles.y;
        _mouseY = transform.eulerAngles.x;
    }

    private void FixedUpdate()
    {
        _camRay = new Ray(transform.position, _dir);

        _isCamBlocked = Physics.SphereCast(_camRay, _detectionRadius, out _camRayHit, _maxDistance);


    }

    private void LateUpdate()
    {
        transform.position = _target.position;//Actualizar la posicion del socket

    }

    private void Update()
    {
        UpdateSpringArm(); //Actualizar la posicion de la camara

    }

    void UpdateSpringArm()
    {
        _dir = -transform.forward;

        if (_isCamBlocked)
        {
            Vector3 dirTest = (_camRayHit.point - transform.position) + (_camRayHit.normal * _hitOffset);

            if (dirTest.sqrMagnitude <= Mathf.Pow(_minDistance, 2))
            {
                _camPos = transform.position + _dir * _minDistance;
            }

            else
            {
                _camPos = transform.position + dirTest;
            }
        }

        else
        {
            _camPos = transform.position + _dir * _maxDistance;
        }

        _cam.transform.position = _camPos;
        _cam.transform.LookAt(transform.position);
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pos, 0.1f);
        Gizmos.DrawSphere(_cam.transform.position, 0.125f);
        Gizmos.DrawLine(pos, _cam.transform.position);
    }
}
