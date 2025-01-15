using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Tooltip("Contador de tiempo para instanciar una caja")] private float _counter;
    private ObjectPool<Box> _objectPool;

    public void AddReference(ObjectPool<Box> op) => _objectPool = op;

    public static void TurnOff(Box box) =>  box.gameObject.SetActive(false);

    public static void TurnOn(Box box)
    {
        box._counter = 0;
        box.gameObject.SetActive(true);
    }
}
