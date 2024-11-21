using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    Factory<Bullet> _factory;
    ObjectPool<Bullet> _objectPool;
    private Animator _myAnimator;

    public Bullet prefab;
    [Tooltip("Segundos de cooldown")][SerializeField] float _coolDown; 
    [Tooltip("cantidad de balas que instancio al principio")][SerializeField] int _bulletQuantity;

    [Tooltip("Cooldown para llamar a la corrutina")]private bool _isInCoolDown;

    [Header("BURST")]
    [Tooltip("si dispara en rafaga")]public bool isBurst;
    [Tooltip("no es exactamente cuantos segundos dispara, asi que ir probando")]public float timeShooting;
    private float _secondsShooting;
    [SerializeField] private float _burstCoolDown;

    private void Awake()
    {
        _myAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _factory = new BulletFactory(prefab);
        
        _objectPool = new ObjectPool<Bullet>(_factory.GetObj, Bullet.TurnOff, Bullet.TurnOn, _bulletQuantity);
        _secondsShooting = timeShooting;
    }

    private void Update()
    {     
        if (!_isInCoolDown)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        _isInCoolDown = true;

        _myAnimator.SetBool("Attack", true);

        var bullet = _objectPool.Get();
        bullet.AddReference(_objectPool);
        bullet.transform.position = transform.position;
        bullet.transform.forward = transform.forward;

        yield return new WaitForSeconds(_coolDown);

        if (isBurst)
        {
            _secondsShooting--;

            if (_secondsShooting <= 0) //Si los segundos es menor a 0
            {
                _myAnimator.SetBool("Attack", false);

                yield return new WaitForSeconds(_burstCoolDown); //Espero unos segundos

                _secondsShooting = timeShooting;
            }

        }

        _isInCoolDown = false;

    }


}
