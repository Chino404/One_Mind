using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnElevator : MonoBehaviour, IInteracteable
{
    [SerializeField] private Elevator _elevator;

    public void Active()
    {

        if (_elevator.IsNotMove)
        {
            _elevator.ActiveElevator();

            _elevator.isPlaySound = false;
        }
    }

    public void Deactive()
    {
        
    }
}
