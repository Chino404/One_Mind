using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EstadoDeBongo
{
    Normal,
    Escalando,
    Golpeando,
}
public abstract class Characters : Entity
{
    protected Rigidbody _rbCharacter;
    protected Animator _animPlayer;

    [Header("--- VALUE CHARACTERS ---")]

    public EstadoDeBongo actualStateBongo;

    [Space(10), Header("--> GENERAL")]
    [SerializeField] protected float _maxLife = 3f;
    [SerializeField] protected float _actualLife;
    [SerializeField] protected float _speed = 10f;
    protected float _actualSpeed;
    [SerializeField] protected float _forceGravity = 1.25f;
    protected float _initialForceGravity;
    [SerializeField, Tooltip("Fuerza de salto normal")] protected float _jumpForce = 25f;
    [SerializeField, Range(0, 0.4f), Tooltip("Tiempo para saltar cuando dejo de tocar el suelo")] protected float _coyoteTime = 0.2f;
    protected float _coyoteTimeCounter;
    [SerializeField, Tooltip("Daño de golpe")] protected int _normalDamage;
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
    protected Ray _moveRay;
    protected bool _stopGrabb;
    protected Vector3 _dirGrabb; //Direccion de movimiento en la agarradera

    [Header("--> PARTICLES")]
    [SerializeField] protected ParticleSystem _particleSpinAttack;
    [SerializeField] protected ParticleSystem _particleJump;
    [SerializeField] protected ParticleSystem _polvo;

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

    #region RAYCAST

    /// <summary>
    /// Si el Ray choca con un objeto de la layer asignada, me lo indica con un TRUE
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    protected bool IsTouch(Vector3 dir, int layerMask)
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
        if (actualStateBongo == EstadoDeBongo.Golpeando || IsTouch(dir.normalized, _moveMask)) return;

        ActualMove(dirRaw, dir);
    }

    protected void ChangeSpeed()
    {
        if (actualStateBongo == EstadoDeBongo.Normal) _actualSpeed = _speed;
        if (actualStateBongo == EstadoDeBongo.Escalando) _actualSpeed = 7;
        if (actualStateBongo == EstadoDeBongo.Golpeando) _actualSpeed = 0;
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
                actualStateBongo = EstadoDeBongo.Escalando;

                ActualMove = HandleMovement;
                _jumpGrabb = false;
                return;
            }

            _animPlayer.SetBool("Walk", true);
        }

        else
        {
            _animPlayer.SetBool("Walk", false);
        }
    }

    public void HandleMovement(Vector3 dirRaw, Vector3 dir)
    {
        _animPlayer.SetBool("Walk", false);

        _forceGravity = 0;
        _rbCharacter.isKinematic = true;

        if (dirRaw.sqrMagnitude != 0)
        {
            Vector3 dirEscalando = new Vector3(dir.normalized.x, dir.normalized.z);
            _dirGrabb = dirEscalando;

            Ray vistaEnredadera = new Ray(transform.position + dirEscalando, transform.forward);
            Debug.DrawRay(transform.position + dirEscalando, transform.forward * _forwardRange, Color.green);

            RaycastHit hitInfo;
            if (Physics.Raycast(vistaEnredadera, out hitInfo)) transform.forward = hitInfo.transform.forward; //Miro para la enredadera

            if (!Physics.Raycast(vistaEnredadera, _forwardRange, _handleMask)) //Si para la direccion que quiero ir no hay mas agarradera, no sigo
            {
                _stopGrabb = true;
                return;
            }

            if (IsTouch(dirEscalando, _moveMask))
            {
                ActualMove = NormalMovement;
                actualStateBongo = EstadoDeBongo.Normal;

                return;
            }

            _stopGrabb = false;
            _rbCharacter.MovePosition(transform.position + dirEscalando * _actualSpeed * Time.fixedDeltaTime);

        }
        else
        {
            _dirGrabb = default;
        }
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

        if (actualStateBongo == EstadoDeBongo.Escalando) //Si estoy escalando
        {
            //actualStateBongo = EstadoDeBongo.Quieto;
            StartCoroutine(WaitRayChange());

            if (_dirGrabb.sqrMagnitude != 0 && _stopGrabb)
            {
                ActualMove = NormalMovement;

                _jumpGrabb = true;
                _rbCharacter.isKinematic = false;
                _forceGravity = _initialForceGravity;

                //Depende la direccion de donde quiera ir, va a saltar
                if (_dirGrabb.y > 0) _rbCharacter.velocity = new Vector3(0, _dirGrabb.y * _jumpForceAxiY * 2);
                else if (_dirGrabb.x != 0) _rbCharacter.velocity = new Vector3(_dirGrabb.x * _jumpForceAxiX, _jumpForceAxiY);

                _dirGrabb = default;
            }

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

        else if ( actualStateBongo == EstadoDeBongo.Normal && _coyoteTimeCounter > 0f)
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
}
