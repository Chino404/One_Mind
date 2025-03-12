using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Connected : Rewind
{
    [SerializeField, Tooltip("Objeto conectado.")] protected Connected _connectedObject;

    [SerializeField] protected bool _isActive;
    public bool IsActive {  get { return _isActive; } }
}
