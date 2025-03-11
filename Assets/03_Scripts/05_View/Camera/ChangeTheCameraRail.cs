using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTheCameraRail : MonoBehaviour, IInteracteable
{
    public CharacterTarget playerTarget;
    public Rail newRail;

    public void Active()
    {
        Debug.LogWarning("Entre!!");
        if (playerTarget == CharacterTarget.Bongo) GameManager.instance.bongoRailsCamera.ChangeToRail(newRail);

        else GameManager.instance.frankRailsCamera.ChangeToRail(newRail);
    }

    public void Deactive()
    {
        
    }
}
