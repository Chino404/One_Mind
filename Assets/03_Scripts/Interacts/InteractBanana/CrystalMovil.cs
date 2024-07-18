using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMovil : MonoBehaviour,IInteractable
{
    public void LeftClickAction()
    {
        
    }

    public void RightClickAction(Transform parent)
    {
        transform.SetParent(parent);
    }

    
}
