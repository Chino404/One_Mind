using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Rewind
{
    private void Awake()
    {
        _currentState = new MementoState();
    }
}
