using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CameraSwitch : MonoBehaviour
{
    [Header("Componet")]
    private CameraTracker _tracker;
    private CameraRails _rail;
    public CharacterTarget myCharacterTarget;
    [Tooltip("Hacia donde se va a mover la cámara")]public Transform goTo;
    [SerializeField] private bool _backToPosition;

    [/*Space(15),SerializeField, */Tooltip("(EN EL CASO QUE NO VUELVA) Punto especifo a cual volver")] private Transform _pointBack;
    private Transform _backTo;

    private void Start()
    {
        if (myCharacterTarget == CharacterTarget.Bongo) _tracker = GameManager.instance.bongoNormalCamera;
        else if (myCharacterTarget == CharacterTarget.Frank) _tracker = GameManager.instance.frankNormalCamera;

        //if (!_tracker)Debug.LogError($"No se asigno ninguna cámara en: {gameObject.name}");

        if (myCharacterTarget == CharacterTarget.Bongo) _rail = GameManager.instance.bongoRailsCamera;
        else if (myCharacterTarget == CharacterTarget.Frank) _rail = GameManager.instance.frankRailsCamera;

        if (!_rail)Debug.LogError($"No se asigno ninguna cámara en: {gameObject.name}");
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
            _rail.isNormalCamera = true;

            //if(_backToPosition && !_pointBack) _backTo = _tracker.Point;

            if(_backToPosition && !_pointBack) _backTo = _rail.point;

            //_tracker.TransicionPoint(goTo);
            _rail.TransicionPoint(goTo);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Characters>() && _backToPosition)
        {

            _rail.isNormalCamera = false;

            if(_pointBack)
            {
                //_tracker.TransicionPoint(_pointBack);

                _rail.TransicionPoint(_pointBack);
                return;
            }

            //_tracker.TransicionPoint(_backTo);
            _rail.TransicionPoint(_pointBack);
            _backTo = null;
        }
    }

}
