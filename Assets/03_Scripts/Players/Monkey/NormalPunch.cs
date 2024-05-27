using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPunch : Hits
{
    [SerializeField] float _damage;

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            target.TakeDamageEntity(_damage, _entity.transform.position);
        }
    }
}
