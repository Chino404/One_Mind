using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    private Animator _myAnimator;
    private Button _button;

    private void Awake()
    {
        _myAnimator = GetComponent<Animator>();
        _button = GetComponent<Button>();
    }

    public void PointExitFunc()
    {
        
        if(_button.interactable) _myAnimator.SetTrigger("PointExit");
    }
}
