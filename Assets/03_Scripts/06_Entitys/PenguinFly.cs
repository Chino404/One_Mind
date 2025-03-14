using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinFly : MonoBehaviour, IInteracteable
{
    private Vector3 _newPosition;
    [SerializeField] private Vector3 _positionInBongo;

    [SerializeField]private bool _isInBongo;
    private bool _isDisable;
    [SerializeField]private Animator _animator;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_isInBongo && !_isDisable)
        {         
            transform.position = GameManager.instance.modelBongo.gameObject.transform.position + _positionInBongo;
            transform.forward = GameManager.instance.modelBongo.gameObject.transform.forward;
        }

        else if (_isDisable)
        {
            GameManager.instance.modelBongo.FlyPenguin(false);
            GameManager.instance.modelBongo.IsGetPenguin = false;
            gameObject.SetActive(false);
            GameManager.instance.modelBongo.penguin = null;
        }
    }

    public void Active()
    {
        if (_isDisable) return;

        _isInBongo = true;
        GameManager.instance.modelBongo.penguin = this;
        GameManager.instance.modelBongo.IsGetPenguin = true;
    }

    public void Deactive()
    {
           
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 16) _isDisable = true;
    }

    public void Fly()
    {
        _animator.SetTrigger("Flying");
    }

    public void StopFlying()
    {
        _animator.SetTrigger("Normal");
    }
}
