using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPunch : Hits
{
    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            target.TakeDamageEntity(_damagePunch, _entity.transform.position);
        }
    }
}
