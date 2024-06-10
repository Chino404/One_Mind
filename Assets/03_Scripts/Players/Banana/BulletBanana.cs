using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBanana : MonoBehaviour
{
    [SerializeField] float _speed;

    [Header("Object Pool")]
    public float counter;
    
    ObjectPool<BulletBanana> _objectPool;

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
        counter += Time.deltaTime;

        if (counter >= 2)
        {
            _objectPool.StockAdd(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var aux = other.gameObject.GetComponent<IExplosion>();

        if (aux != null)
        {
            aux.Execute();
        }
    }

    public static void TurnOff(BulletBanana bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    public static void TurnOn(BulletBanana bullet)
    {
        bullet.counter = 0;
        bullet.gameObject.SetActive(true);
    }

    public void AddReference(ObjectPool<BulletBanana> bu)
    {
        _objectPool = bu;
    }
}
