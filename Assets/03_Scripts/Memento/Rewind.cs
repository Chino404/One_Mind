using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rewind : MonoBehaviour
{
    protected MementoState _currentState;

    public virtual void Awake()
    {
        _currentState = new MementoState();
        GameManager.instance.rewinds.Add(this);
    }

    
    public abstract void Save();
    public abstract void Load();
}
