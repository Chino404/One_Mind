using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class ModelMonkey : Characters, IDamageable, ICure, IObservableGrapp
{
    [Header("Values Character")]
    [SerializeField] private float _maxLife;
    private float _actualLife;
    [SerializeField] private float _speed = 5f;
    private float _actualSpeed;
    [SerializeField] private float _forceGravity;
    private float _initialForceGravity;
    [SerializeField] private float _jumpForce;
    [SerializeField, Range(0, 0.4f)] private float _coyoteTime = 0.2f;
    private float _coyoteTimeCounter;
    [HideInInspector]public bool _holdPower;
    [SerializeField, Range(2f, 7f) ,Tooltip("Fuerza de empuje del golpe")] private float _pushingForce = 5f;
    private bool _grabbed;

    [Header("Daños")]
    [SerializeField] private int _normalDamage;
    [SerializeField, Range(0, 2f)]private float _comboTime = 1.25f;
    private int _currentCombo;
    private float _comboTimeCounter;
    [SerializeField] private int _spinDamage;
    [SerializeField]private float _timePressed;
    public float TimePressed { set{ _timePressed = value; } }
    [SerializeField] private int _getUpDamage;
    [SerializeField] private float _forceToGetUp;
    [HideInInspector]public bool chargeGetUp;
    [SerializeField, Tooltip("Tiempo para el que salte el player"), Range(0.3f, 1f)] private float _timeForGetUp;
    [SerializeField] private bool _punching;
    public event Action PowerUp;

    [Header("Reference")]
    [SerializeField] private LayerMask _floorLayer;
    public float groundDistance = 2;
    [SerializeField] private Transform _pointRotation;
    [SerializeField] private Transform _pointFromPlayer;
    [SerializeField] private MeshRenderer _meshRendererHook;


    //Referencias
    private ControllerMonkey _controller;
    private ViewMonkey _view;

    public Enemy enemy;
    
    private void Awake()
    {
        GameManager.instance.actualCharacter = this;
        GameManager.instance.possibleCharacters[0] = this;
        GameManager.instance.playerGM = this;

        _animatorCharacter = GetComponentInChildren<Animator>();

        _rbCharacter = GetComponent<Rigidbody>();
        _rbCharacter.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; //De esta manera para que se freezeen los dos
        _rbCharacter.angularDrag = 1f; //Friccion de la rotacion


        _view = new ViewMonkey(_animatorCharacter);
        _controller = new ControllerMonkey(this);
    }

    private void Start()
    {
        _actualLife = _maxLife;
        _actualSpeed = _speed;
        _initialForceGravity = _forceGravity;
        _comboTimeCounter = _comboTime;

    }

    private void Update()
    {
        //if (enemy == null) enemy = GetComponent<Enemy>();

        //if (enemy.target == null) enemy.target = this.gameObject;

        if (IsGrounded())
        {
            _coyoteTimeCounter = _coyoteTime;
            PowerUp = SpinAttack;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
            PowerUp = GoToDownAttack;
        }

        if (_comboTimeCounter > 0) _comboTimeCounter -= Time.deltaTime;
        else
        {
            _currentCombo = 0;
            _actualSpeed = _speed;
        }

        _controller.ArtificialUpdate();

    }

    private void FixedUpdate()
    {

        _controller.ListenFixedKeys();
        _rbCharacter.AddForce(Vector3.down * _forceGravity, ForceMode.VelocityChange);
    }

    #region Movement
    public void Movement(Vector3 dirRaw, Vector3 dir)
    {
        if (_punching || chargeGetUp) return;

        if (_grabbed) EventManager.Trigger("Rotate", dirRaw.x);

        if (dirRaw.sqrMagnitude != 0)
        {
            _rbCharacter.MovePosition(transform.position + dir.normalized * _actualSpeed * Time.fixedDeltaTime);
            Rotate(dir);
        }
    }

    public void Rotate(Vector3 dirForward)
    {
        transform.forward = dirForward;
    }

    void CancelarTodasLasFuerzas()
    {
        _forceGravity = 0.05f;
        _rbCharacter.velocity = Vector3.zero; // Establece la velocidad del Rigidbody a cero
        _rbCharacter.angularVelocity = Vector3.zero; // Establece la velocidad angular del Rigidbody a cero
        _rbCharacter.Sleep(); // Detiene toda la simulación dinámica en el Rigidbody
    }

    #endregion

    #region Jump
    public bool IsGrounded()
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        float dist = groundDistance;

        Debug.DrawLine(pos, pos + (dir * dist));

        return Physics.Raycast(pos, dir, out RaycastHit hit, dist, _floorLayer);
    }

    public void Jump()
    {
        if(_grabbed)
        {
            StopGrab();
            Debug.Log("Soltar");
        }

        if (_coyoteTimeCounter > 0f)
        {
            //_rbCharacter.velocity = new Vector3(_rbCharacter.velocity.x, _jumpForce);
            _rbCharacter.velocity = Vector3.up * _jumpForce;
        }
    }

    public void CutJump()
    {
        _coyoteTimeCounter = 0;
        _rbCharacter.velocity = new Vector3(_rbCharacter.velocity.x, _rbCharacter.velocity.y * 0.5f);
    }

    #endregion

    #region Attacks
    public void Attack()
    {
        if (_grabbed) return;
        if (_holdPower) PowerUp();
        else NormalPunch();
    }

    public void NormalPunch()
    {
        if (_punching) return;

        _currentCombo++;

        switch (_currentCombo)
        {
            case 1:
                Debug.Log("1er golpe");
                StartCoroutine(SystemNormalCombo(_pushingForce));
                _comboTimeCounter = _comboTime;
                break;

            case 2:
                Debug.Log("2do golpe");
                StartCoroutine(SystemNormalCombo(_pushingForce + (_pushingForce * 0.5f)));
                _comboTimeCounter = _comboTime;
                break;

            case 3:
                Debug.Log("3er golpe");
                StartCoroutine(SystemNormalCombo(_pushingForce + (_pushingForce * 0.75f)));
                _comboTimeCounter = 0;
                break;
        }
    }

    IEnumerator SystemNormalCombo(float powerForce)
    {
        _punching = true;
        _actualSpeed = 0;
        _rbCharacter.AddForce(transform.forward * powerForce, ForceMode.Impulse);
        EventManager.Trigger("NormalAttack", _normalDamage, 0.3f);
        if(!IsGrounded())CancelarTodasLasFuerzas();
        yield return new WaitForSeconds(0.3f);
        _punching = false;
        yield return new WaitForSeconds(0.2f);

        if(!_punching)
        {
            _actualSpeed = _speed;
            _forceGravity = _initialForceGravity;

        }
    }

    public void SpinAttack()
    {
        _comboTimeCounter = _comboTime * 0.25f;
        EventManager.Trigger("SpinAttack", _spinDamage, _comboTimeCounter);
        _actualSpeed = 2;
    }

    /// <summary>
    /// Ataque para levantar y bajar
    /// </summary>
    public void GoToUpAttack()
    {
        if (chargeGetUp || !IsGrounded()) return;

        _timePressed += Time.deltaTime;

        if(_timePressed >= _timeForGetUp && !chargeGetUp)
        {
            EventManager.Trigger("GetUpAttack", _getUpDamage, (_timeForGetUp /2), _forceToGetUp);
            StartCoroutine(TimeToGetUp());
            _currentCombo = 0;
            _timePressed = 0;
            chargeGetUp = true;
        }
    }

    IEnumerator TimeToGetUp()
    {
        yield return new WaitForSeconds(0.25f);
        Jump();
    }

    public void GoToDownAttack()
    {
        EventManager.Trigger("GetUpAttack", _getUpDamage, (_timeForGetUp / 2), - _forceToGetUp * 1.5f);
        _rbCharacter.velocity = new Vector3(_rbCharacter.velocity.x, -_jumpForce);
        //CancelarTodasLasFuerzas();
        //StartCoroutine(ReturnGravity());
    }

    IEnumerator ReturnGravity()
    {
        yield return new WaitForSeconds((_timeForGetUp/2));
        _forceGravity = _initialForceGravity;
    }
    #endregion

    #region Damage / Life
    public void TakeDamageEntity(float dmg, Vector3 target)
    {
        if(_actualLife >0)
        {
            _actualLife -= dmg;

            if(_actualLife <= 0)
            {
                Debug.Log("Game Over");
                _actualLife = 0;
            }

            EventManager.Trigger("ProjectLifeBar", _maxLife, _actualLife);
        }
    }

    public void GetUpDamage(float dmg, Vector3 target, float forceToUp)
    {

    }

    public void Heal(float life)
    {
        if(_actualLife < _maxLife)
        {
            _actualLife += life;
            if (_actualLife > _maxLife) _actualLife = _maxLife;

            EventManager.Trigger("ProjectLifeBar", _maxLife, _actualLife);
        }
    }
    #endregion

    public void Grab()
    {
        if (_grappList.Count == 0) return;


        _grabbed = true;
        foreach (var item in _grappList)
        {
            var posGrappeable = item.ReturnPosition();

            _meshRendererHook.enabled = true;

            //El player mira al objeto q se agarra
            Vector3 direction = posGrappeable.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

            //El punto en el que se encaja para rotar
            _pointRotation.position = posGrappeable.position;
            _pointRotation.rotation = posGrappeable.rotation;

            transform.position = _pointFromPlayer.position;
        }
    }

    public void StopGrab()
    {

        _grabbed = false;
        _meshRendererHook.enabled = false;
        _rbCharacter.velocity = Vector3.up * (_jumpForce);
    }

    public List<IObserverGrappeable> _grappList = new List<IObserverGrappeable>();

    public void Subscribe(IObserverGrappeable obs)
    {
        if(!_grappList.Contains(obs))
        {
            _grappList.Add(obs);
        }
    }

    public void Unsubscribe(IObserverGrappeable obs)
    {
        if (_grappList.Contains(obs))
        {
            _grappList.Remove(obs);
        }
    }
}
