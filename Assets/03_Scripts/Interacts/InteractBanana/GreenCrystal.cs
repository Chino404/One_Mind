using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrystal : MonoBehaviour, IInteractable
{
    public void LeftClickAction()
    {
        Debug.Log("hago accion");
        gameObject.SetActive(false);
    }


    public void RightClickAction(Transform parent)
    {
        
    }
}
