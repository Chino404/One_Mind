using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBanana : MonoBehaviour
{
    [Header("Object Pool")]
    public float counter;
    ObjectPool<BulletBanana> _objectPool;

    private void Update()
    {
        if (counter >= 2)
        {
            _objectPool.StockAdd(this);
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
