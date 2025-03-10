using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("Componet")]
    private CameraRails _myRail;

    [Space(10)]
    public CharacterTarget myCharacterTarget;

    public enum TransitionType {Goto, BackTo, Both }
    [HideInInspector] public TransitionType myTransitiontype;

    [HideInInspector, Tooltip("Hacia donde se va a mover la cámara")] public Transform goTo;
    [HideInInspector] public bool backToPosition = true;


    private void Start()
    {
        if (myCharacterTarget == CharacterTarget.Bongo) _myRail = GameManager.instance.bongoRailsCamera;
        else if (myCharacterTarget == CharacterTarget.Frank) _myRail = GameManager.instance.frankRailsCamera;

        if (!_myRail) Debug.LogError($"No se asigno ninguna cámara en: {gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!goTo)
        {
            Debug.LogError($"Poner un 'point' para la cámara en: {gameObject.name}");

            return;
        }

        if (other.gameObject.GetComponent<Characters>() && myTransitiontype == TransitionType.Goto)
        {
            _myRail.TransitionToAFixedNode(goTo);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Characters>() && backToPosition && myTransitiontype == TransitionType.BackTo || myTransitiontype == TransitionType.Both)
        {

            _myRail.TransitionToRail();

        }
    }
}
