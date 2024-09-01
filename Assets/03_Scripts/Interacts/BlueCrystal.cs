using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCrystal : MonoBehaviour, IInteractable
{
    private bool _isObjectAttached;
    [SerializeField]private bool _canActivate;

    public BaseBlueCrystal baseCrystal;
    

    public void Interact()
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
        //else if (_isObjectAttached)
        //{
        //    ReleaseObject();
        //}
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<BaseBlueCrystal>())
        {
            _canActivate = true;
            baseCrystal = collision.gameObject.GetComponentInParent<BaseBlueCrystal>();
        }
    }
    
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<BaseBlueCrystal>())
        {
            _canActivate = false;
            baseCrystal = null;
        }
    }

    public void Disconnect()
    {
        
    }
}
