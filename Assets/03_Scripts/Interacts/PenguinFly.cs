using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinFly : MonoBehaviour, IInteracteable
{
    private Vector3 _newPosition;
    [SerializeField] private Vector3 _positionInBongo;

    [SerializeField]private bool _isInBongo;
    private bool _isDisable;

    private void Update()
    {
        if(_isInBongo && !_isDisable) transform.position = GameManager.instance.bongo.gameObject.transform.position + _positionInBongo;

        else if (_isDisable)
        {
            GameManager.instance.bongo.Glide(false);
            GameManager.instance.bongo.IsGetPenguin = false;
            gameObject.SetActive(false);
        }
    }

    public void Active()
    {
        if (_isDisable) return;

        _isInBongo = true;

        GameManager.instance.bongo.IsGetPenguin = true;
    }

    public void Deactive()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 16) _isDisable = true;
    }
}
