using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EstadoDePlayer
{
    Normal,
    Escalando,
    Golpeando,
}
public abstract class Characters : Entity, IDamageable
{
    protected Rigidbody _rbCharacter;
    protected Animator _animPlayer;

    [Header("--- VALUE CHARACTERS ---")]

    [Space(10)]public EstadoDePlayer actualStatePlayer;

    [Header("--> GENERAL")]
    [SerializeField] protected float _maxLife = 3f;
    protected float _actualLife;
    [SerializeField] protected float _speed = 10f;
    protected float _actualSpeed;
    [SerializeField] protected float _forceGravity = 1.25f;
    protected float _initialForceGravity;
    [SerializeField, Tooltip("Fuerza de salto normal")] protected float _jumpForce = 25f;
    [SerializeField, Range(0, 0.4f), Tooltip("Tiempo para saltar cuando dejo de tocar el suelo")] protected float _coyoteTime = 0.15f;
    protected float _coyoteTimeCounter;
    [SerializeField, Tooltip("Daño de golpe")] protected int _normalDamage = 1;
    protected Vector3 _launchDir;

    [Header("--> CLIMB")]
    [SerializeField, Range(1, 40), Tooltip("Fuerza de salto en el eje X")] protected float _jumpForceAxiX = 20;
    [SerializeField, Range(1, 40), Tooltip("Fuerza de salto en el eje Y")] protected float _jumpForceAxiY = 15;
    //[SerializeField, Range(1, 40), Tooltip("Fuerza de salto en el eje Z, dirección opuesta a la enredadera")] protected float _jumpForceAxiZ = 10;
    protected bool _jumpGrabb;
    protected bool _waitRay;

    [Header("--> RAYCASTS")]
    [SerializeField, Range(0.1f, 3f) , Tooltip("Rango del raycast para el coyote time")] protected float _groundRange = 2;
    [SerializeField, Tooltip("Layer de objeto en donde pueda saltar")] protected LayerMask _floorLayer;
    [SerializeField, Range(0.1f, 1f) , Tooltip("Rango del raycast para las colisiones")] protected float _forwardRange = 0.75f; //Rango para el Raycast para evitar q el PJ se pegue a la pared
    [SerializeField, Tooltip("Que Layer no quiero que se acerque")] protected LayerMask _moveMask; //Para indicar q layer quierO q no se acerque mucho
    [SerializeField, Tooltip("Layer de Enredaderas u objeto a trepar")] protected LayerMask _handleMask;
    [SerializeField, Tooltip("Layer de objeto para seguir")] protected LayerMask _continueMask;
    protected Ray _moveRay;
    public bool stopMove;
    protected Vector3 _dirGrabb; //Direccion de movimiento en la agarradera
    protected bool _stopGrabb;

    [Header("--> PARTICLES")]
    [SerializeField] protected ParticleSystem _particleJump;
    [SerializeField] protected ParticleSystem _polvo;
    //[SerializeField] protected ParticleSystem _particleSpinAttack;

    public delegate void MyDelegate(Vector3 dirRaw, Vector3 dir);
    public event MyDelegate _actualMove;
    public MyDelegate ActualMove { get { return _actualMove; } set { _actualMove = value; } }


    public override void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //Me bloque el mouse al centro de la pantalla
        Cursor.visible = false; //Me lo oculta

        _rbCharacter = GetComponent<Rigidbody>();
        _rbCharacter.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; //De esta manera para que se freezeen los dos
        _rbCharacter.angularDrag = 1f; //Friccion de la rotacion
        _rbCharacter.drag = 1;

        _animPlayer = GetComponentInChildren<Animator>();

