using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ModelMonkey : Characters, ICure
{
    #region VARIABLES QUE YA NO SE USAN
    //private Animator _animPlayer;

    //[SerializeField] private float _maxLife;
    //[SerializeField]private float _actualLife;
    //[SerializeField] private float _iniSpeed = 10f;
    //[SerializeField] private float _actualSpeed;
    //private float _initialForceGravity;
    //[SerializeField, Tooltip("Fuerza de salto normal")] private float _jumpForce;
    //[SerializeField, Range(1,40), Tooltip("Fuerza de salto en el eje Z cuando está en la enredadera")] private float _jumpForceAxiZ;
    //[SerializeField, Range(1,40), Tooltip("Fuerza de salto en el eje X cuando está en la enredadera")] private float _jumpForceAxiX;
    //[SerializeField, Range(1,40), Tooltip("Fuerza de salto en el eje Y cuando está en la enredadera")] private float _jumpForceAxiY;
    //private bool _jumpGrabb;
    //private bool _waitRay;
    //[SerializeField, Range(0, 0.4f)] private float _coyoteTime = 0.2f;
    //private float _coyoteTimeCounter;
    //[SerializeField, Range(2f, 7f) ,Tooltip("Fuerza de empuje del golpe")] private float _pushingForce = 5f;

    ////Raycast para las colsiones y las agarraderas
    //[SerializeField, Tooltip("Rango para evitar pegarme al objeto de _moveMask")] private float _moveRange = 0.75f; //Rango para el Raycast para evitar q el PJ se pegue a la pared
    //[SerializeField, Tooltip("Que Layer no quiero que se acerque")] private LayerMask _moveMask; //Para indicar q layer quierO q no se acerque mucho
    //[SerializeField, Tooltip("Layer de Enredaderas u objeto a trepar")] private LayerMask _handleMask;
    //private Ray _moveRay;
    //private bool _stopGrabb;
    //private Vector3 _dirGrabb; //Direccion de movimiento en la agarradera

    //[Header("--- DAÑOS ---")]
    //[SerializeField] private int _normalDamage;
    //private Vector3 _launchDir;

    //[Header("--- REFERENCIAS ---")]
    //[SerializeField] private LayerMask _floorLayer;
    //[SerializeField] private float _groundDistance = 2;
    //[SerializeField] private TargetBanana _targetBanana;


    //[Header("Particulas")]
    //[SerializeField] private ParticleSystem _particleSpinAttack;
    //[SerializeField] private ParticleSystem _particleJump;
    //[SerializeField] private ParticleSystem _polvo;

    //public delegate void MyDelegate(Vector3 dirRaw, Vector3 dir);
    //public event MyDelegate _actualMove;
    //public MyDelegate ActualMove { get { return _actualMove; } set {  _actualMove = value; } }


    //[Header("VARIABLES MINIGUN")]
    //[Tooltip("Velocidad en estado minigun")]
    //[SerializeField] private float _minigunSpeed;
    //public bool canActivateMinigun;
    //[SerializeField] private Minigun _minigun;
    #endregion

    //Referencias
    private ControllerMonkey _controller;
    private ViewMonkey _view;  
    

    public override void Awake()
    {
        base.Awake();

        GameManager.instance.bongo = transform;

        _view = new ViewMonkey(_animPlayer);
        _controller = new ControllerMonkey(this);
        _currentState = new MementoState();
    }

    public override void Start()
    {
        base.Start();
        ActualMove = NormalMovement;

        //canActivateMinigun = false;
    }

    public override void Update()
    {
        if (!GameManager.instance.ContollerMonkey)
        {
            _animPlayer.SetBool("Walk", false);
            return;
        }         

        if (IsGrounded())
        {
            _jumpGrabb = false;
            _coyoteTimeCounter = _coyoteTime;
        }
        else _coyoteTimeCounter -= Time.deltaTime;
        
        ChangeSpeed();

        _controller.ArtificialUpdate();
    }

    public override void FixedUpdate()
    {
        _rbCharacter.AddForce(Vector3.down * _forceGravity, ForceMode.VelocityChange);

        if (!GameManager.instance.ContollerMonkey) return;
        _controller.ListenFixedKeys();

    }

    #region Movement (YA NO SE USA)
    //public void Movement(Vector3 dirRaw, Vector3 dir)
    //{
    //    if (actualStateBongo == EstadoDeBongo.Golpeando || IsTouch(dir.normalized, _moveMask)) return;

    //    ActualMove(dirRaw, dir);
    //}

    //private void ChangeSpeed()
    //{
    //    if (actualStateBongo == EstadoDeBongo.Normal) _actualSpeed = _speed;
    //    if (actualStateBongo == EstadoDeBongo.Escalando) _actualSpeed = 7;
    //    if (actualStateBongo == EstadoDeBongo.Golpeando) _actualSpeed = 0;      
    //}

    //public void NormalMovement(Vector3 dirRaw, Vector3 dir)
    //{
    //    //if(actualStateBongo != EstadoDeBongo.Normal) actualStateBongo = EstadoDeBongo.Normal;

    //    _dirGrabb = default;

    //    _forceGravity = _initialForceGravity;
    //    _rbCharacter.isKinematic = false;

    //    if (dirRaw.sqrMagnitude != 0)
    //    {

    //        _launchDir = dir;

    //        if(!_jumpGrabb)
    //        {

    //            _rbCharacter.MovePosition(transform.position + dir.normalized * _actualSpeed * Time.fixedDeltaTime);
    //            Rotate(dir);
    //        }

    //        if (IsTouch(transform.forward, _handleMask) && !_waitRay) //Si toco algo escalable, cambio de movimiento
    //        {
    //            //_forceGravity = 0;
    //            //_rbCharacter.isKinematic = true;
    //            actualStateBongo = EstadoDeBongo.Escalando;

    //            ActualMove = HandleMovement;
    //            _jumpGrabb = false;
    //            return;
    //        }

    //        _animPlayer.SetBool("Walk", true);
    //    }

    //    else
    //    {
    //        //actualStateBongo = EstadoDeBongo.Quieto;
    //        _animPlayer.SetBool("Walk", false);
    //    }
    //}

    //public void Rotate(Vector3 dirForward)
    //{
    //    transform.forward = dirForward;
    //}

    //public void HandleMovement(Vector3 dirRaw, Vector3 dir)
    //{
    //    _animPlayer.SetBool("Walk", false);

    //    _forceGravity = 0;
    //    _rbCharacter.isKinematic = true;

    //    if (dirRaw.sqrMagnitude != 0)
    //    {
    //        Vector3 dirEscalando = new Vector3(dir.normalized.x, dir.normalized.z);
    //        _dirGrabb = dirEscalando;

    //        Ray vistaEnredadera = new Ray(transform.position + dirEscalando, transform.forward);
    //        Debug.DrawRay(transform.position + dirEscalando, transform.forward * _forwardRange, Color.green);

    //        RaycastHit hitInfo;
    //        if (Physics.Raycast(vistaEnredadera, out hitInfo)) transform.forward = hitInfo.transform.forward; //Miro para la enredadera

    //        if (!Physics.Raycast(vistaEnredadera, _forwardRange, _handleMask)) //Si para la direccion que quiero ir no hay mas agarradera, no sigo
    //        {
    //            _stopGrabb = true;
    //            return;
    //        }

    //        if (IsTouch(dirEscalando, _moveMask))
    //        {
    //            ActualMove = NormalMovement;
    //            actualStateBongo = EstadoDeBongo.Normal;

    //            return;
    //        }

    //        _stopGrabb = false;
    //        _rbCharacter.MovePosition(transform.position + dirEscalando * _actualSpeed * Time.fixedDeltaTime);

    //    }
    //    else
    //    {
    //        _dirGrabb = default;
    //    }
    //}

    /// <summary>
    /// Si el Ray choca con un objeto de la _moveMask, me lo indica con un TRUE
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    //private bool IsTouch(Vector3 dir, int layerMask)
    //{
    //    _moveRay = new Ray(transform.position, dir);
    //    Debug.DrawRay(transform.position, dir * _forwardRange, Color.red);

    //    //RaycastHit hitInfo;
    //    //if (Physics.Raycast(_moveRay, out hitInfo))
    //    //{
    //    //    Debug.Log("Objeto alcanzado: " + hitInfo.collider.gameObject.name);
    //    //}

    //    return Physics.Raycast(_moveRay,out RaycastHit hit ,_forwardRange, layerMask);
    //}


    //public void CancelarTodasLasFuerzas()
    //{
    //    _forceGravity = 0.05f;
    //    _rbCharacter.velocity = Vector3.zero; // Establece la velocidad del Rigidbody a cero
    //    _rbCharacter.angularVelocity = Vector3.zero; // Establece la velocidad angular del Rigidbody a cero
    //    _rbCharacter.Sleep(); // Detiene toda la simulación dinámica en el Rigidbody
    //}

    #endregion

    #region Jump (YA NO SE USA)
    //public bool IsGrounded()
    //{      
    //    Vector3 pos = transform.position;
    //    Vector3 dir = Vector3.down;
    //    float dist = _groundRange;

    //    //Debug.DrawLine(pos, pos + (dir * dist));

    //    return Physics.Raycast(pos, dir, out RaycastHit hit, dist, _floorLayer);
    //}

    //public void Jump()
    //{

    //    if(actualStateBongo == EstadoDeBongo.Escalando) //Si estoy escalando
    //    {
    //        //actualStateBongo = EstadoDeBongo.Quieto;
    //        StartCoroutine(WaitRayChange());

    //        if (_dirGrabb.sqrMagnitude != 0 && _stopGrabb)
    //        {
    //            ActualMove = NormalMovement;

    //            _jumpGrabb = true;
    //            _rbCharacter.isKinematic = false;
    //            _forceGravity = _initialForceGravity;

    //            //Depende la direccion de donde quiera ir, va a saltar
    //            if (_dirGrabb.y > 0) _rbCharacter.velocity = new Vector3(0, _dirGrabb.y * _jumpForceAxiY * 2);
    //            else if (_dirGrabb.x != 0) _rbCharacter.velocity = new Vector3(_dirGrabb.x * _jumpForceAxiX, _jumpForceAxiY);

    //            _dirGrabb = default;
    //        }

    //        //else if(_dirGrabb.sqrMagnitude == 0) //Si no, salta en direccion opuesta a la agarradera
    //        //{
    //        //    ActualMove = NormalMovement;

    //        //    Vector3 oppDir = -transform.forward;
    //        //    Vector3 jumpDir = new Vector3(oppDir.x, _jumpForceAxiY, oppDir.z * _jumpForceAxiZ);
    //        //    _forceGravity = _initialForceGravity;
    //        //    _rbCharacter.isKinematic = false;
    //        //    _rbCharacter.velocity = jumpDir;
    //        //}
    //    }

    //    if (_coyoteTimeCounter > 0f)
    //    {
    //        _animPlayer?.SetTrigger("Jump");
    //        _particleJump?.Play();
    //        _rbCharacter.velocity = Vector3.up * _jumpForce;
    //        AudioManager.instance.PlayMonkeySFX(AudioManager.instance.jump);
    //    }
    //}

    //IEnumerator WaitRayChange()
    //{
    //    _waitRay = true;
    //    yield return new WaitForSeconds(0.2f);
    //    _waitRay = false;
    //}
    #endregion

    #region Attacks (YA NO SE USA)
    //public void Attack()
    //{
    //    if(IsGrounded())NormalPunch();
    //}

    //public void NormalPunch()
    //{

    //    if (actualStateBongo == EstadoDeBongo.Golpeando) return;

    //    //_currentCombo++;
    //    AudioManager.instance.PlayMonkeySFX(AudioManager.instance.swoosh);


    //    //switch (_currentCombo)
    //    //{
    //    //    case 1:
    //    //        StartCoroutine(SystemNormalCombo(_pushingForce));
    //    //        _comboTimeCounter = _comboTime;
    //    //        break;

    //    //    //case 2:
    //    //    //    StartCoroutine(SystemNormalCombo(_pushingForce + (_pushingForce * 0.5f)));
    //    //    //    _comboTimeCounter = _comboTime;
    //    //    //    break;

    //    //    //case 3:
    //    //    //    StartCoroutine(SystemNormalCombo(_pushingForce + (_pushingForce * 0.75f)));
    //    //    //    _comboTimeCounter = 0;
    //    //    //    break;
    //    //}

    //    StartCoroutine(SystemNormalCombo());
    //}

    //IEnumerator SystemNormalCombo()
    //{
    //    actualStateBongo = EstadoDeBongo.Golpeando;
    //    _animPlayer.SetTrigger("Attack");
    //    yield return new WaitForSeconds(0.4f);
    //    actualStateBongo = EstadoDeBongo.Normal;
    //    yield return new WaitForSeconds(0.2f);

    //    if(actualStateBongo == EstadoDeBongo.Normal)
    //    {
    //        _actualSpeed = _speed;
    //        _forceGravity = _initialForceGravity;

    //    }
    //}

    //public void ChargedAttack()
    //{
    //    if (BananaGuide.Instance.actualStateBananaNPC != EstadoDeBananaBotNPC.EnPosicion) return;

    //    EventManager.Trigger("ChargeAttack");

    //    actualStateBongo = EstadoDeBongo.CargandoAtaqueElectrico;
    //    Debug.DrawRay(transform.position, transform.forward * 20f, Color.red);
    //    _targetBanana.ChargedAttack();
    //}

    //public void SuccesChargedAttack()
    //{
    //    if (BananaGuide.Instance.actualStateBananaNPC != EstadoDeBananaBotNPC.EnPosicion) return;

    //    EventManager.Trigger("LaunchChargedAttack", _launchDir.normalized);

    //    _targetBanana.NormalPosition();
    //    StartCoroutine(ReturnMove());
    //}

    //IEnumerator ReturnMove()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    actualStateBongo = EstadoDeBongo.Normal;

    //}

    //public void SpinAttack()
    //{

    //    var clip = AudioManager.instance.attackSpin;
    //    if (!AudioManager.instance.ExecuteClipMonkey(clip)) AudioManager.instance.PlayMonkeySFX(clip);

    //    //AudioManager.instance.PlaySFX(clip);

    //    _particleSpinAttack.Play();
    //    _animPlayer.SetTrigger("Spin");
    //    _actualSpeed = 2;
    //    StartCoroutine(RotateObject());
    //}

    //IEnumerator RotateObject()
    //{
    //    isRotating = true;
    //    float elapsedTime = 0f;
    //    float startRotationY = transform.eulerAngles.y;
    //    float targetRotationY = startRotationY - 360f;

    //    while (elapsedTime < 0.2f)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float currentRotationY = Mathf.Lerp(startRotationY, targetRotationY, elapsedTime / 0.2f) % 360f;
    //        transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
    //        yield return null;
    //    }

    //    transform.rotation = Quaternion.Euler(0, targetRotationY % 360f, 0); // Asegura que la rotación final sea precisa
    //    _particleSpinAttack.Stop();
    //    AudioManager.instance.StopMonkeySFX();
    //    isRotating = false;
    //}

    //public void ActivateMinigun()
    //{
    //    actualStateBongo = EstadoDeBongo.Minigun;
    //    _minigun.gameObject.SetActive(true);
    //}

    //public void DesactiveMinigun()
    //{
    //    actualStateBongo = EstadoDeBongo.Normal;
    //    _minigun.gameObject.SetActive(false);
    //}

    #endregion

    #region Damage / Life (YA NO SE USA)
    //public void TakeDamageEntity(float dmg, Vector3 target)
    //{
    //    if(_actualLife >0)
    //    {
    //        _actualLife -= dmg;

    //        if(_actualLife <= 0)
    //        {
    //            //_actualLife = 0;
    //            Dead();
    //        }

    //        EventManager.Trigger("ProjectLifeBar", _maxLife, _actualLife);
    //    }
    //}

    //public void GetUpDamage(float dmg, Vector3 target, float forceToUp)
    //{

    //}

    //public void Heal(float life)
    //{
    //    if(_actualLife < _maxLife)
    //    {
    //        _actualLife += life;
    //        if (_actualLife > _maxLife) _actualLife = _maxLife;

    //        EventManager.Trigger("ProjectLifeBar", _maxLife, _actualLife);
    //    }
    //}

    //public void Dead()
    //{
    //    _actualLife = 0;
    //    PauseManager.instance.GameOver();

    //}
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

    #region Memento (YA NO SE USA)
    //public override void Save()
    //{
    //    _currentState.Rec(transform.position, transform.rotation, _actualLife, actualStateBongo /*, _minigun.gameObject.activeInHierarchy, canActivateMinigun*/);
    //    //Debug.Log("guarde mono");
    //}

    //public override void Load()
    //{
    //    if (!_currentState.IsRemember()) return;

    //    var col = _currentState.Remember();
    //    transform.position = (Vector3)col.parameters[0]; 
    //    transform.rotation = (Quaternion)col.parameters[1]; 
    //    _actualLife = (float)col.parameters[2];
    //    actualStateBongo = (EstadoDeBongo)col.parameters[3];
    //    //_minigun.gameObject.SetActive((bool)col.parameters[4]);
    //    //canActivateMinigun = (bool)col.parameters[5];

    //    EventManager.Trigger("ProjectLifeBar", _maxLife, _actualLife);


    //    //Debug.Log("cargue mono");
    //}
    #endregion

}
