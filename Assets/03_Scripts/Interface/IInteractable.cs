using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public void LeftClickAction();
    public void RightClickAction(Transform parent);
    public void ReleaseObject();
}
