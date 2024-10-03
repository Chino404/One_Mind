using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    Activar,
    Desactivar,
    Ambas
}

public class ActiveObject : MonoBehaviour, IInteracteable
{
    [SerializeField, Tooltip("Acción a realizar")] private Action _action;

    [SerializeField, Tooltip("Objetos a activar")] private GameObject[] _objectsToActive;
    [SerializeField, Tooltip("Objetos a desactivar")] private GameObject[] _objectsToDeactive;

    private void Awake()
    {
        for (int i = 0; i < _objectsToActive.Length; i++)
        {
            if (_objectsToActive[i].activeInHierarchy) _objectsToActive[i].SetActive(false);
        }
    }

    public void Active()
    {
        if (_action == Action.Activar) ActiveObjects();
        else if (_action == Action.Desactivar) DeactiveObjects();
    }

    private void ActiveObjects()
    {
        for (int i = 0; i < _objectsToActive.Length; i++)
        {
            _objectsToActive[i].SetActive(true);
        }
    }

    private void DeactiveObjects() 
    {
        for (int i = 0; i < _objectsToDeactive.Length; i++)
        {
            _objectsToDeactive[i].SetActive(false);
        }
    }

    public void Deactive()
    {

    }
}
