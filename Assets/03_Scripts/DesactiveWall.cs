using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactiveWall : Rewind
{
    public bool _isActing;
    protected bool _shouldOpenOnEnable;
    protected bool _shouldCloseOnEnable;
    
    public override void Save()
    {

    }

    public override void Load()
    {
       
    }
    

    public virtual void Active()
    {
        
    }

    public virtual void Desactive()
    {
        
    }

    private void OnEnable()
    {
        if (_shouldCloseOnEnable)
        {
            Active();
        }
        if (_shouldOpenOnEnable)
        {
            Desactive();
        }
    }
}
