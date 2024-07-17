using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrystal : MonoBehaviour, IInteractable
{
    public void Action()
    {
        Debug.Log("hago accion");
        gameObject.SetActive(false);
    }

    
}
