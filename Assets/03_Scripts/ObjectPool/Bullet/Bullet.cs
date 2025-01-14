using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [Tooltip("Contador de tiempo para instanciar una bala")] private float _counter;
    //public float damage;

    private ObjectPool<Bullet> _objectPool;


    void Update()
    {
        transform.position += transform.forward* _speed * Time.deltaTime;
        
        _counter += Time.deltaTime;
        
        if(_counter >= 2)
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
            damageable.Dead();
        }

        else
        {
            gameObject.SetActive(false);
        }
    }
}
