using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModeSwitchCamera
{
    Normal,
    Rail
}

[RequireComponent(typeof(BoxCollider))]
public class OldCameraSwitch : MonoBehaviour
{
    [Header("Componet")]
    private CameraTracker _tracker;
    private CameraRails _myRail;

    public ModeSwitchCamera modeSwitchCamera = ModeSwitchCamera.Normal;

    [Space(10)]
    public CharacterTarget myCharacterTarget;

    [Tooltip("Hacia donde se va a mover la cámara")] public Transform goTo;
    [SerializeField] private bool _backToPosition;

    [/*Space(15), SerializeField, */Tooltip("(EN EL CASO QUE NO VUELVA) Punto especifo a cual volver")] private Transform _pointBack;
    private Transform _backTo;

    private void Start()
    {
        if(modeSwitchCamera == ModeSwitchCamera.Normal)
        {
            if (myCharacterTarget == CharacterTarget.Bongo) _tracker = GameManager.instance.bongoNormalCamera;
            else if (myCharacterTarget == CharacterTarget.Frank) _tracker = GameManager.instance.frankNormalCamera;

            if (!_tracker)Debug.LogError($"No se asigno ninguna cámara en: {gameObject.name}");
        }

        else
        {
            if (myCharacterTarget == CharacterTarget.Bongo) _myRail = GameManager.instance.bongoRailsCamera;
            else if (myCharacterTarget == CharacterTarget.Frank) _myRail = GameManager.instance.frankRailsCamera;

            if (!_myRail)Debug.LogError($"No se asigno ninguna cámara en: {gameObject.name}");
        }
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
            if(modeSwitchCamera == ModeSwitchCamera.Normal)
            {
                if (_backToPosition && !_pointBack)
                {
                    _backTo = _tracker.Point;
                    _tracker.TransicionPoint(goTo);
                }
            }

            else _myRail.TransitionToAFixedNode(goTo);

        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Characters>() && _backToPosition)
        {
            if(_pointBack)
            {
                if (modeSwitchCamera == ModeSwitchCamera.Normal) _tracker.TransicionPoint(_pointBack);
                return;
            }

            if (modeSwitchCamera == ModeSwitchCamera.Normal)
            {
                _tracker.TransicionPoint(_backTo);
                _backTo = null;
                return;
            }

            _myRail.TransitionToRail();

        }
    }

}
