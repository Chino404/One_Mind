using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : Rewind, IInteractable
{

    private Rigidbody _rb;

    private bool _isObjectAttached;
    
    [SerializeField] Transform banana; 
    public ModelBanana modelBanana;

    private Ray _moveRay;
    private float _moveRange=0.75f;
    [SerializeField]private LayerMask _moveMask;

    public override void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        base.Awake();
    }

    void FixedUpdate()
    {
        //if (IsBlocked(modelBanana.Velocity)) modelBanana.Velocity = Vector3.zero;

        if (_isObjectAttached && banana!=null)
        {
            _rb.MovePosition(transform.position + modelBanana.Velocity * Time.fixedDeltaTime);
        }
    }

    private bool IsBlocked(Vector3 dir)
    {
        _moveRay = new Ray(transform.position, dir);
        Debug.DrawRay(transform.position, dir * _moveRange, Color.red);

        return Physics.Raycast(_moveRay, _moveRange, _moveMask);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ModelMonkey>())
            collision.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<ModelMonkey>())
            collision.transform.SetParent(null);
    }

    public void LeftClickAction()
    {
        
    }

    public void RightClickAction(Transform parent)
    {        
        if (!_isObjectAttached)
        {
            //transform.SetParent(parent);
            banana = parent;
            _isObjectAttached = true;
        }
        else if (_isObjectAttached)
        {
            ReleaseObject();
        }
    }

    public void ReleaseObject()
    {
        transform.SetParent(null);
        banana = null;
        _isObjectAttached = false;
    }

    public override void Save()
    {
        _currentState.Rec(transform.position,banana,_isObjectAttached);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        transform.position = (Vector3)col.parameters[0];
        banana = (Transform)col.parameters[1];
        _isObjectAttached = (bool)col.parameters[2];
    }
}
