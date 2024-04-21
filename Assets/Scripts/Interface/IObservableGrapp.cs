using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservableGrapp
{
    void Subscribe(IObserverGrappeable obs);

    void Unsubscribe(IObserverGrappeable obs);
}
