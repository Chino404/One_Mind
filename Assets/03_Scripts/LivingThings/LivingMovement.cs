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

    public override IEnumerator WaitSeconds()
    {
        Debug.Log("freno");
        
        _currentVelocity = 0;

        yield return new WaitForSeconds(_secondsWaiting);
        _currentVelocity = _maxVelocity;
        
    }
}
