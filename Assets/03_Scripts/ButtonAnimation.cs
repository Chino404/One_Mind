using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour, IInteracteable
{
    [Tooltip("boton que tiene que hacer la animacion")][SerializeField] private GameObject _button;
    public float _endAnimation;

    private bool _isPressed;

    void Update()
    {
        if (_isPressed == true)
        {
            if (_button.transform.localPosition.y >= _endAnimation)
                _button.transform.localPosition -= new Vector3(0, Time.deltaTime, 0);
        }
        
    }

    public void Active()
    {

        _isPressed = true;
        Debug.Log("lo aprete");
        
    }

    

    public void Deactive()
    {
        _isPressed = false;
        Debug.Log("lo desaprete");
    }
}
