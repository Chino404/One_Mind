using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMovementCollider : MonoBehaviour
{
    public LivingMovement animal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
            animal.canMove = true;
    }
}
