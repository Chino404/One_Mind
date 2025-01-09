using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour, IInteracteable
{
    public Mechanism[] mechanism;

    public void Active()
    {
        for (int i = 0; i < mechanism.Length; i++)
        {
            mechanism[i].ActiveMechanism();
        }
    }

    public void Deactive()
    {

    }

}
