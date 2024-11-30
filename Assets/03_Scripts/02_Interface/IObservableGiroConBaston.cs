using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservableGiroConBaston
{
    void Subscribe(IObserverGiroConBaston obs);

    void Unsubscribe(IObserverGiroConBaston obs);
}
