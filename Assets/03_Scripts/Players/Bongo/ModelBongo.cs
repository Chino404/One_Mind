using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-1)]
[RequireComponent(typeof(Rigidbody))]
public class ModelBongo : Characters
{
    //Referencias
    public static ModelBongo instance;
    private ControllerBongo _controller;
    private ViewBongo _view;

    [Header("--- BONGO'S VALUES ---")]

    [Space(15), Header("-> Glide")]
    [SerializeField] private bool _isGetPenguin;
    public bool IsGetPenguin { set {  _isGetPenguin = value; } }
    [SerializeField, Range(0.1f, 0.9f)] private float _gravityPlan = 0.3f;
    [SerializeField, Range(0.1f, 0.9f) , Tooltip("Tiempo para planear")] private float _timeToPlane = 0.25f;
    [SerializeField, Tooltip("Tiempo de tecla presionado")] private float _keyPressTime;

    [Space(15),Header("-> Spell")]
    [SerializeField, Tooltip("Prefab del Spell")] private Bullet _spellPrefab;
    private Vector3 _dirSpells;
    public Vector3 DirSpells { set { _dirSpells = value; } }
    [SerializeField, Tooltip("Cantidad de balas que instancio al principio")] int _bulletQuantity;

    [SerializeField, Tooltip("Segundos de cooldown")] float _seconds;
    [HideInInspector] public float valueScroll;
    bool _coolDown;

    private FixativeObjectUI _targetLock;
    private Factory<Bullet> _factory;
    private ObjectPool<Bullet> _objectPool;


    public override void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        GameManager.instance.bongo = instance;
        base.Awake();

        _view = new ViewBongo(this, _animPlayer);
        _controller = new ControllerBongo(this, _view);

        _currentState = new MementoState();
        _targetLock = GetComponent<FixativeObjectUI>();
    }

    public override void Start()
    {
        base.Start();

        _factory = new BulletFactory(_spellPrefab);
        _objectPool = new ObjectPool<Bullet>(_factory.GetObj, Bullet.TurnOff, Bullet.TurnOn, _bulletQuantity);


        ActualMove = NormalMovement;
    }

    public override void Update()
    {
        base.Update();

        _controller.ArtificialUpdate();

        _targetLock.TargetLockA(valueScroll);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _controller.ListenFixedKeys();
    }

    /// <summary>
    /// Planeo
    /// </summary>
    /// <param name="isPressed"></param>
    public void Glide(bool isPressed)
    {
        if (!_isGetPenguin) return;

        if (!isPressed)
        {
            _forceGravity = _initialForceGravity;
            _keyPressTime = 0;
        }

        else
        {
            _keyPressTime += Time.deltaTime;

            if (_keyPressTime >= _timeToPlane)
            {
                _forceGravity = _gravityPlan;
                Vector3 dir = transform.forward;

                Movement(dir, dir);

                //_rbCharacter.velocity = transform.forward * _speed;

                //_rbCharacter.velocity = _rbCharacter.transform.forward * _speed * Time.deltaTime;

                //if(!IsGrounded(_floorLayer)) transform.position += transform.forward * _speed * Time.deltaTime;
                //if(!IsGrounded(_floorLayer)) _rbCharacter.velocity = transform.forward * _speed;
            }
        }
    }

    public override void Attack()
    {
        //base.Attack();

        //if (!_coolDown) StartCoroutine(Shoot());

    }

    IEnumerator Shoot()
    {
        _coolDown = true;
        var bullet = _objectPool.Get(); //Pido un Spell al _objectPool
        bullet.AddReference(_objectPool); //Le añado la referencia a mi prefab
        bullet.transform.position = transform.position; //Le asigno el lugar por donde va a salir

        if(_dirSpells != Vector3.zero) bullet.transform.forward = _dirSpells.normalized;
        else bullet.transform.forward = transform.forward; //Asigno su dirección

        yield return new WaitForSeconds(_seconds);
        _coolDown = false;

    }

}
