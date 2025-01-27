using System.Collections.Generic;
using UnityEngine;

public class OPSBoxesManager : MonoBehaviour
{
    public static OPSBoxesManager instance;

    [SerializeField] private Box[] _boxesModel;
    [Tooltip("cantidad de cajas que instancio al principio")][SerializeField] int _boxesQuantity;

    private Dictionary<BoxType, ObjectPool<Box>> _objectsPoolDict;

    private void Awake()
    {
        instance = this;
        _objectsPoolDict = new ();

        foreach (var prefab in _boxesModel)
        {
            if(!_objectsPoolDict.ContainsKey(prefab.type)) //Si no existe el prefab en el diccionario
            {
                var factory = new BoxesFactory(new[] { prefab }); //Paso el prefab al factory
                var pool = new ObjectPool<Box>(factory.GetArrayObjects, Box.TurnOff, Box.TurnOn, _boxesQuantity); //Llamo al ObjectPool

                _objectsPoolDict[prefab.type] = pool; //Le asigno a la key del diccionario el pool
            }
        }
    }

    /// <summary>
    /// Obtener una caja según el tipo especificado.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Box GetBox(BoxType type)
    {
        if (_objectsPoolDict.TryGetValue(type, out var pool))
        {
            var bullet = pool.Get(); //Dame el ojeto
            bullet.AddReference(pool); //Lo añado la referencia de la pool 
            return bullet;
        }

        Debug.LogError($"No se encontró un pool para el tipo de bala: {type}");
        return null;
    }
}
