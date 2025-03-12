using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Connected, IInteracteable
{
    private int _actualIndex;
    private Vector3 _velocity;
    private Rigidbody _rb;

    [Space(10)]
    [SerializeField, Tooltip("Puntos a los que va a ir")] Transform[] _waypoints;
    public GameObject door;

    [SerializeField, Tooltip("Velocidad")] private float _maxVelocity = 9f;
    private float _currentVelocity = 7f;
    [SerializeField, Range(0, 8f) ,Tooltip("Segunso que va a esperar para moverse otra vez")] float _secondsWaiting = 5f;


    [Space(10), SerializeField] private bool _isActiveElevator = true;
    public bool IsActiveElevator { set { _isActiveElevator = value; } }

    public override void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();
        
        base.Awake();
    }

    private void Start()
    {
        door.gameObject.SetActive(false);

        _currentVelocity = _maxVelocity;

    }

    private void FixedUpdate()
    {
        if (!_isActiveElevator) return;

        if (Vector3.Distance(_rb.position, _waypoints[_actualIndex].position) <= 1)
        {
            StartCoroutine(WaitSeconds());
            _actualIndex++;

            if (_actualIndex >= _waypoints.Length) _actualIndex = 0;
        }

        _velocity = _waypoints[_actualIndex].position - _rb.position;
        _velocity.Normalize();


        _rb.MovePosition(_rb.position + _velocity * _currentVelocity * Time.fixedDeltaTime);

    }

    IEnumerator WaitSeconds()
    {
        Debug.Log("freno");

        _currentVelocity = 0;
        door.gameObject.SetActive(false);

        yield return new WaitForSeconds(_secondsWaiting);

        door.gameObject.SetActive(true);
        _currentVelocity = _maxVelocity;
    }

    public void Active()
    {
        _isActive = true;

        if (_connectedObject.IsActive)
        {
            door.gameObject.SetActive(true);
            _connectedObject.gameObject.GetComponent<Elevator>().door.gameObject.SetActive(true);

            _isActiveElevator = true;
            _connectedObject.gameObject.GetComponent<Elevator>().IsActiveElevator = true;
        }
        else
            Debug.LogWarning("No esta activado el otro objeto");
    }

    public void Deactive()
    {
        _isActive = false;
    }

    public override void Save()
    {
        //_currentState.Rec(transform.position, _isActiveMove /*_currentVelocity*/);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;


        var col = _currentState.Remember();

        //transform.position = _waypoints[_actualIndex].position;
        //StartCoroutine(WaitSeconds());
        //_isActiveMove = (bool)col.parameters[1];
    }
}
