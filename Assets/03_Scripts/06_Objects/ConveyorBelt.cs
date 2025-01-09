using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : Mechanism
{
    private Renderer _myRenderer; 

    [SerializeField] private bool _isActive = true;
    [SerializeField] private float _tapeSpeed;

    [Space(10)] public bool reversedSpeed;
    public Color activeColor;
    public Color normalColor;
    public Color reversedColor;

    private Rigidbody _rbEntity;
    private Characters _character;

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
    }

    public override void ActiveMechanism()
    {
        _isActive = true;

        _myRenderer.material.color = activeColor;
    }

    private void MovimientoCinta(Vector3 dir)
    {
        if (!_isActive) return; 

        dir = transform.forward;

        if (_rbEntity)
        {
            if (!reversedSpeed)
            {
                _rbEntity.velocity += dir * _tapeSpeed;
                _myRenderer.material.color = normalColor;
            }
            else
            {
                _rbEntity.velocity -= dir * _tapeSpeed;
                _myRenderer.material.color = reversedColor;
            }

            //_rb.MovePosition(_rb.position + dir * (_tapeSpeed * 0.01f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>()) _rbEntity = collision.gameObject.GetComponent<Rigidbody>();

        if (collision.gameObject.GetComponent<Characters>())
        {
            _character = collision.gameObject.GetComponent<Characters>();

            _character.ActualMove += MovimientoCinta;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            _rbEntity = null;

            _character.ActualMove -= MovimientoCinta;
        }

        if (collision.gameObject.GetComponent<Characters>()) _character = null;
    }
}
