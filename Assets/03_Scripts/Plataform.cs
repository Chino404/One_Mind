using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour
{
    [SerializeField] float _secondsWaiting=2f;
    [SerializeField] Transform[] _waypoints;
    [SerializeField] float _maxVelocity;

    
    [SerializeField]private int _actualIndex;
    private Vector3 _velocity;
    public Vector3 Velocity { get; private set; }


    private Rigidbody _rb;
    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        AddForce(Seek(_waypoints[_actualIndex].position));
        
        
        if (Vector3.Distance(_rb.position, _waypoints[_actualIndex].position)<=0.4f)
        {
            StartCoroutine(WaitSeconds());
            _actualIndex++;
            if (_actualIndex >= _waypoints.Length)
                _actualIndex = 0;
        }
        //transform.position += _velocity * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
    }

    

    IEnumerator WaitSeconds()
    {
        var velocity = _maxVelocity;
        _maxVelocity = 0;
        yield return new WaitForSeconds(_secondsWaiting);
        _maxVelocity = velocity;

    }

    Vector3 Seek(Vector3 target)
    {
        var desired = target - transform.position;
        desired.Normalize();
        desired *= _maxVelocity;
        return desired;
    }

    void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
