using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T> : MonoBehaviour
{
    public delegate T FactoryMethod();//creamos un tipo de dato para guardar metodos void que no reciban parametros
    FactoryMethod _factory;

    Action<T> _turnOff;
    Action<T> _turnOn;

    List<T> _stock = new List<T>();

    public ObjectPool(FactoryMethod factory, Action<T> turnOff, Action<T> turnOn, int initialCount = 5)
    {
        _factory = factory;
        _turnOff = turnOff;
        _turnOn = turnOn;

        for (int i = 0; i < initialCount; i++)
        {
            var obj = _factory();

            _turnOff(obj);

            _stock.Add(obj);
        }
    }
    public T Get()
    {
        T obj = default;
        if (_stock.Count > 0)
        {
            obj = _stock[0];

            _turnOn(obj);
            _stock.RemoveAt(0);//saco el indice 0 de la lista
        }
        else
        {
            obj = _factory();//instancia enemigos por si me quedo sin enemigos
        }

        return obj;
    }

    public void StockAdd(T obj)
    {
        _turnOff(obj);
        _stock.Add(obj);
    }

}
