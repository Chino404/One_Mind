using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [Tooltip("Contador de tiempo para apagarme y guardarme en el objectPool")] public float _counter;
    //public float damage;

    private ObjectPool<Bullet> _objectPool;
    public GameObject bullet;
    private SphereCollider _collider;
    public ParticleSystem particles;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        bullet.SetActive(true);
        _collider.enabled = true;
    }

    void Update()
    {
        transform.position += transform.forward* _speed * Time.deltaTime;
        
        _counter += Time.deltaTime;
        
        if(_counter >= 1)
        {
            _objectPool.StockAdd(this);      
        }
    }

    public void AddReference(ObjectPool<Bullet> op)
    {
        _objectPool = op;
    }

    public static void TurnOff(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    public static void TurnOn(Bullet bullet)
    {
        bullet._counter = 0;
        bullet.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.DeadByShoot();
        }

        else
        {
            StartCoroutine(DestroySnowBall());
        }
    }

    IEnumerator DestroySnowBall()
    {
        particles.Play();
        bullet.SetActive(false);
        _collider.enabled = false;
        yield return new WaitForSeconds(0.2f);
        _objectPool.StockAdd(this);
    }

}
