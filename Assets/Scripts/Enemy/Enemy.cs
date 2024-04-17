using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    [SerializeField] private float _dmg;
    [SerializeField] private float _life = 100;
    [SerializeField] private float _forceGravity;
    private float _initalForceGravity;
    [SerializeField]private float _timeInvencible = 0.5f;
    private float _timerCounterInveencible;
    [SerializeField] private bool _takingDamage;
    [SerializeField] private float _recoilForce = 10f;

    [Header("Object Pool")]
    public float counter;
    ObjectPool<Enemy> _objectPool;

    [Header("Reference")]
    public float groundDistance = 1.3f;
    [SerializeField] private LayerMask _floorLayer;
    private Rigidbody _rigidbody;
    [SerializeField]private bool _inAir;



    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _initalForceGravity = _forceGravity;
        _timerCounterInveencible = _timeInvencible;
    }


    void Update()
    {
        
        _inAir = IsGrounded() ? false : true;

        if (_takingDamage)
        {
            _timerCounterInveencible -= Time.deltaTime;

            if (_timerCounterInveencible <= 0)
            {
                _takingDamage = false;
                _timerCounterInveencible = _timeInvencible;

                if(_inAir) StartCoroutine(ReturnGravity());
            }
        }


        if(counter>=2)
        {
            _objectPool.StockAdd(this);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(Vector3.down * _forceGravity, ForceMode.VelocityChange);
    }

    public void AddReference(ObjectPool<Enemy> en)
    {
        _objectPool = en;
    }

    public static void TurnOff(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    public static void TurnOn(Enemy enemy)
    {
        enemy.counter = 0;
        enemy.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.gameObject.GetComponent<IDamageable>();

        if(damageable != null)
        {
            damageable.TakeDamageEntity(_dmg, transform.position);
        }
    }

    public void TakeDamageEntity(float dmg, Vector3 target)
    {
        _takingDamage = true;
        _timerCounterInveencible = _timeInvencible;

        ViewTakeDamage(dmg, target);

        if (!_inAir) _rigidbody.AddForce(-transform.forward * _recoilForce, ForceMode.VelocityChange);
        else CancelarTodasLasFuerzas();

    }

    public void GetUpDamage(float dmg, Vector3 target, float forceToUp)
    {
        _takingDamage = true;
        _timerCounterInveencible = _timeInvencible;

        ViewTakeDamage(dmg, target);

        _rigidbody.velocity = Vector3.up * forceToUp;
    }

    private void ViewTakeDamage(float damage, Vector3 viewTarget)
    {
        if (_life > 0)
        {
            _life -= damage;

            if (_life <= 0) _life = 0;
        }

        Vector3 direction = viewTarget - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }

    public bool IsGrounded()
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        float dist = groundDistance;

        Debug.DrawLine(pos, pos + (dir * dist));

        return Physics.Raycast(pos, dir, out RaycastHit hit, dist, _floorLayer);
    }

    void CancelarTodasLasFuerzas()
    {
        _forceGravity = 0.05f;
        _rigidbody.velocity = Vector3.zero; // Establece la velocidad del Rigidbody a cero
        _rigidbody.angularVelocity = Vector3.zero; // Establece la velocidad angular del Rigidbody a cero
        _rigidbody.Sleep(); // Detiene toda la simulación dinámica en el Rigidbody
    }

    IEnumerator ReturnGravity()
    {
        yield return new WaitForSeconds(0.15f);
        if(!_takingDamage)_forceGravity = _initalForceGravity;
    }
}
