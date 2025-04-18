using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    Activar,
    Desactivar,
    Ambas
}

public class ActiveObject : Rewind, IInteracteable
{
    [SerializeField, Tooltip("Acción a realizar")] private Action _action;

    [SerializeField, Tooltip("Objetos a activar")] private GameObject[] _objectsToActive;
    [SerializeField, Tooltip("Objetos a desactivar")] private GameObject[] _objectsToDeactive;

    public bool desactivarCuandoSalgo;

    private void Start()
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
        if(desactivarCuandoSalgo)
        {
            for (int i = 0; i < _objectsToDeactive.Length; i++)
            {
                _objectsToDeactive[i].SetActive(false);
            }
            gameObject.SetActive(false);
        }
    }

    public override void Save()
    {
        foreach (var item in _objectsToDeactive)
        {
            if(item == null)
            {
                Debug.Log($"Variable <color=yellow>{gameObject.name}</color> no asignada!");
                continue;
            }

            _currentState.Rec(item.activeInHierarchy);
        }
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;
        var col = _currentState.Remember();

        foreach (var item in _objectsToDeactive)
        {
            item.SetActive((bool)col.parameters[0]);
        }
    }

    //public override void Save()
    //{
    //    //foreach (var item in _objectsToActive)
    //    //{
    //    //    _currentState.Rec(item.activeInHierarchy);
    //    //}
    //}

    //public override void Load()
    //{
    //    //if(!_currentState.IsRemember()) return;

    //    //var col = _currentState.Remember();
    //    //gameObject.SetActive((bool)col.parameters[0]);
    //}
}
