using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverCamera
{
    /// <summary>
    /// Llamar cuando se haga el cambio de cámara.
    /// </summary>
    public void Change();
}
