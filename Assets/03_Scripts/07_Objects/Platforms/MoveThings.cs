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

    

    [Space(10), SerializeField] private bool _isActiveMove = true;
    public bool IsActiveMove { set { _isActiveMove = value; } }


    private int _actualIndex;
    public int ActualIndex { set { _actualIndex = value; } }
    private Vector3 _velocity;
    public Vector3 Velocity { get { return _velocity; } }

    private Rigidbody _rb;
    private Vector3 _startPos;

    //private Animator _animator;
    //[SerializeField] Vector3[] _positions;
    //[SerializeField]private float _speed;
    //private int _actualPosition;



    //public Characters character;

    //private bool _characterInPlataform;

    //private bool _isObjectAttached;

    //[SerializeField] Transform banana; 
    //public ModelBanana modelBanana;

    //private Ray _moveRay;
    //private float _moveRange=0.75f;
    //[SerializeField]private LayerMask _moveMask;

    public override void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();
        
        base.Awake();
    }

    public virtual void Start()
    {

        
        _startPos = _rb.position;
       
    }

    private void OnEnable()
    {
        _currentVelocity = _maxVelocity;
    }

    //private void Update()
    //{
    //    if(Vector3.Distance(transform.position, _waypoints[_actualPosition].position) <= 0.2f)
    //    {
    //        _actualPosition++;
    //        if (_actualPosition >= _positions.Length)
    //            _actualPosition = 0;
    //    }

    //}

    //void FixedUpdate()
    //{
    //    ////if (IsBlocked(modelBanana.Velocity)) modelBanana.Velocity = Vector3.zero;

    //    //if (_isObjectAttached && banana != null)
    //    //{
    //    //    _rb.MovePosition(transform.position + modelBanana.Velocity * Time.fixedDeltaTime);
    //    //}



    //}

    //MATI GIL
    public virtual void FixedUpdate()
    {
        if (!_isActiveMove) return;

        if (Vector3.Distance(_rb.position, waypoints[_actualIndex].position) <= 1f)
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<Characters>())
    //    {
    //        Active();
    //        Debug.Log("estoy arriba de la plataforma");
    //    }
    //}

    //public void Active()
    //{
    //    if(!_isActiveMove) _isActiveMove = true;
    //}

    //public void Deactive()
    //{
        
    //}

    //void CharacterAttached() //Todo lo que quiero que pase cuando el player esta en la plataforma
    //{
    //    character.GetComponent<Rigidbody>().MovePosition(character.GetComponent<Rigidbody>().position + _velocity*0.01f );
    //}

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

    //private bool IsBlocked(Vector3 dir)
    //{
    //    _moveRay = new Ray(transform.position, dir);
    //    Debug.DrawRay(transform.position, dir * _moveRange, Color.red);

    //    return Physics.Raycast(_moveRay, _moveRange, _moveMask);
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<Characters>())
    //    {
    //        collision.transform.SetParent(transform);
           
            
    //    }

    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<Characters>())
    //    {
    //        //collision.transform.SetParent(null);
    //        _characterInPlataform = false;
    //        character = null;

    //    }
    //}

    //public void LeftClickAction()
    //{

    //}

    //public void RightClickAction(Transform parent)
    //{        
    //    if (!_isObjectAttached)
    //    {
    //        //transform.SetParent(parent);
    //        banana = parent;
    //        _isObjectAttached = true;
    //    }
    //    else if (_isObjectAttached)
    //    {
    //        ReleaseObject();
    //    }
    //}



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
