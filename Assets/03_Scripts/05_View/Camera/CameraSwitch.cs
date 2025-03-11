using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("Componet")]
    private CameraRails _myRefCamera;

    [Space(10)]
    public CharacterTarget myCharacterTarget;

    public enum TransitionType {Goto, BackTo, Both }
    [HideInInspector] public TransitionType myTransitiontype;

    [HideInInspector, Tooltip("Hacia donde se va a mover la cámara")] public Transform goToNode;

    [HideInInspector] public bool isChangeNewCamera;
    [HideInInspector] public CameraRails newCamera;

    [HideInInspector] public bool isBackToNewNode = false;
    [HideInInspector] public Transform newToNode;


    private void Start()
    {
        if (newCamera) _myRefCamera = newCamera;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isChangeNewCamera && newCamera != null)
        {
            CameraRails currentCamera = default;

            //Averiguo que cámara me guardo.
            if (myCharacterTarget == CharacterTarget.Bongo)
            {
                currentCamera = GameManager.instance.bongoRailsCamera;

                GameManager.instance.bongoRailsCamera = newCamera;
            }
            else
            {
                currentCamera = GameManager.instance.frankRailsCamera;

                GameManager.instance.frankRailsCamera = newCamera;
            }

            Transform auxTarget = currentCamera.target;

            //Apago la cámara que está activa.
            if (currentCamera.myCamera.gameObject.activeInHierarchy) currentCamera.myCamera.gameObject.SetActive(false);

            _myRefCamera.target = auxTarget;

            //Activo la que quiero usar ahora.
            if (!_myRefCamera.myCamera.gameObject.activeInHierarchy) _myRefCamera.myCamera.gameObject.SetActive(true);
        }

        else
        {
            if (myCharacterTarget == CharacterTarget.Bongo) _myRefCamera = GameManager.instance.bongoRailsCamera;

            else _myRefCamera = GameManager.instance.frankRailsCamera;

            if (!_myRefCamera)
            {
                Debug.LogError($"No se asigno ninguna cámara en: {gameObject.name}");
                return;
            }
        }


        if (!goToNode && myTransitiontype != TransitionType.BackTo)
        {
            Debug.LogError($"Poner un 'point' para la cámara en: {gameObject.name}");

            return;
        }

       

        if (other.gameObject.GetComponent<Characters>() && myTransitiontype != TransitionType.BackTo)
        {
            _myRefCamera.TransitionToAFixedNode(goToNode);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Characters>() && myTransitiontype != TransitionType.Goto)
        {
            if (isBackToNewNode && newToNode != null)
            {
                _myRefCamera.TransitionToRail(newToNode);
                return;
            }

            _myRefCamera.TransitionToRail();

        }
    }
}
