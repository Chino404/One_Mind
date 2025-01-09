using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float _tapeSpeed;
    [SerializeField] private Rigidbody _rb;
    private Characters _character;


    private void MovimientoCinta(Vector3 dir)
    {
        dir = transform.forward;

        if (_rb)
        {
            _rb.MovePosition(_rb.position + dir * (_tapeSpeed * 0.01f));

            //_rb.velocity = 
            //_rb.velocity += transform.forward * _speed * Time.fixedDeltaTime;

            //_character.Movement(dir, _tapeSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>()) _rb = collision.gameObject.GetComponent<Rigidbody>();

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
            _rb = null;

            _character.ActualMove -= MovimientoCinta;
        }

        if (collision.gameObject.GetComponent<Characters>()) _character = null;
    }
}
