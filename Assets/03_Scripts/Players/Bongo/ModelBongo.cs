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

    public override void Attack()
    {
        //base.Attack();

        if (!_coolDown) StartCoroutine(Shoot());

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
