using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    [SerializeField] private float _dmg;
    [SerializeField] private float _life;
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.gameObject.GetComponent<IDamageable>();

        if(damageable != null)
        {
            damageable.TakeDamageEntity(_dmg, transform.position);
        }
    }

    public void TakeDamageEntity(float dmg, Vector3 target)
    {
        if(_life > 0)
        {
            _life -= dmg;
            if(_life <= 0 )
                _life = 0;
        }

        Vector3 direction = target - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        _rigidbody.AddForce(-transform.forward * 7, ForceMode.VelocityChange);

    }

    public void GetUpDamage()
    {

    }
}
