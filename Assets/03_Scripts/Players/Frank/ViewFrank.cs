using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewFrank
{
    private Animator _myAnimator;
    private ModelFrank _myModelFrank;

    public ViewFrank(ModelFrank model,Animator animator)
    {
        _myAnimator = animator;
        _myModelFrank = model;
    }

    public void Walking(bool value)
    {
        if (_myModelFrank.IsInIce)
        {
            _myAnimator.SetLayerWeight(1, 1f);
            _myAnimator.SetLayerWeight(0, 0f);
        }
        else
        {
            _myAnimator.SetLayerWeight(1, 0f);
            _myAnimator.SetLayerWeight(0, 1f);
        }

        _myAnimator.SetBool("Walk", value);
    }
}
