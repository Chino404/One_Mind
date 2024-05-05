using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadInFall : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<ModelMonkey>();
        if (target != null)
        {
            target.TakeDamageEntity(1000,this.transform.position);
            Debug.Log("me cai");

        }

            
    }
}
