using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    [HideInInspector] public int _damagePunch;

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            target.TakeDamageEntity(_damagePunch, transform);
        }
    }
}
