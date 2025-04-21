using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCurrentCollider : MonoBehaviour
{
    public GameObject[] plataforms;

    

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            foreach (var item in plataforms)
            {
                item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
        }
        
    }
}
