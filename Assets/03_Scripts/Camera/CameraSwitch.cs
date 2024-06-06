using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CameraSwitch : MonoBehaviour
{
    [Header("Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera _actualVC;
    [SerializeField] private CinemachineVirtualCamera _newVC = null;

    public static bool _camera2D;


    private void Start()
    {
        _newVC.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ModelMonkey>())
        {
            _newVC.enabled = true;
            _actualVC.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if(other.gameObject.GetComponent<ModelMonkey>())
        {
            _newVC.enabled = false;
            _actualVC.enabled = true;
        }
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }



}
