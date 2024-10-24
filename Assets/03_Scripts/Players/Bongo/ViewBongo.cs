using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBongo
{
    private Animator _myAnimator;
    private ModelBongo _myModelBongo;

    public ViewBongo(ModelBongo model , Animator animator)
    {
        _myAnimator = animator;
        _myModelBongo = model;

        
    }

    public void Walking(bool value)
    {
        if(_myModelBongo.IsInIce)
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
