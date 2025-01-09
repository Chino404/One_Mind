using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : Mechanism
{
    private Renderer _myRenderer; 
    [SerializeField] private List<Rigidbody> _rbEntity;

    [SerializeField] private bool _isActive = true;
    [Space(10), SerializeField] private float _tapeSpeed;
    public bool reversedSpeed;

    [Space(10)]
    public Color normalColor;
    public Color reversedColor;

    private Characters _character;

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
    }

    public override void ActiveMechanism()
    {
        _isActive = true;
    }

    private void FixedUpdate()
    {
        if(!_isActive) return;

        float speed = 0;

        if (!reversedSpeed)
        {
            _myRenderer.material.color = normalColor;
            speed = _tapeSpeed;
        }
        else
        {
            _myRenderer.material.color = reversedColor;
            speed = _tapeSpeed * -1;
        }


        if (_rbEntity.Count > 0)
        {
            for (int i = 0; i < _rbEntity.Count; i++)
            {
                _rbEntity[i].velocity += transform.forward * speed;
            }
        }
    }

    private void MovimientoCinta(Vector3 dir)
    {
        if (!_isActive) return; 

        dir = transform.forward;

        if (_character)
        {
            if (!reversedSpeed)
            {
                //_rbEntity.velocity += dir * _tapeSpeed;
                _myRenderer.material.color = normalColor;
            }
            else
            {
                //_rbEntity.velocity -= dir * _tapeSpeed;
                _myRenderer.material.color = reversedColor;
            }

            //_rb.MovePosition(_rb.position + dir * (_tapeSpeed * 0.01f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            //_rbEntity = collision.gameObject.GetComponent<Rigidbody>();

            _rbEntity.Add(collision.gameObject.GetComponent<Rigidbody>());
        }

        if (collision.gameObject.GetComponent<Characters>())
        {
            _character = collision.gameObject.GetComponent<Characters>();

            //_character.ActualMove += MovimientoCinta;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            //_rbEntity = null;
            _rbEntity.Remove(collision.gameObject.GetComponent<Rigidbody>());

            //_character.ActualMove -= MovimientoCinta;
        }

        if (collision.gameObject.GetComponent<Characters>()) _character = null;
    }
}
