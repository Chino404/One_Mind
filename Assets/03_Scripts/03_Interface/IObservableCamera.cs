using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservableCamera
{
    /// <summary>
    /// Para suscribirme.
    /// </summary>
    public void Suscribe(IObserverCamera obs);

    /// <summary>
    /// Para desuscribirme.
    /// </summary>
    public void Unsuscribe(IObserverCamera obs);
}
