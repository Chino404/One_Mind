using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rewind : MonoBehaviour
{
    protected MementoState _currentState;

    private void Awake()
    {
        Debug.Log("REWINDDDD");
        _currentState = new MementoState();
    }

    public abstract void Save();
    public abstract void Load();
}
