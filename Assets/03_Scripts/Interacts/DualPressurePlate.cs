using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualPressurePlate : MonoBehaviour, IInteractable
{
    [Header("PARAMETERS")]
    [Space(5)]
    [SerializeField,Tooltip("Colocar la otra placa de presion en la cual va a estar vinculada")] private DualPressurePlate _otherDualPressurePlate;
    [SerializeField, Tooltip("Objeto al que se le va a ejecutar una acción")] private GameObject _objectToInteract;

    private bool _pressed;
    public bool Pressed { get { return _pressed; } }

    public void Desinteract()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        
    }
}
