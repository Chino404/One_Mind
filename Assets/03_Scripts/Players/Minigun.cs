using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : MonoBehaviour
{
    [Header("Bullet")]
    public BulletBanana bulletBanana;
    [SerializeField] int _bulletQuantity;
    Factory<BulletBanana> _factory;
    ObjectPool<BulletBanana> _objectPool;
    [SerializeField] private GameObject bulletSpawner;

    [SerializeField] private float _coolDown=0.2f;
    private bool _isShooting;

    
    void Start()
    {
        _factory = new BulletFactory(bulletBanana);
        _objectPool = new ObjectPool<BulletBanana>(_factory.GetObj, BulletBanana.TurnOff, BulletBanana.TurnOn, _bulletQuantity);
        gameObject.SetActive(false);
    }

    
    void Update()
    {
        if (_isShooting) return;
        StartCoroutine(Shoot(_coolDown));
    }

    IEnumerator Shoot(float cooldown)
    {
        _isShooting = true;
        yield return new WaitForSeconds(cooldown);
        var bullet = _objectPool.Get();
        bullet.AddReference(_objectPool);
        bullet.transform.position = bulletSpawner.transform.position;
        bullet.transform.rotation = bulletSpawner.transform.rotation;
        bulletBanana.transform.forward = bulletSpawner.transform.forward;
        _isShooting = false;
    }
}
