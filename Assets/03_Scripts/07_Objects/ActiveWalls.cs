using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWalls : MonoBehaviour, IInteracteable
{
    [SerializeField] private DesactiveWall[] _wallsToActive;
    
    public void Active()
    {
        foreach (var item in _wallsToActive)
        {
            item.Active();
        }
    }

    public void Deactive()
    {
        
    }
}
