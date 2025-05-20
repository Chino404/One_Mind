using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Connected : Rewind
{
    [SerializeField, Tooltip("Objeto conectado.")] protected Connected _connectedObject;

    [Space(5), SerializeField, Tooltip("Si el objeto está activado")] protected bool _isActive;
    [Tooltip("Si el objeto está activado")]public bool IsActive {  get { return _isActive; } }
}
