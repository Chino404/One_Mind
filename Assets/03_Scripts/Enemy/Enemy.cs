using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    [Header("Values")]
    [SerializeField] private float _life = 100;
    [SerializeField] private float _dmg;
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
    public bool _inAir;

    [Header("Flocking")]
    public float maxVelocity;
    private float _iniVelocity;
    public float maxForce;
    public ModelMonkey target;

    [Header("Radius")]
    public float separationRadius;
    public float viewRadius;
    public float attackDistance; //maxDistance;
    
    Vector3 _velocity;
    public Vector3 Velocity { get { return _velocity; } }

    [Header("Hit")]
    public float damage;
    [HideInInspector] FSM fsm;
    [SerializeField] float _cooldownHit;
    [HideInInspector]public bool isHitting;

    [Header("Animation")]
    public Animator anim;
    //[SerializeField] string _speedAnim = "Speed";
    [SerializeField] string _damageAnim = "DamageReceived";

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        anim.GetComponentInChildren<Animator>();
        _currentState = new MementoState();

    }

    private void Start()
    {
        _initalForceGravity = _forceGravity;
        _timerCounterInveencible = _timeInvencible;
        //AddForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f))*maxVelocity);
        GameManager.instance.enemies.Add(this);

        _iniVelocity = maxVelocity;

        fsm = new FSM();

        

        fsm.CreateState("Idle", new Idle(this, fsm));
        fsm.CreateState("Attack", new Attack(this, fsm));
        fsm.CreateState("Follow Player", new FollowPlayer(this, fsm));
        fsm.ChangeState("Idle");

        target = GameManager.instance.players[0].GetComponent<ModelMonkey>();


    }



    void Update()
    {
        //Flocking();

        //if (_velocity != Vector3.zero)
        //{
        ////    transform.position += _velocity * Time.deltaTime;
        //    transform.forward = _velocity;
        //}

        Vector3 seekDir = Seek(target.transform.position);
        Vector3 flockingDir = Separation(GameManager.instance.enemies, separationRadius);
        Vector3 flockingAlignment = Alignment(GameManager.instance.enemies, separationRadius);

        _velocity += flockingDir*GameManager.instance.weightSeparation + 
            seekDir*GameManager.instance.weightSeek+
            flockingAlignment*GameManager.instance.weightAlignment;
        

        //if (!_inAir)
        fsm.Execute();

        if (_life <= 0)
        {
            GameManager.instance.enemies.Remove(this);

            gameObject.SetActive(false);

        }

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

    //private void OnTriggerEnter(Collider other)
    //{

    //    var damageable = other.gameObject.GetComponent<IDamageable>();
    //    if (damageable != null)
    //    {
    //        damageable.TakeDamageEntity(_dmg, transform.position);
    //    }
    //}

    #region Damage
    public void TakeDamageEntity(float dmg, Vector3 target)
    {
        
        SetDamageAnimation();
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
    #endregion

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

    #region Flocking
    //public void Flocking()
    //{
    //    AddForce(Separation(GameManager.instance.enemies, separationRadius));
    //    //AddForce(Alignment(GameManager.instance.enemies, viewRadius) * GameManager.instance.weightAlignment);
    //}

    Vector3 Separation(List<Enemy> enemies, float radius)
    {
        Vector3 desired = Vector3.zero;

        if(enemies.Count > 1)
        {
            foreach (var item in enemies)
            {
                var dir = item.transform.position - transform.position;
                if (dir.magnitude > radius || item == this)
                    continue;//pasa al siguiente con el foreach

                desired -= dir;

            }
        }

        if (desired == Vector3.zero)
            return desired;

        desired.Normalize();
        desired *= maxVelocity;

        return CalculateSteering(desired);
    }

    Vector3 Alignment(List<Enemy> enemies, float radius)
    {
        var desired = Vector3.zero;
        int count = 0;
        foreach (var item in enemies)
        {
            if (item == this)
                continue;
            if (Vector3.Distance(transform.position, item.transform.position) <= radius)
            {
                desired += item._velocity;
                count++;
            }
        }
        if (count <= 0)
            return desired;

        desired /= count;
        desired.Normalize();
        desired *= maxVelocity;

        return CalculateSteering(desired);
    }
    #endregion

    public Vector3 Seek(Vector3 target)
    {
            var desired = target - transform.position;
        //if (!_inAir)
        
            
            desired.Normalize();
            desired *= maxVelocity;

            desired.y = 0;



        return CalculateSteering(desired);

    }

    Vector3 CalculateSteering(Vector3 desired)
    {
        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        return steering;
    }

    public void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, maxVelocity);
    }

    public void Hit()
    {
        if (isHitting) return;
        isHitting = true;
        StartCoroutine(HitCoolDown());     
    }

    public IEnumerator HitCoolDown()
    {
        anim.SetTrigger("AttackTrigger");

        // Obtener la información del estado actual de la animación
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // Duración de la animación en segundos
        float animationDuration = stateInfo.length;
        Debug.Log(animationDuration);

        maxVelocity = 0;

        yield return new WaitForSeconds(2);

        Debug.Log("Cambio de bool");
        maxVelocity = _iniVelocity;
        isHitting = false;

    }

    public void SetDamageAnimation()
    {
        Debug.Log("animacion");
        anim.SetTrigger(_damageAnim);
    }
    

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(transform.position, maxDistance);

        //Vector3 lineA = GetVectorFromAngle(viewAngle * 0.5f + transform.eulerAngles.y);
        //Vector3 lineB = GetVectorFromAngle(-viewAngle * 0.5f + transform.eulerAngles.y);

        //Gizmos.DrawLine(transform.position, transform.position + lineA * viewRadius);
        //Gizmos.DrawLine(transform.position, transform.position + lineB * viewRadius);

        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(transform.position, arriveRadius);
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }


    #endregion


    private void OnDestroy()
    {
       GameManager.instance.enemies.Remove(this);
    }

    public override void Save()
    {
        _currentState.Rec(transform.position, transform.rotation, gameObject.activeInHierarchy, _life);
        Debug.Log("guarde el sapo");
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        transform.position = (Vector3)col.parameters[0];
        transform.rotation = (Quaternion)col.parameters[1];
        gameObject.SetActive((bool)col.parameters[2]);
        _life = (float)col.parameters[3];

        Debug.Log("cargue sapo");
    }
}

