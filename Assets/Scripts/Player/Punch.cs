using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private int _damagePunch;
    public int Damage { set { _damagePunch = value; } }

    private Entity _transformEntity;

    private void Start()
    {
        _transformEntity = GetComponentInParent<Entity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            target.TakeDamageEntity(_damagePunch, _transformEntity.transform.position);
        }
    }
}
