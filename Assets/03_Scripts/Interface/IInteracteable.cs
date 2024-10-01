using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteracteable
{
    /// <summary>
    /// Llamar cuando se INTERACTUA
    /// </summary>
    public void Interact();

    /// <summary>
    /// Llamaer cuando se DEJA de interactuar
    /// </summary>
    public void Disconnect();
}
