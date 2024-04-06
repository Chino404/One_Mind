using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class Model : Characters, IDamageable, ICure
{
    [Header("Values General")]
    [SerializeField] private float _maxLife;
    private float _actualLife;
    [SerializeField] private float _speed = 5f;
    private float _actualSpeed;
    [SerializeField] private float _forceGravity;
    [SerializeField] private float _jumpForce;
    private int _currentCombo;
    private bool _punching;
    [SerializeField] private int _damage;
    [SerializeField, Range(2f, 7f) ,Tooltip("Fuerza de empuje del golpe")] private float _pushingForce = 5f;
    [SerializeField, Range(0, 2f)]private float _comboTime = 1.25f;
    private float _comboTimeCounter;

    [Header("Coyote Time")]
    public float groundDistance = 2;
    [SerializeField, Range(0, 0.4f)] private float _coyoteTime = 0.2f;
    private float _coyoteTimeCounter;

    [Header("Reference")]
    [SerializeField] private LayerMask _floorLayer;


    //Referencias
    private Rigidbody _rb;
    private Controller _controller;
    private View _view;
    [SerializeField]private PunchSystemPlayer _punchSystem;
    public Animator _animator;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _punchSystem = GetComponentInChildren<PunchSystemPlayer>();

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; //De esta manera para que se freezeen los dos
        _rb.angularDrag = 1f; //Friccion de la rotacion


        _view = new View(_animator);
        _controller = new Controller(this, _view);
    }

    private void Start()
    {
        _actualLife = _maxLife;
        _actualSpeed = _speed;
        _comboTimeCounter = _comboTime;
    }

    private void Update()
    {
        if(IsGrounded()) _coyoteTimeCounter = _coyoteTime;
        else _coyoteTimeCounter -= Time.deltaTime;

        if (_comboTimeCounter > 0) _comboTimeCounter -= Time.deltaTime;
        else _currentCombo = 0;

        _controller.ArtificialUpdate();

    }

    private void FixedUpdate()
    {
        _rb.AddForce(Vector3.down * _forceGravity, ForceMode.Impulse);
        _controller.ListenFixedKeys();
    }

    #region Movement
    public void Movement(Vector3 dirRaw, Vector3 dir)
    {
        if (dirRaw.sqrMagnitude != 0 && !_punching)
        {
            _rb.MovePosition(transform.position + dir.normalized * _actualSpeed * Time.fixedDeltaTime);
            Rotate(dir);
        }
    }

    public void Rotate(Vector3 dirForward)
    {
        transform.forward = dirForward;
    }
    #endregion

    #region Attacks
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
        _rb.AddForce(transform.forward * powerForce, ForceMode.VelocityChange);
        EventManager.Trigger("NormalAttack", _damage, 0.3f);
        yield return new WaitForSeconds(0.3f);
        _actualSpeed = _speed;
        _punching = false;

    }

    public void SpinAttack()
    {
        Debug.Log("Spin");
        _actualSpeed = 2;

        StartCoroutine(normalSpeed());
    }

    IEnumerator normalSpeed()
    {
        yield return new WaitForSeconds(0.5f);
        _actualSpeed = _speed;
    }

    #endregion

    #region JUMP
    public bool IsGrounded()
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        float dist = groundDistance;

        Debug.DrawLine(pos, pos + (dir * dist));

        return Physics.SphereCast(pos, 1, dir, out RaycastHit hit, dist, _floorLayer);
    }

    public void Jump()
    {
        if (_coyoteTimeCounter > 0f)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce);
        }
    }

    public void CutJump()
    {
        _coyoteTimeCounter = 0;
        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y * 0.5f);
    }

    #endregion

    #region Damage / Life
    public void TakeDamage(float dmg)
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
}
