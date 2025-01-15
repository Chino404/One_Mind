using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxType
{
    Normal,
    Alta,
    Ancha
}

public class Box : MonoBehaviour, IEnumType<BoxType>
{
    [SerializeField] private BoxType _type;

    public BoxType Type { get { return _type; } }

    [SerializeField,Tooltip("Tiempo de vida")] private float _timeToLife;
    [Tooltip("Contador de tiempo para apagarme y guardarme en el objectPool")] private float _counter;

    private ObjectPool<Box> _objectPool;

    private void Update()
    {
        _counter += Time.deltaTime;

        if (_counter >= _timeToLife)
        {
            _objectPool.StockAdd(this);
        }
    }

    public void AddReference(ObjectPool<Box> objPool) => _objectPool = objPool;

    //Son estaticos para que no necesite pasar la referencia del script directamente!
    public static void TurnOff(Box box) =>  box.gameObject.SetActive(false);

    public static void TurnOn(Box box)
    {
        box._counter = 0;
        box.gameObject.SetActive(true);
    }

}
