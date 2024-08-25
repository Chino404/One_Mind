using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Rewind
{
    public override void Awake()
    {
        _currentState = new MementoState();
    }
}
