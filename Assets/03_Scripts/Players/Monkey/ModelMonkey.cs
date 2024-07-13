using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ModelMonkey : Characters, IDamageable, ICure//, IObservableGrapp
{
    [Header("Valores Personaje")]
    [SerializeField] private float _maxLife;
    [SerializeField]private float _actualLife;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _actualSpeed;
    [SerializeField] private float _forceGravity;
    private float _initialForceGravity;
    [SerializeField] private float _jumpForce;
    [SerializeField, Range(0, 0.4f)] private float _coyoteTime = 0.2f;
    private float _coyoteTimeCounter;
    [SerializeField, Range(2f, 7f) ,Tooltip("Fuerza de empuje del golpe")] private float _pushingForce = 5f;

    //Raycast para las colsiones y las agarraderas
    [SerializeField, Tooltip("Rango para evitar pegarme al objeto de _moveMask")] private float _moveRange = 0.75f; //Rango para el Raycast para evitar q el PJ se pegue a la pared
    [SerializeField] private LayerMask _moveMask; //Para indicar q layer quierp q no se acerque mucho
    [SerializeField] private LayerMask _handleMask;
    private Ray _moveRay;
    [SerializeField]private bool _grabb;
    [SerializeField]private Vector3 _grabPosition;

    [Header("Daños")]
    [SerializeField] private int _normalDamage;
    [SerializeField, Range(0, 2f)]private float _comboTime = 1.25f;
    private float _comboTimeCounter;
    private int _currentCombo;
    [SerializeField] private bool isRotating = false;
    [SerializeField] private int _spinDamage;
    [SerializeField]private float _timePressed;
    public float TimePressed { set{ _timePressed = value; } }
    [SerializeField] private int _getUpDamage;
    [SerializeField] private float _forceToGetUp;
    [HideInInspector]public bool chargeGetUp;
    [SerializeField, Tooltip("Tiempo para el que salte el player"), Range(0.3f, 1f)] private float _timeForGetUp;
    [SerializeField] private bool _punching;
    public event Action PowerUp;

    [Header("Referencia")]
    [SerializeField] private LayerMask _floorLayer;
    public float groundDistance = 2;
    [SerializeField] private Transform _pointRotation;
    [SerializeField] private Transform _pointFromPlayer;

    [Header("Animator")]
    [SerializeField]private Animator _animPlayer;

    [Header("Particulas")]
    [SerializeField] private ParticleSystem _particleSpinAttack;
    [SerializeField] private ParticleSystem _particleJump;
    [SerializeField] private ParticleSystem _polvo;

    //Referencias
    private ControllerMonkey _controller;
    private ViewMonkey _view;


    public delegate void MyDelegate(params object[] parameters);
    private event MyDelegate _actualMove;

    public MyDelegate ActualMove { get { return _actualMove; } set {  _actualMove = value; } }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //Me bloque el mouse al centro de la pantalla
        Cursor.visible = false; //Me lo oculta

        //GameManager.instance.actualCharacter = this;
        _rbCharacter = GetComponent<Rigidbody>();
        _rbCharacter.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; //De esta manera para que se freezeen los dos
        _rbCharacter.angularDrag = 1f; //Friccion de la rotacion

        _animPlayer = GetComponentInChildren<Animator>();

        _view = new ViewMonkey(_animatorCharacter);
        _controller = new ControllerMonkey(this);
        _currentState = new MementoState();
    }

    private void Start()
    {
        GameManager.instance.playerGM = this;
        GameManager.instance.players[0] = this;

        if(!GameManager.instance.rewinds.Contains(this))
        GameManager.instance.rewinds.Add(this);

        _actualLife = _maxLife;
        _actualSpeed = _speed;
        _initialForceGravity = _forceGravity;

        _comboTimeCounter = _comboTime;

        ActualMove = NormalMovement;
        //EventManager.Subscribe("ActualMovement", NormalMovement);
    }

    private void Update()
    {
        if (!GameManager.instance.ContollerMonkey)
        {
            _animPlayer.SetBool("Walk", false);
            return;
        } 
            

        if (IsGrounded())
        {
            _coyoteTimeCounter = _coyoteTime;
            PowerUp = SpinAttack;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
            PowerUp = null;
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
        _rbCharacter.AddForce(Vector3.down * _forceGravity, ForceMode.VelocityChange);

        if (!GameManager.instance.ContollerMonkey) return;
        _controller.ListenFixedKeys();

    }

    #region Movement
    public void Movement(Vector3 dirRaw, Vector3 dir)
    {
        if (_punching || IsTouch(dir.normalized, _moveMask)) return;

        ActualMove(dirRaw, dir);
        //EventManager.Trigger("ActualMovement", dirRaw, dir);
    }

    public void NormalMovement(params object[] parameters)
    {
        var dirRaw = (Vector3)parameters[0];
        var dir = (Vector3)parameters[1];

        _grabb = false;
        _actualSpeed = _speed;
        _forceGravity = _initialForceGravity;
        _rbCharacter.isKinematic = false;

        if (dirRaw.sqrMagnitude != 0)
        {
            _rbCharacter.MovePosition(transform.position + dir.normalized * _actualSpeed * Time.fixedDeltaTime);
            Rotate(dir);

            if(IsTouch(dir.normalized, _handleMask))
            {
                ActualMove = HandleMovement;

                //EventManager.Unsubscribe("ActualMovement", NormalMovement);
                //EventManager.Subscribe("ActualMovement", HandleMovement);
                return;
            }

            _animPlayer.SetBool("Walk", true);
        }
        else
        {
            _animPlayer.SetBool("Walk", false);
        }
    }

    public void HandleMovement(params object[] parameters)
    {
        var dirRaw = (Vector3)parameters[0];
        var dir = (Vector3)parameters[1];

        _animPlayer.SetBool("Walk", false);

        _grabb = true;
        _actualSpeed = 7f;
        _forceGravity = 0;
        _rbCharacter.isKinematic = true;

       // RaycastHit hitInfo;

        //if (Physics.Raycast(_moveRay, out hitInfo))
        //{
        //    _grabPosition = hitInfo.point;
        //    var objeto = hitInfo.transform.forward;
        //    transform.forward = objeto;
        //}

        if (dirRaw.sqrMagnitude != 0)
        {
            Vector3 subida = new Vector3(dir.normalized.x, dir.normalized.z);

            Ray vistaEnredadera = new Ray(transform.position + subida, transform.forward);
            RaycastHit hitInfo;

            Debug.DrawRay(transform.position + subida, transform.forward * _moveRange, Color.green);

            if (Physics.Raycast(vistaEnredadera, out hitInfo))
            {
                _grabPosition = hitInfo.point;
                var objeto = hitInfo.transform.forward;
                transform.forward = objeto;
            }

            if (!Physics.Raycast(vistaEnredadera, _moveRange, _handleMask)) return;

            if (IsTouch(subida, _moveMask))
            {
                ActualMove = NormalMovement;
                //EventManager.Unsubscribe("ActualMovement", HandleMovement);
                //EventManager.Subscribe("ActualMovement", NormalMovement);

                return;
            }

            _rbCharacter.MovePosition(transform.position + subida * _actualSpeed * Time.fixedDeltaTime);

        }
    }

    /// <summary>
    /// Si el Ray choca con un objeto de la _moveMask, me lo indica con un TRUE
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private bool IsTouch(Vector3 dir, int layerMask)
    {
        _moveRay = new Ray(transform.position, dir);
        Debug.DrawRay(transform.position, dir * _moveRange, Color.red);

        //RaycastHit hitInfo;
        //if (Physics.Raycast(_moveRay, out hitInfo))
        //{
        //    Debug.Log("Objeto alcanzado: " + hitInfo.collider.gameObject.name);
        //}

        return Physics.Raycast(_moveRay,out RaycastHit hit ,_moveRange, layerMask);
    }

    public void Rotate(Vector3 dirForward)
    {
        transform.forward = dirForward;
    }

    public void CancelarTodasLasFuerzas()
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

        //Debug.DrawLine(pos, pos + (dir * dist));

        return Physics.Raycast(pos, dir, out RaycastHit hit, dist, _floorLayer);
    }

    public void Jump()
    {
        if (isRotating) return;

        if(_grabb)
        {
            ActualMove = NormalMovement;
            //Vector3 directionToJump = -_grabPosition * 2;
            _rbCharacter.isKinematic = false;
            _forceGravity = _initialForceGravity;
            _rbCharacter.velocity = new Vector3(0, 10, 6 * -transform.position.z);
            //_rbCharacter.AddForce(directionToJump * _jumpForce, ForceMode.Impulse);
            Debug.Log("SALTO");
        }

        if (_coyoteTimeCounter > 0f)
        {
            _animPlayer?.SetTrigger("Jump");
            _particleJump?.Play();
            _rbCharacter.velocity = Vector3.up * _jumpForce;
            AudioManager.instance.PlayMonkeySFX(AudioManager.instance.jump);
        }
    }

    public void CutJump()
    {
        _particleJump.Stop();
        _coyoteTimeCounter = 0;
        _rbCharacter.velocity = new Vector3(_rbCharacter.velocity.x, _rbCharacter.velocity.y * 0.5f);
    }

    #endregion

    #region Attacks
    public void Attack()
    {
        //if (_grabbed) return;
        //if(!IsGrounded()) GoToDownAttack();
        //else NormalPunch();

        if(IsGrounded())NormalPunch();
    }

    public void NormalPunch()
    {
        if (_punching) return;

        _currentCombo++;
        AudioManager.instance.PlayMonkeySFX(AudioManager.instance.swoosh);
        

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
        _animPlayer.SetTrigger("Attack");
        if (!IsGrounded())CancelarTodasLasFuerzas();
        yield return new WaitForSeconds(0.5f);
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
        if (isRotating) return;

        var clip = AudioManager.instance.attackSpin;
        if (!AudioManager.instance.ExecuteClipMonkey(clip)) AudioManager.instance.PlayMonkeySFX(clip);

        //AudioManager.instance.PlaySFX(clip);
        _comboTimeCounter = _comboTime * 0.25f;

        _particleSpinAttack.Play();
        _animPlayer.SetTrigger("Spin");
        _actualSpeed = 2;
        StartCoroutine(RotateObject());
    }

    IEnumerator RotateObject()
    {
        isRotating = true;
        float elapsedTime = 0f;
        float startRotationY = transform.eulerAngles.y;
        float targetRotationY = startRotationY - 360f;

        while (elapsedTime < 0.2f)
        {
            elapsedTime += Time.deltaTime;
            float currentRotationY = Mathf.Lerp(startRotationY, targetRotationY, elapsedTime / 0.2f) % 360f;
            transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, targetRotationY % 360f, 0); // Asegura que la rotación final sea precisa
        _particleSpinAttack.Stop();
        AudioManager.instance.StopMonkeySFX();
        isRotating = false;
    }

    /// <summary>
    /// Ataque para levantar y bajar
    /// </summary>
    //public void GoToUpAttack()
    //{
    //    if (chargeGetUp || !IsGrounded()) return;

    //    _timePressed += Time.deltaTime;

    //    if(_timePressed >= _timeForGetUp && !chargeGetUp)
    //    {
    //        EventManager.Trigger("GetUpAttack", _getUpDamage, (_timeForGetUp /2), _forceToGetUp);
    //        StartCoroutine(TimeToGetUp());
    //        _currentCombo = 0;
    //        _timePressed = 0;
    //        chargeGetUp = true;
    //    }
    //}

    //IEnumerator TimeToGetUp()
    //{
    //    yield return new WaitForSeconds(0.25f);
    //    Jump();
    //}

    //IEnumerator ReturnGravity()
    //{
    //    yield return new WaitForSeconds((_timeForGetUp/2));
    //    _forceGravity = _initialForceGravity;
    //}

    public void GoToDownAttack()
    {
        EventManager.Trigger("GetUpAttack", _getUpDamage, (_timeForGetUp / 2), - _forceToGetUp * 1.5f);
        _rbCharacter.velocity = new Vector3(_rbCharacter.velocity.x, -_jumpForce);
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
                //_actualLife = 0;
                Dead();
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

    public void Dead()
    {
        _actualLife = 0;
        PauseManager.instance.GameOver();

    }
    #endregion

    #region Lihana Giro
    /*
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
        if (_grappList.Count == 0) return;

        EventManager.Trigger("StopRotate");
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
    }*/
    #endregion

    #region Memento
    public override void Save()
    {
        _currentState.Rec(transform.position, transform.rotation, _actualLife);
        //Debug.Log("guarde mono");
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        transform.position = (Vector3)col.parameters[0]; 
        transform.rotation = (Quaternion)col.parameters[1]; 
        _actualLife = (float)col.parameters[2];
        EventManager.Trigger("ProjectLifeBar", _maxLife, _actualLife);


        //Debug.Log("cargue mono");
    }
    #endregion

    private void OnDestroy()
    {
         EventManager.Unsubscribe("ActualMovement", NormalMovement);
         EventManager.Unsubscribe("ActualMovement", HandleMovement);

    }

}
