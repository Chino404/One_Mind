using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    [SerializeField] private float _dmg;
    [SerializeField] private float _life;
    [SerializeField] private float _forceGravity;
    private float _initalForceGravity;
    private bool _takingDamage;

    ObjectPool<Enemy> _objectPool;
    public float counter;

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
    }


    void Update()
    {

       _inAir = IsGrounded() ? true : false;

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

        TakeDamage(dmg, target);

        if (_inAir)
            _rigidbody.AddForce(-transform.forward * 7, ForceMode.VelocityChange);
        else
        { 
            CancelarTodasLasFuerzas();
            StartCoroutine(ReturnGravity());
        }

        StartCoroutine(ViewDamage());
    }

    IEnumerator ViewDamage()
    {
        yield return new WaitForSeconds(0.5f);
        _takingDamage = false;
    }

    public void GetUpDamage(float dmg, Vector3 target, float forceToUp)
    {
        TakeDamage(dmg, target);

        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, forceToUp);
    }

    private void TakeDamage(float damage, Vector3 viewTarget)
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
        yield return new WaitForSeconds(0.5f);
        _forceGravity = _initalForceGravity;
    }
}
