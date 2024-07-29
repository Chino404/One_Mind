using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCrystal : MonoBehaviour, IInteractable
{
    private bool _isObjectAttached;
    [SerializeField]private bool _canActivate;

    public BaseBlueCrystal baseCrystal;
    

    public void LeftClickAction()
    {
        if (!_canActivate) return;
        baseCrystal.SpawnPath();
        
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
        if (collision.gameObject.GetComponent<BaseBlueCrystal>())
        {
            _canActivate = true;
            baseCrystal = collision.gameObject.GetComponent<BaseBlueCrystal>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<BaseBlueCrystal>())
        {
            _canActivate = false;
            baseCrystal = null;
        }
    }

}
