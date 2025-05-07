using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("Componet")]
    [SerializeField,Tooltip("¡¡No poner nada acá!!")] private CameraRails _myRefCamera;

    [Space(10)]
    public CharacterTarget myCharacterTarget;

    public enum TransitionType {None, GoToFixedNode, BackToRail, Both}
    [HideInInspector] public TransitionType myTransitiontype;

    [HideInInspector, Tooltip("Hacia donde se va a mover la cámara")] public Transform goToNode;

    [HideInInspector] public bool isChangeNewCamera;
    [HideInInspector] public CameraRails newCamera;

    [HideInInspector] public bool isBackToNewNode = false;
    [HideInInspector] public Transform newToNode;
    private ChangeTheCameraRail _myRefChangeCameraRail;


    //private void Start() => CamerasManager.instance.getCamera += SetMyRefCamera;
    //private void SetMyRefCamera(CameraRails newCamera) => _myRefCamera = newCamera;

    private void Awake()
    {
        _myRefChangeCameraRail = gameObject.GetComponent<ChangeTheCameraRail>() ? gameObject.GetComponent<ChangeTheCameraRail>() : null;
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.GetComponent<Characters>())
        {
            if (!goToNode && myTransitiontype == TransitionType.GoToFixedNode)
            {
                Debug.LogError($"Poner un 'point' para la cámara en: {gameObject.name}");

                return;
            }

            if (isChangeNewCamera && newCamera != null)
            {
                //Si la cámara que quiero activar es la misma que ya está activada, entonces no sigo.
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
                //Verifico si volví a entrar al mismo collider para que no vaya de nuevo al nodo.
                if(_myRefChangeCameraRail && _myRefCamera.RefRail == _myRefChangeCameraRail.newRail)  return;

                _myRefCamera.TransitionToRail(newToNode);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Characters>() && myTransitiontype == TransitionType.Both)
        {
            //Si tengo un punto especifico a cuál mandarlo, le asigno ese nodo.
            if (isBackToNewNode && newToNode != null)
            {
                _myRefCamera.TransitionToRail(newToNode);
                return;
            }

            _myRefCamera.TransitionToRail();

        }

        //if (other.gameObject.GetComponent<Characters>() && myTransitiontype != TransitionType.GoToFixedNode && myTransitiontype != TransitionType.None)
        //{
        //    //Si tengo un punto especifico a cuál mandarlo, le asigno ese nodo.
        //    if (isBackToNewNode && newToNode != null)
        //    {
        //        _myRefCamera.TransitionToRail(newToNode);
        //        return;
        //    }

        //    _myRefCamera.TransitionToRail();

        //}
    }
}
