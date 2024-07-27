using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCrystal : MonoBehaviour, IInteractable
{
    private bool _isObjectAttached;
    [SerializeField]private bool _canActivate;
    
    

    public void LeftClickAction()
    {
        
    }

    public void ReleaseObject()
    {
        transform.SetParent(null);
        _isObjectAttached = false;
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
            ReleaseObject();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 17)
        {
            _canActivate = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 17)
        {
            _canActivate = false;
        }
    }

}
