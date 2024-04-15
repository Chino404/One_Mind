using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateEnemies : MonoBehaviour
{
    public Enemy enemy;
    public Transform[] instantiatePoint;
    [SerializeField] int _enemiesQuantity;//cantidad de enemigos que instancio al principio

    Factory<Enemy> _factory;
    ObjectPool<Enemy> _objectPool;

    private void Start()
    {
        _factory = new EnemyFactory(enemy);
        _objectPool = new ObjectPool<Enemy>(_factory.GetObj, Enemy.TurnOff, Enemy.TurnOn, _enemiesQuantity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
            InstantiateEnemy();
    }

    void InstantiateEnemy()
    {
        foreach (var item in instantiatePoint)
        {
            var enemy = _objectPool.Get();
            enemy.AddReference(_objectPool);
            enemy.transform.position = item.transform.position;
            
        }
    }
}
