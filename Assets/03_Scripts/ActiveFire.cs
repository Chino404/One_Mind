using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFire : Rewind
{
    public GameObject turnOnFire;
    public GameObject turnOffFire;
    public override void Save()
    {
        if (turnOnFire != null)
            _currentState.Rec(turnOffFire.activeInHierarchy, turnOnFire.activeInHierarchy);
        else _currentState.Rec(turnOffFire.activeInHierarchy);


    }
    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        turnOffFire.SetActive((bool)col.parameters[0]);
        if (turnOnFire != null)
            turnOnFire.SetActive((bool)col.parameters[1]);
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Characters>())
        {
            turnOffFire.SetActive(false);
            if (turnOnFire != null)
                turnOnFire.SetActive(true);
        }
    }
}
