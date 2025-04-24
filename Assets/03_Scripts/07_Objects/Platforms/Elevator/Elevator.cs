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
    [SerializeField, Range(0, 8f) ,Tooltip("Segunso que va a esperar para moverse otra vez")] float _secondsWaiting = 5f;


    [Space(10), SerializeField, Tooltip("Elevador activado")] private bool _isActiveElevator = true;
    public bool IsActiveElevator { set { _isActiveElevator = value; } }

    public bool isNotMove;

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
            ActiveElevator(false);

            _actualIndexNode++;

            if (_actualIndexNode >= _waypoints.Length)
            {
                isNotMove = false;

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
    public void ActiveElevator(bool active)
    {
        if(!active)
        {
            _isActiveElevator = false;

            isNotMove = true;

            _currentVelocity = 0;
            door.gameObject.SetActive(false);

            return;
        }

        else
        {
            _isActiveElevator = true;
            //_myAudioSource.Play();

            AudioManager.instance.Play(SoundId.WoodElevator);
        }


        _currentVelocity = _maxVelocity;
        door.gameObject.SetActive(true);
    }

    public void Active()
    {
        _isActive = true;

        if (_connectedObject.IsActive && !_connectedObject.GetComponent<Elevator>().isNotMove)
        {

            //door.gameObject.SetActive(true);
            //_connectedObject.gameObject.GetComponent<Elevator>().door.gameObject.SetActive(true);

            //_isActiveElevator = true;
            //_connectedObject.gameObject.GetComponent<Elevator>().IsActiveElevator = true;


            ActiveElevator(true);
            _connectedObject.gameObject.GetComponent<Elevator>().ActiveElevator(true);
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
