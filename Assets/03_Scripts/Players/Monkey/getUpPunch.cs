using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getUpPunch : Hits
{
    [SerializeField] float _damage;
    private float _forceToUp;
    public float ForceToUp { set { _forceToUp = value; } }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            target.GetUpDamage(_damage, _entity.transform.position, _forceToUp);
        }
    }
}
