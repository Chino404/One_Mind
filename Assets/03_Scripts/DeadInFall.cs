using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadInFall : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            target.Dead();

        }

            
    }
}
