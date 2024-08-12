using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public static CameraTarget instance;

    [SerializeField]private Transform target;
    public Transform Target { set { target = value; } }

    [SerializeField] private float _smoothSpeed;

    [SerializeField] Vector3 pos;
    private Vector3 _velocity;

    private void Awake()
    {
        if (instance != null) instance = this;
    }

    private void Start()
    {
        pos = transform.position - target.position;
    }

    private void FixedUpdate()
    {

        _velocity.Normalize();

        if (target.position.y + pos.y != transform.position.y) _velocity.y = target.position.y+pos.y - transform.position.y;
        else _velocity.y = 0;

        if (target.position.z + pos.z != transform.position.z)_velocity.z = target.position.z + pos.z - transform.position.z;
        else _velocity.z = 0;

        if (target.position.x + pos.x != transform.position.x)_velocity.x = target.position.x + pos.x - transform.position.x;
        else _velocity.x = 0;

        transform.position += _velocity * _smoothSpeed * Time.deltaTime;
    }
}
