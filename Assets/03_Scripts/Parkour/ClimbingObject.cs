using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingObject : MonoBehaviour
{
    public ModelMonkey monkey;
    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject==monkey.gameObject)
        {
            monkey.GetComponent<Rigidbody>().isKinematic = true;
            monkey.isRestricted = true;
            
        }
    }

    

   
}
