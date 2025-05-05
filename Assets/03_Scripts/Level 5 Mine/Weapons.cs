using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [Range(0,1)]public int mouseButton;
    protected bool _isGrabbed;
    protected bool _isAttacking;
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
    private void Update()
    {
        if (_isGrabbed == true)
        {
            if (Input.GetMouseButton(mouseButton)&&_isAttacking==false)
                Attack();
        }
    }
    public virtual void Attack()
    {

    }

    //private void Update()
    //{
    //    if (_isGrabbed)
    //    {
           
    //    }
            
    //}

}
