using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("Field of view")]
    public float newFov;
    float _oldFov;

    public Camera cameraA, cameraB;
    Vector3 _cameraApos, _cameraBpos;
    Quaternion _cameraArot, _cameraBrot;
    //[SerializeField] LayerMask playerLayer;

    bool _camera2D;

    private void Start()
    {
        cameraA = Camera.main;
        _oldFov = cameraA.fieldOfView;
    }

    private void Update()
    {
        if(!_camera2D)
        {
            if (cameraA.fieldOfView > _oldFov)
            {
                cameraA.fieldOfView-=0.05f;

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            _camera2D = true;
            if(cameraA.fieldOfView<newFov)
            {
                cameraA.fieldOfView+=0.5f;
            }
            
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
            _camera2D = false;
    }

    

}
