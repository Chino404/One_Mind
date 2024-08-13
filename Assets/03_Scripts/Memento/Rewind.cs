using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rewind : MonoBehaviour
{
    protected MementoState _currentState;

    private void Awake()
    {
        _currentState = new MementoState();
    }

    private void Start()
    {
        GameManager.Instance.rewinds.Add(this);
    }

    
    public abstract void Save();
    public abstract void Load();
}
