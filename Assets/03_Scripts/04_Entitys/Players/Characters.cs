using System;
using System.Collections;
using UnityEngine;


public enum EstadoDePlayer
{
    Normal,
    Caminando,
    Escalando    
}

public enum TypeFloor
{
    Normal,
    Cinta,
    Hielo
}

public abstract class Characters : Entity, IDamageable
{
    protected Rigidbody _rbCharacter;
    public Vector3 _myVelocityCharacter;
    protected Animator _animPlayer;

    [Tooltip("Para mostar el velocity")]public bool mostrar;

    [Header("--- VALUE CHARACTERS ---")]

    [Space(10)]public EstadoDePlayer actualStatePlayer;
    public bool isMovementInverse;

    [Header("--> GENERAL")]
    [SerializeField] protected float _maxLife = 1f;
    protected float _actualLife;
    [SerializeField] protected float _iniSpeed = 10f;
    public float actualSpeed;
    private Vector3 _dir;
    [SerializeField] protected float _forceGravity = 1.25f;
    protected float _initialForceGravity;
    [Space(10), SerializeField, Tooltip("Fuerza de salto normal")] protected float _jumpForce = 25f;
    public bool isJumping;
    private bool _isImpulse;
    [SerializeField, Range(0, 0.4f), Tooltip("Tiempo para saltar cuando dejo de tocar el suelo")] protected float _coyoteTime = 0.15f;
    protected float _coyoteTimeCounter;
    [Space(10), SerializeField, Range(0, 0.1f), Tooltip("Cuanto mas alto el valor, mas se resbala")] private float _iceFriction = 0.65f;
    [SerializeField,Range(0, 15f),Tooltip("Velocidad máxima cuando se salta en el hielo")] private float _maxSpeedJumpIce;
    private bool _isInIce;
    public bool IsInIce { get { return _isInIce; } }
    //[SerializeField, Tooltip("Daño de golpe")] protected int _normalDamage = 1;

    [Header("--> CLIMB")]
    [SerializeField, Range(1, 40), Tooltip("Fuerza de salto en el eje X")] protected float _jumpForceAxiX = 20;
    [SerializeField, Range(1, 40), Tooltip("Fuerza de salto en el eje Y")] protected float _jumpForceAxiY = 15;
    protected bool _isJumpGrabb;
    protected bool _isWaitRay;

    [Header("--> RAYCASTS")]
    [SerializeField, Range(0.1f, 3f) , Tooltip("Rango del raycast para el coyote time")] protected float _groundRange = 2;
    [SerializeField, Tooltip("Layer de objeto en donde pueda saltar")] protected LayerMask _floorLayer;
    [SerializeField, Tooltip("Tipo de piso")] public TypeFloor _typeFloor;
    [Space(10),SerializeField] private LayerMask _iceLayer;
    [Space(10),SerializeField, Range(0.1f, 2f) , Tooltip("Rango del raycast para las colisiones")] protected float _forwardRange = 0.75f; //Rango para el Raycast para evitar q el PJ se pegue a la pared
    [SerializeField, Tooltip("Que Layer no quiero que se acerque")] protected LayerMask _moveMask; //Para indicar q layer quierO q no se acerque mucho
    [Space(10),SerializeField, Tooltip("Layer de Enredaderas u objeto a trepar")] protected LayerMask _handleMask;
    [SerializeField, Tooltip("Layer de objeto para seguir")] protected LayerMask _continueMask;
    protected Ray _moveRay;
    public bool isStopMove;
    protected Vector3 _dirGrabb; //Direccion de movimiento en la agarradera
    protected bool _isStopGrabb;

    [Header("--> PARTICLES")]
    [SerializeField] protected ParticleSystem _particleJump;

    //public delegate void MyDelegate(Vector3 dirRaw);
    //public event MyDelegate _actualMove;
    //public MyDelegate ActualMove { get { return _actualMove; } set { _actualMove = value; } }

    public Action<Vector3> ActualMove;

    public delegate void MyDelegateEvent();
    public event MyDelegateEvent ActualJump;

    [HideInInspector] public CheckPoint actualCheckpoint;

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
        actualSpeed = _iniSpeed;
        _initialForceGravity = _forceGravity;

