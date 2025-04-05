using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("Componet")]
    [SerializeField,Tooltip("��No poner nada ac�!!")] private CameraRails _myRefCamera;

    [Space(10)]
    public CharacterTarget myCharacterTarget;

    public enum TransitionType {None, GoToFixedNode, BackToRail, Both}
    [HideInInspector] public TransitionType myTransitiontype;

    [HideInInspector, Tooltip("Hacia donde se va a mover la c�mara")] public Transform goToNode;

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
            if (!goToNode && myTransitiontype == TransitionType.GoToFixedNode)
            {
                Debug.LogError($"Poner un 'point' para la c�mara en: {gameObject.name}");

                return;
            }

            if (isChangeNewCamera && newCamera != null)
            {
                //Si la c�mara que quiero activar es la misma que ya est� activada, entonces no sigo.
                if (newCamera.NumberCamera == CamerasManager.instance.GetCurrentCamera(myCharacterTarget).NumberCamera) return;

                CamerasManager.instance.ActiveCamera(newCamera, myCharacterTarget);
            }

            _myRefCamera = CamerasManager.instance.GetCurrentCamera(myCharacterTarget);

            //Si es un Goto o Both, lo mado a un nodo fijo
            if (myTransitiontype == TransitionType.GoToFixedNode || myTransitiontype == TransitionType.Both)
            {
                _myRefCamera.TransitionToAFixedNode(goToNode);
                return;
            }

            else if(myTransitiontype == TransitionType.BackToRail)
            {
                _myRefCamera.TransitionToRail(newToNode);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Characters>() && myTransitiontype == TransitionType.Both)
        {
            //Si tengo un punto especifico a cu�l mandarlo, le asigno ese nodo.
            if (isBackToNewNode && newToNode != null)
            {
                _myRefCamera.TransitionToRail(newToNode);
                return;
            }

            _myRefCamera.TransitionToRail();

        }

        //if (other.gameObject.GetComponent<Characters>() && myTransitiontype != TransitionType.GoToFixedNode && myTransitiontype != TransitionType.None)
        //{
        //    //Si tengo un punto especifico a cu�l mandarlo, le asigno ese nodo.
        //    if (isBackToNewNode && newToNode != null)
        //    {
        //        _myRefCamera.TransitionToRail(newToNode);
        //        return;
        //    }

        //    _myRefCamera.TransitionToRail();

        //}
    }
}
