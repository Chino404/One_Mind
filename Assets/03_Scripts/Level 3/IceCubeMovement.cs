using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeMovement : Rewind, IInteracteable
{
    [SerializeField, Tooltip("Segunso que va a esperar para moverse otra vez")] float _secondsWaiting = 1f;
    [SerializeField, Tooltip("Puntos a los que va a ir")] Transform[] _waypoints;
    [SerializeField, Tooltip("Velocidad")] private float _maxVelocity = 7f;
    private float _currentVelocity = 7f;

    [Space(10), SerializeField] private bool _isActiveMove = true;
    public bool IsActiveMove { set { _isActiveMove = value; } }


    private int _actualIndex;
    private Vector3 _velocity;


    private Rigidbody _rb;



    public override void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();
        base.Awake();
    }

    private void Start()
    {
        _currentVelocity = _maxVelocity;
    }


    private void FixedUpdate()
    {
        if (!_isActiveMove) return;

        if (Vector3.Distance(_rb.position, _waypoints[_actualIndex].position) <= 1)
        {
            StartCoroutine(WaitSeconds());
            _actualIndex++;

            if (_actualIndex >= _waypoints.Length) _actualIndex = 0;
        }

        _velocity = _waypoints[_actualIndex].position - _rb.position;
        _velocity.Normalize();

        //transform.position += _velocity*_maxVelocity * Time.deltaTime;
        _rb.MovePosition(_rb.position + _velocity * _currentVelocity * Time.fixedDeltaTime);

        //if (_characterInPlataform)
        //    CharacterAttached();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            Active();
        }
    }

    public void Active()
    {
        if (!_isActiveMove) _isActiveMove = true;
    }

    public void Deactive()
    {

    }

    

    IEnumerator WaitSeconds()
    {
        Debug.Log("freno");

        _currentVelocity = 0;

        yield return new WaitForSeconds(_secondsWaiting);
        _currentVelocity = _maxVelocity;

    }



    



    public override void Save()
    {
        _currentState.Rec(transform.position, _isActiveMove, _currentVelocity);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;


        var col = _currentState.Remember();

        transform.position = (Vector3)col.parameters[0];
        _isActiveMove = (bool)col.parameters[1];
        _currentVelocity = (float)col.parameters[2];
        

        //banana = (Transform)col.parameters[1];
        //_isObjectAttached = (bool)col.parameters[2];
    }
}