        ActualMove = NormalMovement;
        ActualJump = NormalJump;
        
    }

    public virtual void Update()
    {
        CoyoteTime();

        if(mostrar) Debug.Log(_rbCharacter.velocity);
    }

    public virtual void FixedUpdate()
    {
        _rbCharacter.AddForce(Vector3.down * _forceGravity, ForceMode.VelocityChange);
    }

    void CoyoteTime()
    {
        
        if (IsGrounded(_iceLayer) && Mathf.Abs(_rbCharacter.velocity.y) <= 0.01f)
        {
            if(_rbCharacter.drag != 0)_rbCharacter.drag = 0;

            _isInIce = true;

            _animPlayer.SetBool("IsGrounded", true);
            _coyoteTimeCounter = _coyoteTime;
        }

        //Si toco el suelo
        else if (IsGrounded(_floorLayer) && Mathf.Abs(_rbCharacter.velocity.y) <= 10f)
        {
            if(_rbCharacter.drag != 1) _rbCharacter.drag = 1;

            _isInIce = false;
            isJumping = false;
            _isJumpGrabb = false;

            _animPlayer.SetBool("IsGrounded", true);
            _coyoteTimeCounter = _coyoteTime;

        }

        else
        {
            //if(_rbCharacter.velocity.x == 0 && _rbCharacter.velocity.z == 0) _isInIce = false;
            if(_rbCharacter.velocity.x <= 5 && _rbCharacter.velocity.x >= -5  &&  _rbCharacter.velocity.z <= 5 && _rbCharacter.velocity.z >= -5) _isInIce = false;

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
    /// Si estoy tocando algun objeto debajo mio, correspondiente a la layer
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded(int layer = default)
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        RaycastHit hit;


        return Physics.Raycast(pos, dir, out hit, _groundRange, layer);
    }

    #endregion

    #region MOVEMENT
    public virtual void Movement(Vector3 dirRaw = default)
    {

        if (IsTouch(dirRaw.normalized, _moveMask)) //Si estoy tocando una pared
        {
            isStopMove = true;
            _animPlayer.SetBool("IsWallDetected", true);
            transform.forward = dirRaw;
            return;
        }
        else
        {
            _animPlayer.SetBool("IsWallDetected", false);
            isStopMove = false;
        }
  

        ActualMove(dirRaw);
        
    }

    public void Rotate(Vector3 dirForward) => transform.forward = dirForward;

    public void ApplyForce(float force, Vector3 dir)
    {
        _rbCharacter.velocity += (dir * force);
    }

    public void InverseMovement(Vector3 dirRaw)
    {
        NormalMovement(-dirRaw);
    }

    private void NormalMovement(Vector3 dirRaw)
    {
        //if (_isJumpGrabb) return;

        //if(actualStatePlayer != EstadoDePlayer.Normal) actualStatePlayer = EstadoDePlayer.Normal;

        _dirGrabb = default;
        _dir = dirRaw;

        //Dejar por las dudas!!!
        //if(_rbCharacter.isKinematic) _rbCharacter.isKinematic = false;
        //_rbCharacter.MovePosition(transform.position + (dir.normalized * _actualSpeed * Time.fixedDeltaTime));

        _myVelocityCharacter = Vector3.zero;

        if (dirRaw.sqrMagnitude != 0)
        {
            actualStatePlayer = EstadoDePlayer.Caminando;

            Vector3 mov = new Vector3(dirRaw.normalized.x, 0, dirRaw.normalized.z);
            _myVelocityCharacter = mov * actualSpeed;
        }
        else actualStatePlayer = EstadoDePlayer.Normal;

        //Vector3 mov = new Vector3(dir.normalized.x, 0, dir.normalized.z);
        //_myVelocityCharacter = mov * _actualSpeed;

        //Rotate(mov);

        _myVelocityCharacter.y = _rbCharacter.velocity.y;

        if (_isInIce)
        {
            float force = 0f;
            // Aplica la fuerza en el suelo con fricción de hielo
            if (IsGrounded(_iceLayer))
            {
                force = _iceFriction;
            }
            else
            {
                force = _iceFriction / _maxSpeedJumpIce;
            }

            _rbCharacter.AddForce(new Vector3(_myVelocityCharacter.x * force, 0, _myVelocityCharacter.z * force), ForceMode.VelocityChange);
        }
        else
        {
            if(!_isImpulse) _rbCharacter.velocity = _myVelocityCharacter;
        }


        if (IsTouch(transform.forward, _handleMask) && !_isWaitRay) //Si toco algo escalable, cambio de movimiento
        {
            Debug.LogWarning("ENREDADERA DETECTADA");
            actualStatePlayer = EstadoDePlayer.Escalando;

            if (IsGrounded(_floorLayer)) _rbCharacter.velocity = transform.up * _jumpForce;

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

    public void HandleMovement(Vector3 dirRaw)
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
            Vector3 dirEscalando = new Vector3(dirRaw.normalized.x, dirRaw.normalized.z);
            _dirGrabb = dirEscalando;

            Ray vistaEnredadera = new Ray(transform.position + dirEscalando, transform.forward);
            Debug.DrawRay(transform.position + dirEscalando, transform.forward * _forwardRange, Color.green);

            RaycastHit hitInfo;
            if (Physics.Raycast(vistaEnredadera, out hitInfo, _forwardRange, _handleMask)) transform.forward = hitInfo.transform.forward; //Miro para la enredadera

            if (IsGrounded(_floorLayer)) 
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
            _rbCharacter.MovePosition(transform.position + dirEscalando * actualSpeed * Time.fixedDeltaTime);

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
        _rbCharacter.Sleep(); // Detiene toda la simulación dinámica en el Rigidbody
    }

    #endregion

    #region JUMP
    public void Jump()
    {
        ActualJump();

        #region No sirve por ahora
        //if (actualStatePlayer == EstadoDePlayer.Escalando && _dirGrabb.sqrMagnitude != 0 && _isStopGrabb) //Si estoy escalando
        //{

        //    //CREO Q NO SIRVE ESTO
        //    //StartCoroutine(WaitRayChange());

        //    //if (_dirGrabb.sqrMagnitude != 0)
        //    //{
        //    //    StartCoroutine(WaitRayChange());
        //    //    ActualMove = NormalMovement;

        //    //    _jumpGrabb = true;
        //    //    _rbCharacter.isKinematic = false;
        //    //    _forceGravity = _initialForceGravity;

        //    //    //Depende la direccion de donde quiera ir, va a saltar
        //    //    if (_dirGrabb.y > 0) _rbCharacter.velocity = new Vector3(0, _dirGrabb.y * _jumpForceAxiY * 2);
        //    //    else if (_dirGrabb.x != 0) _rbCharacter.velocity = new Vector3(_dirGrabb.x * _jumpForceAxiX, _jumpForceAxiY);

        //    //    _dirGrabb = default;
        //    //}

        //    //ESTO SI
        //    StartCoroutine(WaitRayChange());
        //    ActualMove = NormalMovement;

        //    _isJumpGrabb = true;
        //    _rbCharacter.isKinematic = false;

        //    //Depende la direccion de donde quiera ir, va a saltar
        //    if (_dirGrabb.y > 0) _rbCharacter.velocity = new Vector3(0, _dirGrabb.y * _jumpForceAxiY * 2);
        //    else if (_dirGrabb.x != 0) _rbCharacter.velocity = new Vector3(_dirGrabb.x * _jumpForceAxiX, _jumpForceAxiY);

        //    _dirGrabb = default;

        //    //ESTO NO SE
        //    //else if(_dirGrabb.sqrMagnitude == 0) //Si no, salta en direccion opuesta a la agarradera
        //    //{
        //    //    ActualMove = NormalMovement;

        //    //    Vector3 oppDir = -transform.forward;
        //    //    Vector3 jumpDir = new Vector3(oppDir.x, _jumpForceAxiY, oppDir.z * _jumpForceAxiZ);
        //    //    _forceGravity = _initialForceGravity;
        //    //    _rbCharacter.isKinematic = false;
        //    //    _rbCharacter.velocity = jumpDir;
        //    //}
        //}

        //else if ( actualStatePlayer != EstadoDePlayer.Escalando && _coyoteTimeCounter > 0) //0.08f
        //{
        //    isJumping = true;

        //    _coyoteTimeCounter = 0f;
        //    _animPlayer?.SetTrigger("Jump");
        //    _particleJump?.Play();

        //    _rbCharacter.velocity = new Vector3(_rbCharacter.velocity.x, _jumpForce, _rbCharacter.velocity.z);

        //    // Limitar la velocidad horizontal al saltar
        //    Vector3 horizontalVelocity = new Vector3(_rbCharacter.velocity.x, 0, _rbCharacter.velocity.z);

        //    if (horizontalVelocity.magnitude > _maxSpeedJumpIce)
        //    {
        //        // Limitar la velocidad horizontal a un máximo
        //        horizontalVelocity = horizontalVelocity.normalized * _maxSpeedJumpIce;
        //        _rbCharacter.velocity = new Vector3(horizontalVelocity.x, _rbCharacter.velocity.y, horizontalVelocity.z);
        //    }

        //    AudioManager.instance.Play(SoundId.Jump);
        //}
        #endregion
    }

    private void NormalJump()
    {
        if (_coyoteTimeCounter < 0) return;

        isJumping = true;

        _coyoteTimeCounter = 0f;
        _animPlayer?.SetTrigger("Jump");
        _particleJump?.Play();

        _rbCharacter.velocity = new Vector3(_rbCharacter.velocity.x, _jumpForce, _rbCharacter.velocity.z);

        AudioManager.instance.Play(SoundId.Jump);
    }

    private void IceJump()
    {
        if (_coyoteTimeCounter < 0) return;

        isJumping = true;

        _coyoteTimeCounter = 0f;
        _animPlayer?.SetTrigger("Jump");
        _particleJump?.Play();

        _rbCharacter.velocity = new Vector3(_rbCharacter.velocity.x, _jumpForce, _rbCharacter.velocity.z);

        // Limitar la velocidad horizontal al saltar
        Vector3 horizontalVelocity = new Vector3(_rbCharacter.velocity.x, 0, _rbCharacter.velocity.z);

        if (horizontalVelocity.magnitude > _maxSpeedJumpIce)
        {
            // Limitar la velocidad horizontal a un máximo
            horizontalVelocity = horizontalVelocity.normalized * _maxSpeedJumpIce;
            _rbCharacter.velocity = new Vector3(horizontalVelocity.x, _rbCharacter.velocity.y, horizontalVelocity.z);
        }
    }

    private void HandleJump()
    {
        if (_dirGrabb.sqrMagnitude == 0 && !_isStopGrabb) return;

        StartCoroutine(WaitRayChange());
        ActualMove = NormalMovement;

        _isJumpGrabb = true;
        _rbCharacter.isKinematic = false;

        //Depende la direccion de donde quiera ir, va a saltar
        if (_dirGrabb.y > 0) _rbCharacter.velocity = new Vector3(0, _dirGrabb.y * _jumpForceAxiY * 2);
        else if (_dirGrabb.x != 0) _rbCharacter.velocity = new Vector3(_dirGrabb.x * _jumpForceAxiX, _jumpForceAxiY);

        _dirGrabb = default;
    }

    IEnumerator WaitRayChange()
    {
        _isWaitRay = true;
        yield return new WaitForSeconds(0.1f);
        _isWaitRay = false;
    }
    #endregion

    #region ATTACKS
    public virtual void Attack()
    {
        //NormalPunch();
        //_animPlayer.SetTrigger("Attack");

    }

    //public void NormalPunch()
    //{
    //    if (actualStatePlayer == EstadoDePlayer.Golpeando) return;



    //    StartCoroutine(SystemNormalCombo());
    //}

    //IEnumerator SystemNormalCombo()
    //{
    //    actualStatePlayer = EstadoDePlayer.Golpeando;
    //    _animPlayer.SetTrigger("Attack");
    //    yield return new WaitForSeconds(0.4f);
    //    actualStatePlayer = EstadoDePlayer.Normal;
    //    yield return new WaitForSeconds(0.2f);

    //    if (actualStatePlayer == EstadoDePlayer.Normal)
    //    {
    //        _actualSpeed = _speed;
    //        _forceGravity = _initialForceGravity;

    //    }
    //}
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        var interact = other.GetComponent<IInteracteable>();
        var impulse = other.GetComponent<IImpulse>();

        if (interact != null) interact.Active();

        if (impulse != null) _isImpulse = true; 

    }

    private void OnTriggerExit(Collider other)
    {
        var interact = other.GetComponent<IInteracteable>();
        var impulse = other.GetComponent<IImpulse>();

        if (interact != null) interact.Deactive();

        if (impulse != null) _isImpulse = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var interact = collision.gameObject.GetComponent<IInteracteable>();

        if(interact != null) interact.Active();


        switch (collision.gameObject.layer)
        {
            case 6:
                {
                    _typeFloor = TypeFloor.Normal;
                    ActualJump = NormalJump;
                }
                break;

            case 13:
                {
                    _typeFloor = TypeFloor.Hielo;
                    ActualJump = IceJump;
                }
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
                                    //ICEBLOCK
        if (collision.gameObject.layer == 14 && _myVelocityCharacter != Vector3.zero)
        {
            _animPlayer.SetBool("IsWallDetected", true);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 14)
        {
            _animPlayer.SetBool("IsWallDetected", false);
        }
    }

    #region DAMAGE / LIFE
    public void TakeDamageEntity(float dmg, Vector3 target)
    {
        //if (_actualLife > 0)
        //{
        //    _actualLife -= dmg;

        //    if (_actualLife <= 0) Dead();

        //    EventManager.Trigger("ProjectLifeBar", _maxLife, _actualLife);
        //}

        Dead();
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
            transform.position = actualCheckpoint.SpawnPoint.position;
            transform.rotation = actualCheckpoint.SpawnPoint.rotation;
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
