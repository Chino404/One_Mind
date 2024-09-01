using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPressurePlate : MonoBehaviour, IInteractable
{
    [Header("OBJECTS TO...")]
    [Space(5),SerializeField] private GameObject[] _active;
    [SerializeField] private GameObject[] _deactive;

    private void Start()
    {
        for (int i = 0; i < _active.Length; i++)
        {
            if (_active[i].activeInHierarchy) _active[i].gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        for (int i = 0; i < _active.Length; i++)
        {
            _active[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < _deactive.Length; i++)
        {
            _deactive[i].gameObject.SetActive(false);
        }
    }

    public void Desinteract()
    {
        throw new System.NotImplementedException();
    }
}
