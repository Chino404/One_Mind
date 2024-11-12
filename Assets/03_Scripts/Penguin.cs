using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    [Tooltip("Segundos de cooldown")][SerializeField] float _coolDown; 
    [Tooltip("cantidad de balas que instancio al principio")][SerializeField] int _bulletQuantity;
    public Bullet prefab;

    Factory<Bullet> _factory;
    ObjectPool<Bullet> _objectPool;

    private bool _isInCoolDown;
    [Tooltip("si dispara en rafaga")]public bool isBurst;
    public float timeShooting;
    private float _secondsShooting;

    private void Start()
    {
        _factory = new BulletFactory(prefab);
        
        _objectPool = new ObjectPool<Bullet>(_factory.GetObj, Bullet.TurnOff, Bullet.TurnOn, _bulletQuantity);
        _secondsShooting = timeShooting;
    }

    private void Update()
    {
        
        if (!_isInCoolDown)
        {
            if (!isBurst)
                StartCoroutine(Shoot());

            else
                StartCoroutine(BurstShoot());
            
               
            


        }
    }

    IEnumerator Shoot()
    {
        _isInCoolDown = true;
        
        var bullet = _objectPool.Get();
        bullet.AddReference(_objectPool);
        bullet.transform.position = transform.position;
        bullet.transform.forward = transform.forward;
        
        yield return new WaitForSeconds(_coolDown);
        _isInCoolDown = false;
        
    }

    IEnumerator BurstShoot()
    {
        if (timeShooting <= 0)
        {
            yield return new WaitForSeconds(_secondsShooting);
            timeShooting = _secondsShooting;
        }

        timeShooting-=Time.time;
        var bullet = _objectPool.Get();
        bullet.AddReference(_objectPool);
        bullet.transform.position = transform.position;
        bullet.transform.forward = transform.forward;
        yield return new WaitForSeconds(_coolDown);
        
    }
}
