using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTheCameraRail : MonoBehaviour, IInteracteable
{
    public CharacterTarget playerTarget;
    [Space(10)]public bool isRemoveCurrentRail;
    public Rail newRail;


    public void Active()
    {
        if (!newRail && !isRemoveCurrentRail)
        {
            Debug.LogError($" Falra referencia de Rail en: <color=yellow>{gameObject.name}</color>");
            return;
        }

        if (playerTarget == CharacterTarget.Bongo)
        {
            if (isRemoveCurrentRail)
            {
                CamerasManager.instance.currentBongoCamera.RemoveCurrentRail();
                return;
            }

            CamerasManager.instance.currentBongoCamera.ChangeToRail(newRail);
        }

        else
        {
            if (isRemoveCurrentRail)
            {
                CamerasManager.instance.currentFrankCamera.RemoveCurrentRail();
                return;
            }

            CamerasManager.instance.currentFrankCamera.ChangeToRail(newRail);
        }
    }

    public void Deactive()
    {
        
    }
}
