using System;
using UnityEngine;

[Serializable]
public class BoxData
{
    public BoxType boxType;
    public GameObject prefab;
}

public enum BoxType
{
    Normal,
    Alta,
    Ancha
}

public class Box : MonoBehaviour
{
    public BoxData[] _prefabsBoxes;

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

    public void ChangeBox(BoxType type)
    {
        foreach (var item in _prefabsBoxes)
        {
            if (item.boxType == type)
            {
                item.prefab.gameObject.SetActive(true);
                break;
            }
        }
    }

    //Son estaticos para que no necesite pasar la referencia del script directamente!
    public static void TurnOff(Box box)
    {
        foreach (var item in box._prefabsBoxes)
        {
            item.prefab.SetActive(false);
        }      

        box.gameObject.SetActive(false);
    }

    public static void TurnOn(Box box)
    {
        box._counter = 0;
        box.gameObject.SetActive(true);
    }


}

