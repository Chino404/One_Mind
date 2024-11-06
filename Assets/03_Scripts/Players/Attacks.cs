using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<IHittable>();
        if (interactable != null)
            interactable.Action();

    }
}
