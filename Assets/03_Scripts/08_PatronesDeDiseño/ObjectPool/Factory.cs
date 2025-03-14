using System.Collections.Generic;
using UnityEngine;

public abstract class Factory<T> where T : MonoBehaviour //Donde lo que sea <T> herede de Monobeheaviour
{
    public T prefab;

    public T[] arrayPrefabs;

    /// <summary>
    /// Obtengo el objeto <T>
    /// </summary>
    /// <returns></returns>
    public virtual T GetObj() //Si quiero puedo modificar este metodo con el que herede de Factory
    {
        return GameObject.Instantiate(prefab);
    }

    public virtual T GetArrayObjects()
    {
        if (arrayPrefabs.Length == 0)
        {
            Debug.LogError("No se han asignado prefabs al Factory.");
            return null;
        }

        //No genera problemas (dejarlo)
        int randomIndex = Random.Range(0, arrayPrefabs.Length);
        return GameObject.Instantiate(arrayPrefabs[randomIndex]);
    }
}
