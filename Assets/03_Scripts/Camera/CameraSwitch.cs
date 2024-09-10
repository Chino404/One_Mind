using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CameraSwitch : MonoBehaviour
{
    [Header("Componet")]
    [SerializeField]private CameraTracker _tracker;
    [Tooltip("Hacia donde se va a mover la cámara")]public Transform goTo;
    private Transform _backTo;
    [SerializeField] private bool _backToPosition;

    //private void Start()
    //{
    //    _tracker = CameraTracker.Instance;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Characters>())
        {
            if(_backToPosition)_backTo = _tracker.Point;
            _tracker.TransicionPoint(goTo);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Characters>() && _backToPosition)
        {
            _tracker.TransicionPoint(_backTo);
            _backTo = null;
        }
    }

}
