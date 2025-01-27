using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectPool<T>
{
    public delegate T FactoryMethod();//creamos un tipo de dato para guardar metodos void que no reciban parametros
    //[Tooltip("METODO que instancia el objeto")] FactoryMethod _arrayFactory;
    [Tooltip("METODO que instancia el objeto")] FactoryMethod _factory;

    [Tooltip("Funcion donde se apaga")] Action<T> _turnOff;
    [Tooltip("Funcion donde se enciende")] Action<T> _turnOn;

    [Tooltip("Mi stock de los objetos creados")] List<T> _stock = new List<T>();

    public ObjectPool(FactoryMethod factory, Action<T> turnOff, Action<T> turnOn, int initialCount = 5)
    {
        _factory = factory; //El METODO que Instancia el objeto

        _turnOff = turnOff;
        _turnOn = turnOn;

        for (int i = 0; i < initialCount; i++)
        {
            //Creo una variable temporal donde me guardo el objeto llamando al metodo para crearlo.
            var obj = _factory();

            _turnOff(obj); //Lo apago

            _stock.Add(obj); //Lo agrego a la lista
        }
    }

    public T Get()
    {
        T obj = default;

        if (_stock.Count > 0)
        {
            obj = _stock[0]; //Obtengo el primer objeto

            _turnOn(obj); //Lo enciendo 

            _stock.RemoveAt(0);//Remuevo el indice 0 (es decir, el primer objeto) de la lista
        }
        else
        {
            obj = _factory();//instancio por si me quedo sin objeto para pedir
        }

        return obj;
    }

    public void StockAdd(T obj)
    {
        _turnOff(obj);
        _stock.Add(obj);
    }

}
