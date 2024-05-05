using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CameraSwitch : MonoBehaviour
{
    [Header("Field of view")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera = null;
    //public float newFov;
    //float _oldFov;

    //public Camera cameraA, cameraB;
    //Vector3 _cameraApos, _cameraBpos;
    //Quaternion _cameraArot, _cameraBrot;
    //public ModelMonkey monkey;

    //[SerializeField] LayerMask playerLayer;

    public static bool _camera2D;


    private void Start()
    {
        //_camera2D = false;
        //cameraA = Camera.main;
        //_oldFov = cameraA.fieldOfView;

        _virtualCamera.enabled = false;
    }

    private void Update()
    {
        //if(!_camera2D)
        //{
        //    if (cameraA.fieldOfView > _oldFov)
        //    {
        //        cameraA.fieldOfView-=0.05f;

        //    }
        //}


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ModelMonkey>())
            _virtualCamera.enabled = true;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    Vector3 newDirRaw = Vector3.zero;
    //    Vector3 newDir = Vector3.zero;

    //    newDirRaw.x = Input.GetAxisRaw("Horizontal");
    //    newDir.x = Input.GetAxis("Horizontal");
        
    //    if (other.gameObject.layer == 3)
    //    {
    //        _camera2D = true;
    //        if(cameraA.fieldOfView<newFov)
    //        {
    //            cameraA.fieldOfView+=0.5f;
    //        }
    //        monkey.Movement(newDirRaw, newDir);
            
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.layer == 3)
        //    _camera2D = false;

        if (other.gameObject.GetComponent<ModelMonkey>())
            _virtualCamera.enabled = false;
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }



}
