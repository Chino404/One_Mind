using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrystal : MonoBehaviour, IInteractable
{
    public void Disconnect()
    {
        
    }

    public void Action()
    {
        Debug.Log("hago accion");
        gameObject.SetActive(false);
    }

    public void ReleaseObject()
    {
       
    }

    public void RightClickAction(Transform parent)
    {
        
    }
}
