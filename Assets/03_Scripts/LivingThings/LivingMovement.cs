using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LivingMovement : MoveThings
{
    private Animator _animator;

    public override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_currentVelocity > 0) _animator.SetBool("IsWalking", true);
        else if (_currentVelocity == 0) _animator.SetBool("IsWalking", false);
    }

    public override void FixedUpdate()
    {
        if (_actualIndex >= waypoints.Length)
        {
            _currentVelocity = 0f;
            return;
        }
        

        if (Vector3.Distance(_rb.position, waypoints[_actualIndex].position) <= 1f)
        {
            StartCoroutine(WaitSeconds());
            _actualIndex++;

            
        }

        _velocity = waypoints[_actualIndex].position - _rb.position;
        _velocity.Normalize();
        transform.LookAt(waypoints[_actualIndex].position);

        //transform.position += _velocity*_maxVelocity * Time.deltaTime;
        _rb.MovePosition(_rb.position + _velocity * _currentVelocity * Time.fixedDeltaTime);
    }

    public override IEnumerator WaitSeconds()
    {
        Debug.Log("freno");
        
        _currentVelocity = 0;

        yield return new WaitForSeconds(_secondsWaiting);
        _currentVelocity = _maxVelocity;
        
    }
}
