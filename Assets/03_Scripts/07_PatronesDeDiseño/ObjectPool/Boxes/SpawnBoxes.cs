using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoxes : MonoBehaviour
{
    [SerializeField] private Box[] _boxesModel;
    [Tooltip("Segundos de cooldown")][SerializeField] float _coolDown;
    [Tooltip("cantidad de cajas que instancio al principio")][SerializeField] int _boxesQuantity;

    //private BoxesFactory _factory;
    private Factory<Box> _factory;
    private ObjectPool<Box> _objectPool;
    private int _indexBox;

    private void Start()
    {
        _indexBox = Random.Range(0, _boxesModel.Length);

        foreach (var item in _boxesModel)
        {
            _factory = new BoxesFactory(item);

            _objectPool = new ObjectPool<Box>(_factory.GetObj, Box.TurnOff, Box.TurnOn, _boxesQuantity);
        }

        StartCoroutine(CreateBoxes());
    }

    IEnumerator CreateBoxes()
    {
        while (true)
        {


            yield return null;
        }
    }
}
