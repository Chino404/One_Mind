using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EstadoDePlayer
{
    Normal,
    Escalando,
    Golpeando
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
    [SerializeField] protected float _coyoteTimeCounter;
    [SerializeField, Tooltip("Da�o de golpe")] protected int _normalDamage = 1;
    protected Vector3 _launchDir;

    [Header("--> CLIMB")]
    [SerializeField, Range(1, 40), Tooltip("Fuerza de salto en el eje X")] protected float _jumpForceAxiX = 20;
    [SerializeField, Range(1, 40), Tooltip("Fuerza de salto en el eje Y")] protected float _jumpForceAxiY = 15;
    //[SerializeField, Range(1, 40), Tooltip("Fuerza de salto en el eje Z, direcci�n opuesta a la enredadera")] protected float _jumpForceAxiZ = 10;
    protected bool _isJumpGrabb;
    protected bool _isWaitRay;

    [Header("--> RAYCASTS")]
    [SerializeField, Range(0.1f, 3f) , Tooltip("Rango del raycast para el coyote time")] protected float _groundRange = 2;
    [SerializeField, Tooltip("Layer de objeto en donde pueda saltar")] protected LayerMask _floorLayer;
    [SerializeField, Range(0.1f, 2f) , Tooltip("Rango del raycast para las colisiones")] protected float _forwardRange = 0.75f; //Rango para el Raycast para evitar q el PJ se pegue a la pared
    [SerializeField, Tooltip("Que Layer no quiero que se acerque")] protected LayerMask _moveMask; //Para indicar q layer quierO q no se acerque mucho
    [SerializeField, Tooltip("Layer de Enredaderas u objeto a trepar")] protected LayerMask _handleMask;
    [SerializeField, Tooltip("Layer de objeto para seguir")] protected LayerMask _continueMask;
    protected Ray _moveRay;
    public bool isStopMove;
    protected Vector3 _dirGrabb; //Direccion de movimiento en la agarradera
    protected bool _isStopGrabb;

    [Header("--> PARTICLES")]
    [SerializeField] protected ParticleSystem _particleJump;
    //[SerializeField] protected ParticleSystem _polvo;

    public delegate void MyDelegate(Vector3 dirRaw, Vector3 dir);
    public event MyDelegate _actualMove;
    public MyDelegate ActualMove { get { return _actualMove; } set { _actualMove = value; } }

    [HideInInspector] public CheckPoint actualCheckpoint;

    [HideInInspector] public PlatformController currentPlatform;

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
        CoyoteTime();

        if (currentPlatform != null)
        {
            // A�ado el movimiento de la plataforma a la posici�n del jugador
            _rbCharacter.MovePosition(_rbCharacter.position + currentPlatform.GetPlatformMovement());

            // Primero, ajusto la posici�n del jugador en funci�n de la rotaci�n de la plataforma
            Vector3 relativePos = _rbCharacter.position - currentPlatform.transform.position;
            relativePos = currentPlatform.GetPlatformRotation() * relativePos;
            _rbCharacter.MovePosition(currentPlatform.transform.position + relativePos);

            // Despu�s, roto al jugador con la plataforma (si se desea que el jugador rote)
            _rbCharacter.MoveRotation(currentPlatform.GetPlatformRotation() * _rbCharacter.rotation);
        }
    }

    public virtual void FixedUpdate()
    {
        _rbCharacter.AddForce(Vector3.down * _forceGravity, ForceMode.VelocityChange);
    }

    void CoyoteTime()
    {
        if (IsGrounded())
        {
            _animPlayer.SetBool("IsGrounded", true);
            _coyoteTimeCounter = _coyoteTime;

            _isJumpGrabb = false;
        }
        else
        {
            _animPlayer.SetBool("IsGrounded", false);
            _coyoteTimeCounter -= Time.deltaTime;
        }
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

        return Physics.Raycast(pos, dir, out RaycastHit hit, _groundRange, _floorLayer);
    }

    #endregion

    #region MOVEMENT
    public virtual void Movement(Vector3 dirRaw, Vector3 dir)
    {
        if (/*actualStatePlayer == EstadoDePlayer.Golpeando || */IsTouch(dir.normalized, _moveMask))
        {
            isStopMove = true;
            _animPlayer.SetBool("IsWallDetected", true);
            transform.forward = dir;
            //_animPlayer.SetBool("Walk", false);
            return;
        }
        else
        {
            _animPlayer.SetBool("IsWallDetected", false);
            isStopMove = false;
        }


        ActualMove(dirRaw, dir);
    }

    public void Rotate(Vector3 dirForward) => transform.forward = dirForward;

   

    public void NormalMovement(Vector3 dirRaw, Vector3 dir)
    {
        if(actualStatePlayer != EstadoDePlayer.Normal) actualStatePlayer = EstadoDePlayer.Normal;

        _dirGrabb = default;

        //if(_rbCharacter.isKinematic) _rbCharacter.isKinematic = false;

        if (dirRaw.sqrMagnitude != 0)
        {

            _launchDir = dir;

            if (!_isJumpGrabb)
            {

                _rbCharacter.MovePosition(transform.position + dir.normalized * _actualSpeed * Time.fixedDeltaTime);
                Rotate(dir);
            }
        }

        if (IsTouch(transform.forward, _handleMask) && !_isWaitRay) //Si toco algo escalable, cambio de movimiento
        {
            Debug.LogWarning("ENREDADERA DETECTADA");
            actualStatePlayer = EstadoDePlayer.Escalando;
            if (IsGrounded()) _rbCharacter.velocity = transform.up * _jumpForce;
            StartCoroutine(WaitingKinematic(true));

            ActualMove = HandleMovement;
            _isJumpGrabb = false;
            return;
        }
    }

    IEnumerator WaitingKinematic(bool value)
    {
        yield return new WaitForSeconds(0.1f);
        _rbCharacter.isKinematic = value;
    }

    public void HandleMovement(Vector3 dirRaw, Vector3 dir)
    {
        _animPlayer.SetBool("Walk", false);

        if (actualStatePlayer == EstadoDePlayer.Normal)
        {
            StartCoroutine(WaitRayChange());
            ActualMove = NormalMovement;
            return;
        }

        _animPlayer.SetFloat("xAxi", dirRaw.x);
        _animPlayer.SetFloat("yAxi", dirRaw.z);

        //if(!_rbCharacter.isKinematic) _rbCharacter.isKinematic = true;

        if (dirRaw.sqrMagnitude != 0)
        {
            Vector3 dirEscalando = new Vector3(dir.normalized.x, dir.normalized.z);
            _dirGrabb = dirEscalando;

            Ray vistaEnredadera = new Ray(transform.position + dirEscalando, transform.forward);
            Debug.DrawRay(transform.position + dirEscalando, transform.forward * _forwardRange, Color.green);

            RaycastHit hitInfo;
            if (Physics.Raycast(vistaEnredadera, out hitInfo, _forwardRange, _handleMask)) transform.forward = hitInfo.transform.forward; //Miro para la enredadera

            if (IsGrounded()) 
            {
                _rbCharacter.isKinematic = false;
                ActualMove = NormalMovement;
                actualStatePlayer = EstadoDePlayer.Normal;
                return;
            }

            Ray vistaParaSaltar = new Ray(transform.position + (dirEscalando * 1.5f), transform.forward);
            Debug.DrawRay(transform.position + (dirEscalando * 1.5f), transform.forward * _forwardRange, Color.blue);

            if (Physics.Raycast(vistaParaSaltar, _forwardRange, _continueMask)) //Si para la direccion que quiero ir no hay mas agarradera y puedo saltar, lo hago
            {
                StartCoroutine(WaitingNormalMovement());
                return;
            }

            if (!Physics.Raycast(vistaEnredadera, _forwardRange, _handleMask)) //Si para la direccion que quiero ir no hay mas agarradera, no sigo
            {
                _isStopGrabb = true;
                return;
            }

            _isStopGrabb = false;
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
        _rbCharacter.Sleep(); // Detiene toda la simulaci�n din�mica en el Rigidbody
    }

    #endregion

    #region JUMP
    public void Jump()
    {

        if (actualStatePlayer == EstadoDePlayer.Escalando && _dirGrabb.sqrMagnitude != 0 && _isStopGrabb) //Si estoy escalando
        {
            //StartCoroutine(WaitRayChange());

            //if (_dirGrabb.sqrMagnitude != 0)
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

            _isJumpGrabb = true;
            _rbCharacter.isKinematic = false;

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
            _coyoteTimeCounter = 0f;
            _animPlayer?.SetTrigger("Jump");
            _particleJump?.Play();
            _rbCharacter.velocity = Vector3.up * _jumpForce;

            AudioManager.instance.Play(SoundId.Jump);
            //OldAudioManager.instance.PlayMonkeySFX(OldAudioManager.instance.jump);
        }
    }

    IEnumerator WaitRayChange()
    {
        _isWaitRay = true;
        yield return new WaitForSeconds(0.1f);
        _isWaitRay = false;
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

        OldAudioManager.instance.PlayMonkeySFX(OldAudioManager.instance.swoosh);

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

    private void OnTriggerEnter(Collider other)
    {
        var interact = other.GetComponent<IInteracteable>();

        if (interact != null) interact.Interact();

    }

    private void OnTriggerExit(Collider other)
    {
        var interact = other.GetComponent<IInteracteable>();

        if (interact != null) interact.Disconnect();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var interact = collision.gameObject.GetComponent<IInteracteable>();

        if(interact != null) interact.Interact();
    }

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
        if (actualCheckpoint != null)
        {
            transform.position = actualCheckpoint.spawnPoint.position;
            transform.rotation = actualCheckpoint.spawnPoint.rotation;
        }
        else
        {
            transform.position = (Vector3)col.parameters[0];
            transform.rotation = (Quaternion)col.parameters[1];
        }


        _actualLife = (float)col.parameters[2];
        actualStatePlayer = (EstadoDePlayer)col.parameters[3];
        _rbCharacter.isKinematic = false;

        EventManager.Trigger("ProjectLifeBar", _maxLife, _actualLife);

        //Debug.Log("cargue mono");
    }
    #endregion

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        float dist = _groundRange;

        Debug.DrawLine(pos, pos + (dir * dist));
    }
}
