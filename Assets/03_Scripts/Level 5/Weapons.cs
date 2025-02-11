using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    protected bool _isGrabbed;
    private Transform _player;
    [SerializeField] private Vector3 _positionInCharacter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            _isGrabbed = true;
            _player = other.transform;
            transform.position = _player.position + _positionInCharacter;
            //transform.forward = _player.forward;
            transform.SetParent(_player.transform);
        }

    }

    //private void Update()
    //{
    //    if (_isGrabbed)
    //    {
           
    //    }
            
    //}

}
