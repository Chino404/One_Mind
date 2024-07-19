using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMovil : MonoBehaviour, IInteractable
{
    public void LeftClickAction()
    {
        
    }

    public void NotParent()
    {
        
    }

    public void RightClickAction(Transform parent)
    {
        if(!transform.IsChildOf(parent))
        transform.SetParent(parent);

        if(transform.IsChildOf(parent)) 
        {
            Debug.Log("dejo de ser hijo");
            transform.SetParent(null);
        }
    }

    
}
