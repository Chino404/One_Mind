using System.Collections.Generic;
using UnityEngine;

public abstract class Factory<T> where T : MonoBehaviour //Donde lo que sea <T> herede de Monobeheaviour
{
    public T prefab;

    /// <summary>
    /// Obtengo el objeto <T>
    /// </summary>
    /// <returns></returns>
    public virtual T GetObj() //Si quiero puedo modificar este metodo con el que herede de Factory
    {
        return GameObject.Instantiate(prefab);
    }
}
