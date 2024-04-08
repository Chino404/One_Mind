using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
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
            damageable.TakeDamageEntity(_dmg, transform);
        }
    }

    public void TakeDamageEntity(float dmg, Transform target)
    {
        if(_life > 0)
        {
            _life -= dmg;
            if(_life <= 0 )
                _life = 0;
        }


        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;

        _rigidbody.AddForce(-transform.forward * 5, ForceMode.VelocityChange);

    }
}
