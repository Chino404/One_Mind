using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverCamera
{
    /// <summary>
    /// Llamar cuando se haga el cambio de c�mara.
    /// </summary>
    public void Change();
}
