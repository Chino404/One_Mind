using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveThings : Rewind
{

    [Tooltip("Puntos a los que va a ir")] public Transform[] waypoints;
    [SerializeField, Tooltip("Velocidad")] protected float _maxVelocity = 7f;
    [SerializeField, Tooltip("Segunso que va a esperar para moverse otra vez")] protected float _secondsWaiting = 1f;
    protected float _currentVelocity = 7f;

    

    [Space(10), SerializeField] protected bool _isActiveMove = true;
    public bool IsActiveMove { set { _isActiveMove = value; } }


    protected int _actualIndex;
    public int ActualIndex { set { _actualIndex = value; } }
    protected Vector3 _velocity;
    public Vector3 Velocity { get { return _velocity; } }

    protected Rigidbody _rb;
    private Vector3 _startPos;

    

    public override void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();
        
        base.Awake();
    }

    public virtual void Start()
    {

        _currentVelocity = _maxVelocity;
        _startPos = _rb.position;
       
    }

    

   
    public virtual void FixedUpdate()
    {
        if (!_isActiveMove) return;

        if (Vector3.Distance(_rb.position, waypoints[_actualIndex].position) <= 0.5f)
        {
            StartCoroutine(WaitSeconds());
            _actualIndex++;

            if (_actualIndex >= waypoints.Length) _actualIndex = 0;
        }
        
        _velocity = waypoints[_actualIndex].position - _rb.position;
        _velocity.Normalize();

        //transform.position += _velocity*_maxVelocity * Time.deltaTime;
        _rb.MovePosition(_rb.position + _velocity * _currentVelocity * Time.fixedDeltaTime);

        //if (_characterInPlataform)
        //    CharacterAttached();

    }

   

    public virtual IEnumerator WaitSeconds()
    {
        Debug.Log("freno");
        
        _currentVelocity = 0;

        yield return new WaitForSeconds(_secondsWaiting);
        _currentVelocity = _maxVelocity;


        //_velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
    }


    Vector3 Seek(Vector3 target)
    {
        var desired = target - transform.position;
        desired.Normalize();
        desired *= _currentVelocity;
        return desired;
    }

    void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, _currentVelocity);
    }

    

    public override void Save()
    {
        _currentState.Rec(_isActiveMove, waypoints, _actualIndex,_rb.position);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        
        var col = _currentState.Remember();

        StopAllCoroutines();
        _isActiveMove = (bool)col.parameters[0];
        waypoints = (Transform[])col.parameters[1];
        _actualIndex = 0;
        //gameObject.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)col.parameters[3];
        _rb.velocity = Vector3.zero;
        _rb.position = (Vector3)col.parameters[3];
        _velocity = Vector3.zero;
        _currentVelocity = _maxVelocity;
        //StartCoroutine(WaitSeconds());
        //_currentVelocity = (float)col.parameters[2];

        //banana = (Transform)col.parameters[1];
        //_isObjectAttached = (bool)col.parameters[2];
    }

}
