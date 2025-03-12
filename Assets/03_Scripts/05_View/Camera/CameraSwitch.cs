using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("Componet")]
    [SerializeField] private CameraRails _myRefCamera;

    [Space(10)]
    public CharacterTarget myCharacterTarget;

    public enum TransitionType {Goto, BackTo, Both }
    [HideInInspector] public TransitionType myTransitiontype;

    [HideInInspector, Tooltip("Hacia donde se va a mover la cámara")] public Transform goToNode;

    [HideInInspector] public bool isChangeNewCamera;
    [HideInInspector] public CameraRails newCamera;

    [HideInInspector] public bool isBackToNewNode = false;
    [HideInInspector] public Transform newToNode;


    //private void Start() => CamerasManager.instance.getCamera += SetMyRefCamera;
    //private void SetMyRefCamera(CameraRails newCamera) => _myRefCamera = newCamera;

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.GetComponent<Characters>())
        {
            if (!goToNode && myTransitiontype != TransitionType.BackTo)
            {
                Debug.LogError($"Poner un 'point' para la cámara en: {gameObject.name}");

                return;
            }

            if (isChangeNewCamera && newCamera != null) CamerasManager.instance.ActiveCamera(newCamera, myCharacterTarget);

            _myRefCamera = CamerasManager.instance.GetCurrentCamera(myCharacterTarget);

            //Si es un Goto o Both, lo mado a un nodo fijo
            if (myTransitiontype != TransitionType.BackTo)
            {
                _myRefCamera.TransitionToAFixedNode(goToNode);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Characters>() && myTransitiontype != TransitionType.Goto)
        {
            //Si tengo un punto especifico a cuál mandarlo, le asigno ese nodo.
            if (isBackToNewNode && newToNode != null)
            {
                _myRefCamera.TransitionToRail(newToNode);
                return;
            }

            _myRefCamera.TransitionToRail();

        }
    }
}
