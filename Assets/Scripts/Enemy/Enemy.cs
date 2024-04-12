using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    [SerializeField] private float _dmg;
    [SerializeField] private float _life;
    [SerializeField] private float _forceGravity;
    private Rigidbody _rigidbody;

    ObjectPool<Enemy> _objectPool;
    public float counter;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(Vector3.down * _forceGravity, ForceMode.VelocityChange);
    }

    void Update()
    {
        if(counter>=2)
        {
            _objectPool.StockAdd(this);
        }
    }

    public void AddReference(ObjectPool<Enemy> en)
    {
        _objectPool = en;
    }

    public static void TurnOff(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    public static void TurnOn(Enemy enemy)
    {
        enemy.counter = 0;
        enemy.gameObject.SetActive(true);
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

    public void GetUpDamage(float dmg, Vector3 target, float forceToUp)
    {
        if (_life > 0)
        {
            _life -= dmg;
            if (_life <= 0)
                _life = 0;
        }

        Vector3 direction = target - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, forceToUp);
    }
}
