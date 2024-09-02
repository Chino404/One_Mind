using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualPressurePlate : MonoBehaviour, IPress
{
    [Header("PARAMETERS")]
    [Space(5)]
    [SerializeField,Tooltip("Colocar la otra placa de presion en la cual va a estar vinculada")] private DualPressurePlate _otherDualPressurePlate;
    [SerializeField, Tooltip("Objeto al que se le va a ejecutar una acción")] private GameObject _objectToInteract;

    private bool _active;
    public bool Active { get { return _active; } }

    private void Start()
    {
        if (_otherDualPressurePlate == null) Debug.LogWarning($"Falta referencia de la otra placa de presión en: {gameObject.name}");
        if (_objectToInteract == null) Debug.LogWarning($"Falta objeto para interactuar en: {gameObject.name}");
    }

    public void Pressed()
    {
        _active = true;

        if (_otherDualPressurePlate != null && _otherDualPressurePlate.Active)
        {
            _otherDualPressurePlate.ActionDualPressurePlate();

            ActionDualPressurePlate();
        }
    }

    public void ActionDualPressurePlate()
    {
        if(_objectToInteract != null)_objectToInteract.gameObject.SetActive(false);
    }

    public void Depressed()
    {
        _active = false;
    }
}
