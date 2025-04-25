using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Connected : Rewind
{
    [SerializeField, Tooltip("Objeto conectado.")] protected Connected _connectedObject;

    [SerializeField, Tooltip("Si el objeto est� activado")] protected bool _isActive;
    [Tooltip("Si el objeto est� activado")]public bool IsActive {  get { return _isActive; } }
}
