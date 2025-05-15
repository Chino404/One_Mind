using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Connected, IInteracteable
{
    private Vector3 _velocity;
    private Rigidbody _rb;
    private AudioSource _myAudioSource;

    [Space(10)]
    [SerializeField, Tooltip("Puntos a los que va a ir")] Transform[] _waypoints;
    [SerializeField] private int _actualIndexNode;
    public GameObject door;

    [SerializeField, Tooltip("Velocidad")] private float _maxVelocity = 9f;
    private float _currentVelocity = 7f;
    [SerializeField, Range(0, 8f), Tooltip("Segunso que va a esperar para moverse otra vez")] float _secondsWaiting = 5f;


    [Space(10), SerializeField, Tooltip("Elevador activado")] private bool _isActiveElevator = true;
    public bool IsActiveElevator { set { _isActiveElevator = value; } }

    [SerializeField, Tooltip("Si el elevador ya no se va a mover")] private bool _isNotMove = false;
    public bool IsNotMove { get { return _isNotMove; } }

    public bool isPlaySound = false;

    public override void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();
        _myAudioSource = GetComponent<AudioSource>();

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

        if (Vector3.Distance(_rb.position, _waypoints[_actualIndexNode].position) <= 0.3f)
        {
            StopElevator();

            _actualIndexNode++;

            if (_actualIndexNode >= _waypoints.Length)
            {
               
                _actualIndexNode = 0;
            }
        }

        _velocity = _waypoints[_actualIndexNode].position - _rb.position;
        _velocity.Normalize();


        _rb.MovePosition(_rb.position + _velocity * _currentVelocity * Time.fixedDeltaTime);

    }

    /// <summary>
    /// Activar el elevador.
    /// </summary>
    /// <param name="active"></param>
    public void ActiveElevator()
    {
        if (!isPlaySound)
        {
            isPlaySound = true;
            AudioManager.instance.Play(SoundId.OnlyActive);
        }

        _isActiveElevator = true;

        _currentVelocity = _maxVelocity;
        door.gameObject.SetActive(true);
    }

    public void StopElevator()
    {
        _currentVelocity = 0;
        door.gameObject.SetActive(false);

        StartCoroutine(CoolDownElevator());

    }

    IEnumerator CoolDownElevator()
    {
        yield return new WaitForSeconds(0.75f);

        _isNotMove = !_isNotMove;

        _isActiveElevator = false;

    }

    public void Active()
    {
        if (_isActiveElevator) return;

        _isActive = true;

        if (_connectedObject.IsActive && !_connectedObject.GetComponent<Elevator>().IsNotMove)
        {
            ActiveElevator();

            _connectedObject.gameObject.GetComponent<Elevator>().ActiveElevator();
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
