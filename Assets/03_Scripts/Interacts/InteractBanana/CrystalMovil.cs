using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMovil : MonoBehaviour, IInteractable
{
    private bool _isObjectAttached;
    public void LeftClickAction()
    {
        
    }

    public void ReleaseObject()
    {
        
    }

    public void RightClickAction(Transform parent)
    {
        if (!_isObjectAttached)
        {
            transform.SetParent(parent);
            _isObjectAttached = true;
        }
        else if (_isObjectAttached)
        {
            transform.SetParent(null);
            _isObjectAttached = false;
        }
    }

    
}
