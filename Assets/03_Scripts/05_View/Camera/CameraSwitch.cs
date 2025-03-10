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

    [HideInInspector, Tooltip("Hacia donde se va a mover la cámara")] public Transform goToNode;

    [HideInInspector] public bool isBackToNewNode = false;
    [HideInInspector] public Transform newToNode;


    private void Start()
    {
        if (myCharacterTarget == CharacterTarget.Bongo) _myRail = GameManager.instance.bongoRailsCamera;
        else if (myCharacterTarget == CharacterTarget.Frank) _myRail = GameManager.instance.frankRailsCamera;

        if (!_myRail) Debug.LogError($"No se asigno ninguna cámara en: {gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!goToNode && myTransitiontype != TransitionType.BackTo)
        {
            Debug.LogError($"Poner un 'point' para la cámara en: {gameObject.name}");

            return;
        }

        if (other.gameObject.GetComponent<Characters>() && myTransitiontype != TransitionType.BackTo)
        {
            _myRail.TransitionToAFixedNode(goToNode);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Characters>() && myTransitiontype != TransitionType.Goto)
        {
            if (isBackToNewNode && newToNode != null)
            {
                _myRail.TransitionToRail(newToNode);
                return;
            }

            _myRail.TransitionToRail();

        }
    }
}
