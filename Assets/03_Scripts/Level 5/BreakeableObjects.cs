using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakeableObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Peak peak = other.GetComponentInParent<Peak>();
        if (peak!=null&&other.gameObject!=peak.gameObject)
            gameObject.SetActive(false);
    }
}
