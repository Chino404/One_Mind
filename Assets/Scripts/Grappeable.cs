using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappeable : MonoBehaviour, IObserverGrappeable
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IObservableGrapp>() != null)
            other.gameObject.GetComponent<IObservableGrapp>().Subscribe(this);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IObservableGrapp>() != null)
            other.gameObject.GetComponent<IObservableGrapp>().Unsubscribe(this);
    }

    public void Action()
    {
        Debug.Log("Gancho");
    }
}