        base.Awake();
    }

    public virtual void Start()
    {
        _actualLife = _maxLife;
        _actualSpeed = _speed;
        _initialForceGravity = _forceGravity;

    }

    public virtual void Update()
    {
        if (IsGrounded())
        {
            _jumpGrabb = false;
            _coyoteTimeCounter = _coyoteTime;
        }
        else _coyoteTimeCounter -= Time.deltaTime;

        ChangeSpeed();
    }

    public virtual void FixedUpdate()
    {
        _rbCharacter.AddForce(Vector3.down * _forceGravity, ForceMode.VelocityChange);
    }

    #region RAYCAST

    /// <summary>
    /// Si el Ray choca con un objeto de la layer asignada, me lo indica con un TRUE
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public bool IsTouch(Vector3 dir, int layerMask)
    {
        _moveRay = new Ray(transform.position, dir);
        Debug.DrawRay(transform.position, dir * _forwardRange, Color.red);

        //RaycastHit hitInfo;
        //if (Physics.Raycast(_moveRay, out hitInfo))
        //{
        //    Debug.Log("Objeto alcanzado: " + hitInfo.collider.gameObject.name);
        //}

        return Physics.Raycast(_moveRay, out RaycastHit hit, _forwardRange, layerMask);
    }

    /// <summary>
    /// Si estoy tocando algun objeto de la layer para saltar
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded()
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        float dist = _groundRange;

        Debug.DrawLine(pos, pos + (dir * dist));

        return Physics.Raycast(pos, dir, out RaycastHit hit, dist, _floorLayer);
    }

    #endregion

    #region MOVEMENT
    public virtual void Movement(Vector3 dirRaw, Vector3 dir)
    {
        if (actualStatePlayer == EstadoDePlayer.Golpeando || IsTouch(dir.normalized, _moveMask))
        {
            stopMove = true;
            _animPlayer.SetBool("Walk", false);
            return;
        }
        else stopMove = false;

        ActualMove(dirRaw, dir);
    }

    protected void ChangeSpeed()
    {
        if (actualStatePlayer == EstadoDePlayer.Normal) _actualSpeed = _speed;
        //if (actualStatePlayer == EstadoDePlayer.Escalando) _actualSpeed = 7;
        if (actualStatePlayer == EstadoDePlayer.Golpeando) _actualSpeed = 0;
    }

    public void Rotate(Vector3 dirForward) => transform.forward = dirForward;

   

    public void NormalMovement(Vector3 dirRaw, Vector3 dir)
    {
        //if(actualStateBongo != EstadoDeBongo.Normal) actualStateBongo = EstadoDeBongo.Normal;

        _dirGrabb = default;

        _forceGravity = _initialForceGravity;
        _rbCharacter.isKinematic = false;

        if (dirRaw.sqrMagnitude != 0)
        {

            _launchDir = dir;

            if (!_jumpGrabb)
            {

                _rbCharacter.MovePosition(transform.position + dir.normalized * _actualSpeed * Time.fixedDeltaTime);
                Rotate(dir);
            }

            if (IsTouch(transform.forward, _handleMask) && !_waitRay) //Si toco algo escalable, cambio de movimiento
            {
                //_forceGravity = 0;
                //_rbCharacter.isKinematic = true;
                actualStatePlayer = EstadoDePlayer.Escalando;

                ActualMove = HandleMovement;
                _jumpGrabb = false;
                return;
            }

            //_animPlayer.SetBool("Walk", true);
        }

        //else
        //{
        //    _animPlayer.SetBool("Walk", false);
        //}
    }

    public void HandleMovement(Vector3 dirRaw, Vector3 dir)
    {
        //Debug.Log("estoy agarrado");

        _animPlayer.SetBool("Walk", false);

        if (actualStatePlayer == EstadoDePlayer.Normal)
        {
            StartCoroutine(WaitRayChange());
            ActualMove = NormalMovement;
        }

        _forceGravity = 0;
        _rbCharacter.isKinematic = true;

        if (dirRaw.sqrMagnitude != 0)
        {
            Vector3 dirEscalando = new Vector3(dir.normalized.x, dir.normalized.z);
            _dirGrabb = dirEscalando;

            Ray vistaEnredadera = new Ray(transform.position + dirEscalando, transform.forward);
            Debug.DrawRay(transform.position + dirEscalando, transform.forward * _forwardRange, Color.green);

            RaycastHit hitInfo;
            if (Physics.Raycast(vistaEnredadera, out hitInfo, _forwardRange, _handleMask)) transform.forward = hitInfo.transform.forward; //Miro para la enredadera

            Ray vistaParaSaltar = new Ray(transform.position + (dirEscalando * 1.5f), transform.forward);
            Debug.DrawRay(transform.position + (dirEscalando * 1.5f), transform.forward * _forwardRange, Color.blue);

            if (Physics.Raycast(vistaParaSaltar, _forwardRange, _continueMask)) //Si para la direccion que quiero ir no hay mas agarradera y puedo saltar, lo hago
            {
                StartCoroutine(WaitingNormalMovement());
                return;
            }

            if (!Physics.Raycast(vistaEnredadera, _forwardRange, _handleMask)) //Si para la direccion que quiero ir no hay mas agarradera, no sigo
            {
                _stopGrabb = true;
                return;
            }

            //if (IsTouch(dirEscalando, _moveMask))
            //{

            //    ActualMove = NormalMovement;
            //    actualStatePlayer = EstadoDePlayer.Normal;

            //    return;
            //}

            _stopGrabb = false;
            _rbCharacter.MovePosition(transform.position + dirEscalando * _actualSpeed * Time.fixedDeltaTime);

        }
        else
        {
            _dirGrabb = default;
        }
    }

    IEnumerator WaitingNormalMovement()
    {
        _rbCharacter.isKinematic = false;
        _rbCharacter.velocity = transform.up * _jumpForceAxiY * 1.5f;
        StartCoroutine(WaitRayChange());

        yield return new WaitForSeconds(0.1f);

        _forceGravity = _initialForceGravity;
        _rbCharacter.velocity = transform.forward * 10;

        yield return new WaitForSeconds(0.15f);

        ActualMove = NormalMovement;
        actualStatePlayer = EstadoDePlayer.Normal;
    }

    public void CancelarTodasLasFuerzas()
    {
        _forceGravity = 0.05f;
        _rbCharacter.velocity = Vector3.zero; // Establece la velocidad del Rigidbody a cero
        _rbCharacter.angularVelocity = Vector3.zero; // Establece la velocidad angular del Rigidbody a cero
        _rbCharacter.Sleep(); // Detiene toda la simulación dinámica en el Rigidbody
    }

    #endregion

    #region JUMP
    public void Jump()
    {

        if (actualStatePlayer == EstadoDePlayer.Escalando && _dirGrabb.sqrMagnitude != 0 && _stopGrabb) //Si estoy escalando
        {
            //actualStateBongo = EstadoDeBongo.Quieto;
            //StartCoroutine(WaitRayChange());

            //if (_dirGrabb.sqrMagnitude != 0 && _stopGrabb)
            //{
            //    StartCoroutine(WaitRayChange());
            //    ActualMove = NormalMovement;

            //    _jumpGrabb = true;
            //    _rbCharacter.isKinematic = false;
            //    _forceGravity = _initialForceGravity;

            //    //Depende la direccion de donde quiera ir, va a saltar
            //    if (_dirGrabb.y > 0) _rbCharacter.velocity = new Vector3(0, _dirGrabb.y * _jumpForceAxiY * 2);
            //    else if (_dirGrabb.x != 0) _rbCharacter.velocity = new Vector3(_dirGrabb.x * _jumpForceAxiX, _jumpForceAxiY);

            //    _dirGrabb = default;
            //}

            StartCoroutine(WaitRayChange());
            ActualMove = NormalMovement;

            _jumpGrabb = true;
            _rbCharacter.isKinematic = false;
            _forceGravity = _initialForceGravity;

            //Depende la direccion de donde quiera ir, va a saltar
            if (_dirGrabb.y > 0) _rbCharacter.velocity = new Vector3(0, _dirGrabb.y * _jumpForceAxiY * 2);
            else if (_dirGrabb.x != 0) _rbCharacter.velocity = new Vector3(_dirGrabb.x * _jumpForceAxiX, _jumpForceAxiY);

            _dirGrabb = default;

            //else if(_dirGrabb.sqrMagnitude == 0) //Si no, salta en direccion opuesta a la agarradera
            //{
            //    ActualMove = NormalMovement;

            //    Vector3 oppDir = -transform.forward;
            //    Vector3 jumpDir = new Vector3(oppDir.x, _jumpForceAxiY, oppDir.z * _jumpForceAxiZ);
            //    _forceGravity = _initialForceGravity;
            //    _rbCharacter.isKinematic = false;
            //    _rbCharacter.velocity = jumpDir;
            //}
        }

        else if ( actualStatePlayer == EstadoDePlayer.Normal && _coyoteTimeCounter > 0f)
        {
            _animPlayer?.SetTrigger("Jump");
            _particleJump?.Play();
            _rbCharacter.velocity = Vector3.up * _jumpForce;
            AudioManager.instance.PlayMonkeySFX(AudioManager.instance.jump);
        }
    }

    IEnumerator WaitRayChange()
    {
        _waitRay = true;
        yield return new WaitForSeconds(0.2f);
        _waitRay = false;
    }
    #endregion

    #region ATTACKS
    public void Attack()
    {
        if (IsGrounded()) NormalPunch();
    }

    protected void NormalPunch()
    {
        if (actualStatePlayer == EstadoDePlayer.Golpeando) return;

        AudioManager.instance.PlayMonkeySFX(AudioManager.instance.swoosh);

        StartCoroutine(SystemNormalCombo());
    }

    IEnumerator SystemNormalCombo()
    {
        actualStatePlayer = EstadoDePlayer.Golpeando;
        _animPlayer.SetTrigger("Attack");
        yield return new WaitForSeconds(0.4f);
        actualStatePlayer = EstadoDePlayer.Normal;
        yield return new WaitForSeconds(0.2f);

        if (actualStatePlayer == EstadoDePlayer.Normal)
        {
            _actualSpeed = _speed;
            _forceGravity = _initialForceGravity;

        }
    }
    #endregion

    #region DAMAGE / LIFE
    public void TakeDamageEntity(float dmg, Vector3 target)
    {
        if (_actualLife > 0)
        {
            _actualLife -= dmg;

            if (_actualLife <= 0) Dead();

            EventManager.Trigger("ProjectLifeBar", _maxLife, _actualLife);
        }
    }

    public void Heal(float life)
    {
        if (_actualLife < _maxLife)
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

    #region Memento
    public override void Save()
    {
        _currentState.Rec(transform.position, transform.rotation, _actualLife, actualStatePlayer);
        //Debug.Log("guarde mono");
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        transform.position = (Vector3)col.parameters[0];
        transform.rotation = (Quaternion)col.parameters[1];
        _actualLife = (float)col.parameters[2];
        actualStatePlayer = (EstadoDePlayer)col.parameters[3];

        EventManager.Trigger("ProjectLifeBar", _maxLife, _actualLife);

        //Debug.Log("cargue mono");
    }
    #endregion
}
