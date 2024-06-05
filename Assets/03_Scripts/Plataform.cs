using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : Rewind
{
    [SerializeField] float _secondsWaiting=2f;
    [SerializeField] Transform[] _waypoints;
    [SerializeField] float _maxVelocity;
    //[SerializeField] float _maxForce;
    private int _actualIndex;
    private Vector3 _velocity;

    private void Update()
    {
        AddForce(Seek(_waypoints[_actualIndex].position));
        if(Vector3.Distance(transform.position, _waypoints[_actualIndex].position)<=0.3f)
        {
            StartCoroutine(WaitSeconds());
            _actualIndex++;
            if (_actualIndex >= _waypoints.Length)
                _actualIndex = 0;
        }
        transform.position += _velocity * Time.deltaTime;
        //transform.forward = _velocity;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer==3)
            collision.transform.SetParent(this.transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer==3)
            collision.transform.SetParent(null);
    }

    public override void Save()
    {
        _currentState.Rec(transform.position);
        Debug.Log("guardo plataforma");
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();

        transform.position = (Vector3)col.parameters[0];
        Debug.Log("cargo plataforma");

    }
}
