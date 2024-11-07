using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    [Tooltip("Segundos de cooldown")][SerializeField] float _seconds; 
    [Tooltip("cantidad de balas que instancio al principio")][SerializeField] int _bulletQuantity;
    public Bullet prefab;

    Factory<Bullet> _factory;
    ObjectPool<Bullet> _objectPool;

    private bool _coolDown;

    private void Start()
    {
        _factory = new BulletFactory(prefab);
        
        _objectPool = new ObjectPool<Bullet>(_factory.GetObj, Bullet.TurnOff, Bullet.TurnOn, _bulletQuantity);
    }

    private void Update()
    {
        if(!_coolDown)
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        _coolDown = true;
        var bullet = _objectPool.Get();
        bullet.AddReference(_objectPool);
        bullet.transform.position = transform.position;
        bullet.transform.forward = transform.forward;
        yield return new WaitForSeconds(_seconds);
        _coolDown = false;
    }
}
