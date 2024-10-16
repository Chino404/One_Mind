using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CameraSwitch : MonoBehaviour
{
    [Header("Componet")]
    private CameraTracker _tracker;
    public CharacterTarget myCharacterTarget;
    [Tooltip("Hacia donde se va a mover la cámara")]public Transform goTo;

    [SerializeField] private bool _backToPosition;
    [SerializeField, Tooltip("Punto especifo a cual volver")] private Transform _pointBack;
    private Transform _backTo;

    private void Start()
    {
        if (myCharacterTarget == CharacterTarget.Bongo) _tracker = GameManager.instance.bongoCamera;
        else if (myCharacterTarget == CharacterTarget.Frank) _tracker = GameManager.instance.frankCamera;

        if(!_tracker)Debug.LogError($"No se asigno ninguna cámara en: {gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!goTo)
        {
            Debug.LogError($"Poner un 'point' para la cámara en: {gameObject.name}");

            return;
        }

        if(other.gameObject.GetComponent<Characters>())
        {
            if(_backToPosition && !_pointBack) _backTo = _tracker.Point;

            _tracker.TransicionPoint(goTo);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Characters>() && _backToPosition)
        {
            if(_pointBack)
            {
                _tracker.TransicionPoint(_pointBack);
                return;
            }

            _tracker.TransicionPoint(_backTo);
            _backTo = null;
        }
    }

}
