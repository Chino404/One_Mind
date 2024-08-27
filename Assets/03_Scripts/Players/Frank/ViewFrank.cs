using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewFrank
{
    private Animator _animator;

    public ViewFrank(Animator animator)
    {
        _animator = animator;
    }

    public void Walking(bool value)
    {
        _animator.SetBool("Walk", value);
    }
}
