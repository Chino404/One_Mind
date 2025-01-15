using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectPool<T>
{
    public delegate T FactoryMethod();//creamos un tipo de dato para guardar metodos void que no reciban parametros
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

        Debug.Log(obj);

        return obj;
    }

    /// <summary>
    /// Obtiene un objeto del stock basado en un valor de Enum.
    /// </summary>
    /// <typeparam name="E">El tipo del Enum.</typeparam>
    /// <param name="enumValue">El valor del Enum a buscar.</param>
    /// <returns>Un objeto del stock cuyo Enum coincida.</returns>
    public T GetByEnum<E>(E enumValue) where E : Enum //Donde "E" es un Enum
    {
        T obj = default;

        if (_stock.Count > 0)
        {
            foreach (var objType in _stock)
            {            
                // Verificar que el objeto implementa la interfaz
                if (objType is IEnumType<E> enumObject)
                {                
                    if (enumObject.Type.Equals(enumValue))
                    {
                        _turnOn(objType); // Activamos el objeto
                        _stock.Remove(objType); // Lo removemos del stock

                        obj = objType;

                        break;
                    }         
                }
                
            }
        }
        else
        {
            Debug.Log("Creo el objeto");
            obj = _factory();
        }

        Debug.Log(obj);

        return obj;
    }


    public void StockAdd(T obj, FactoryMethod factory = default)
    {
        if (factory != default)
        {
            _factory = factory;

            obj = _factory();
        }

        _turnOff(obj);
        _stock.Add(obj);
    }

}
