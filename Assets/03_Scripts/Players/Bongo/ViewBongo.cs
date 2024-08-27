using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBongo
{
    private Animator _animator;

    public ViewBongo(Animator animator)
    {
        _animator = animator;
    }

    public void Walking(bool value)
    {
        _animator.SetBool("Walk", value);
    }
}
