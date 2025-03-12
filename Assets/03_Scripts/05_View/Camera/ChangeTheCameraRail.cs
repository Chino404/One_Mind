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
        if (playerTarget == CharacterTarget.Bongo) CamerasManager.instance.currentBongoCamera.ChangeToRail(newRail);

        else CamerasManager.instance.currentFrankCamera.ChangeToRail(newRail);
    }

    public void Deactive()
    {
        
    }
}
