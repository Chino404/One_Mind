using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualPressurePlate : MonoBehaviour, IPress
{
    [Header("PARAMETERS")]
    [Space(5)]
    [SerializeField,Tooltip("Colocar la otra placa de presion en la cual va a estar vinculada")] private DualPressurePlate _otherDualPressurePlate;
    [SerializeField, Tooltip("Puerta al que se le va a ejecutar una acci�n")] private DualDoor _objectToInteract;
    [SerializeField, Tooltip("Objetos que sirven para indicar que esta placa de presion fue activada")] private Light[] _indicators;

    private bool _active;
    public bool Active { get { return _active; } }

    private bool _actionCompleted = false;
    [SerializeField] GameObject _normalPlaque;
    [SerializeField] GameObject _pressedPlaque;

    

    private void Awake()
    {
        if (_otherDualPressurePlate == null) Debug.LogWarning($"Falta referencia de la otra placa de presi�n en: {gameObject.name}");
        if (_objectToInteract == null) Debug.LogWarning($"Falta objeto para interactuar en: {gameObject.name}");

        for (int i = 0; i < _indicators.Length; i++)
        {
            _indicators[i].gameObject.SetActive(false);
        }
       
    }


    public void Pressed()
    {
        _active = true;
        
        for (int i = 0; i < _indicators.Length; i++)
        {
            _indicators[i].gameObject.SetActive(true);
            _normalPlaque.SetActive(false);
            _pressedPlaque.SetActive(true);
        }

        if (_otherDualPressurePlate != null && _otherDualPressurePlate.Active)
        {
            _otherDualPressurePlate.ActionDualPressurePlate();

            ActionDualPressurePlate();
        }
    }

    public void ActionDualPressurePlate()
    {

        if (_objectToInteract != null)
        {
            //_objectToInteract.gameObject.SetActive(false);

            _objectToInteract.OpenTheDoor();
            if(!_actionCompleted)
            AudioManager.instance.Play(SoundId.Open_Door);

            _actionCompleted = true;
        }

    }

    public void Depressed()
    {
        if (_actionCompleted) return;

        _active = false;
        

        for (int i = 0; i < _indicators.Length; i++)
        {
            _indicators[i].gameObject.SetActive(false);
            _normalPlaque.SetActive(true);
            _pressedPlaque.SetActive(false);
        }

    }
}
