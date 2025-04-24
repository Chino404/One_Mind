using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnElevator : MonoBehaviour, IInteracteable
{
    [SerializeField] private Elevator _elevator;

    public void Active()
    {

        if (_elevator.isNotMove)
        {
            _elevator.isNotMove = false;

            //_elevator.IsActiveElevator = true;

            _elevator.ActiveElevator(true);
        }
    }

    public void Deactive()
    {
        
    }
}
