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

    public override IEnumerator WaitSeconds()
    {
        Debug.Log("freno");
        _animator.SetBool("IsWalking", false);
        _currentVelocity = 0;

        yield return new WaitForSeconds(_secondsWaiting);
        _currentVelocity = _maxVelocity;
        _animator.SetBool("IsWalking", true);
    }
}
