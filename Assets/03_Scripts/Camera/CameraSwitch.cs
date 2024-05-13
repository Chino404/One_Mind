using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CameraSwitch : MonoBehaviour
{
    [Header("Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera = null;

    public static bool _camera2D;


    private void Start()
    {
        _virtualCamera.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ModelMonkey>())
        {
            _virtualCamera.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if(other.gameObject.GetComponent<ModelMonkey>())
            _virtualCamera.enabled = false;
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }



}
