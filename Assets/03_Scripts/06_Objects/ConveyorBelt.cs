using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : Mechanism
{
    private Renderer _myRenderer;
    [SerializeField] private List<Rigidbody> _rbEntities;

    [Header("-> Freeze Position Box")]
    [SerializeField] private bool _axisX;
    [SerializeField] private bool _axisY;
    [SerializeField] private bool _axisZ;

    [Space(10), SerializeField] private bool _isActive = true;
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


        if (_rbEntities.Count > 0)
        {
            for (int i = 0; i < _rbEntities.Count; i++)
            {
                if (_rbEntities[i].gameObject.activeInHierarchy) _rbEntities[i].velocity += transform.forward * speed;

                else _rbEntities.Remove(_rbEntities[i]);
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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>()) _rbEntities.Add(collision.gameObject.GetComponent<Rigidbody>());

        if (collision.gameObject.GetComponentInParent<Box>())
        {
            var rbBox = collision.gameObject.GetComponentInParent<Box>().GetComponent<Rigidbody>();
            rbBox.drag = 20;

                                        //|= Esa concatenacion ase que se AGREGUE y no se pise
            if (_axisX) rbBox.constraints |= RigidbodyConstraints.FreezePositionX;
            if (_axisY) rbBox.constraints |= RigidbodyConstraints.FreezePositionY;
            if (_axisZ) rbBox.constraints |= RigidbodyConstraints.FreezePositionZ;

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
            _rbEntities.Remove(collision.gameObject.GetComponent<Rigidbody>());

            //_character.ActualMove -= MovimientoCinta;
        }

        if (collision.gameObject.GetComponentInParent<Box>()) _rbEntities.Remove(collision.gameObject.GetComponent<Box>().GetComponent<Rigidbody>());


        if (collision.gameObject.GetComponent<Characters>()) _character = null;
    }
}
