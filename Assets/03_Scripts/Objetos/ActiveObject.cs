using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObject : MonoBehaviour, IInteracteable
{
    [SerializeField, Tooltip("Objetos a activar")] private GameObject[] _objectsToActive;

    private void Awake()
    {
        for (int i = 0; i < _objectsToActive.Length; i++)
        {
            if (_objectsToActive[i].activeInHierarchy) _objectsToActive[i].SetActive(false);
        }
    }

    public void Interact()
    {
        for (int i = 0; i < _objectsToActive.Length; i++)
        {
            _objectsToActive[i].SetActive(true);
        }
    }

    public void Disconnect()
    {

    }
}
