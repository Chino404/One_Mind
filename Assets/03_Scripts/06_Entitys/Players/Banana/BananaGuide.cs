using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EstadoDeBananaBotNPC
{
    EnPosicion,
    RegresandoAPosicion,
    AtaqueCargado
}

public class BananaGuide : Rewind
{
    public static BananaGuide Instance;

    [Header("Variables")]
    public EstadoDeBananaBotNPC actualStateBananaNPC;
    public Transform target;
    private Collider _myCollider;
    private Rigidbody _rb;
    [Tooltip("Velocidad")]public float maxSpeed = 10f;
    [Tooltip("Fuerza para girar")]public float maxForce = 6f;

    [Header("Radios")]
    [SerializeField, Tooltip("Radio para empezar a frenar")] private float _arriveRadius = 1f;
    [SerializeField, Tooltip("Radio de visualizacion")] private float _viewRadius = 0.3f;
    [SerializeField, Tooltip("Radio para alejarse del Player")] private float _rangoRadius;
    public float RangoRadius { get { return _rangoRadius; } }

    [Header("Ataque Cargado")]
    [SerializeField, Range(1, 4f), Tooltip("Tiempo para la carga máxima")] private float _maxTime = 2f;
    [SerializeField, Range(0,1f), Tooltip("Tiempo para llegar al lugar de la Explosion")] private float _timeToArrive = 0.5f;
    [SerializeField, Range(0,4f), Tooltip("Tiempo quieto en el lugar")] private float _quietTime = 2f;
    [SerializeField, Tooltip("Collider de la zona del ataque cargado")] private GameObject _ondaExpansiva;
    [SerializeField] private ParticleSystem _electricity;
    private bool _activeParticle = false;
    private float _chargedTime;

    [Header("Obstacle Acoidance / Esquivar Obstaculos")]
    [SerializeField, Tooltip("Capas de obstaculos")] private LayerMask _obstacleLayer;
    [SerializeField,Tooltip("Fueza para esquivar")] private float _avoidWeight; //El peso con el que esquiva las cosas, q tanto se va a mover 
    private Vector3 _velocity;
    private Vector3 _dir;


    public override void Awake()
    {
        Instance = this;

        _rb = GetComponent<Rigidbody>();
        _myCollider = GetComponent<Collider>();
        _currentState = new MementoState();

        //_zoneChargedAttack.enabled = false;
        //_zoneChargedAttack.radius = 2.5f;

        _ondaExpansiva.gameObject.SetActive(false);
        _ondaExpansiva.transform.localScale = new Vector3(3f,3f,3f);

        _electricity?.Stop();

        EventManager.Subscribe("LaunchChargedAttack", LaunchChargedAttack);
        EventManager.Subscribe("ChargeAttack", ChargeAttack);

    }

    private void OnEnable()
    {
        _myCollider.isTrigger = true;
        
    }

    private void OnDisable()
    {
        _myCollider.isTrigger = false;
    }

    private void FixedUpdate()
    {
        if (actualStateBananaNPC == EstadoDeBananaBotNPC.AtaqueCargado) return;

        AddForce(Arrive(target.position));
        Rotate(target);

        //AddForce(ObstacleAvoidance() * avoidWeight);

        if (_velocity != Vector3.zero)
        {
            _rb.MovePosition(transform.position + _velocity * Time.fixedDeltaTime);
        }
        
    }

    private void Rotate(Transform dirForward)
    {
        if (actualStateBananaNPC == EstadoDeBananaBotNPC.EnPosicion) transform.forward = dirForward.forward;
        else if (actualStateBananaNPC == EstadoDeBananaBotNPC.RegresandoAPosicion) transform.LookAt(dirForward);
    }

    #region Ataque Cargado
    public void ChargeAttack(params object[] parameters)
    {
        if(_chargedTime < _maxTime)
        {
            _chargedTime += Time.deltaTime;

            if (!_activeParticle)
            {
                _activeParticle = true;
                _electricity?.Play();
            }

            if (_chargedTime >= _maxTime)
            {
                //_zoneChargedAttack.radius = 7;
                _ondaExpansiva.transform.localScale = new Vector3(7f, 7f, 7f);
            }
            else if (_chargedTime >= _maxTime / 2)
            {
                //_zoneChargedAttack.radius = 4;
                _ondaExpansiva.transform.localScale = new Vector3(5f, 5f, 5f);
            }
        }
    }

    public void LaunchChargedAttack(params object[] parameters)
    {
        //if (actualStateBananaNPC != EstadoDeBananaBotNPC.EnPosicion) return;
        _dir = (Vector3)parameters[0];
        _dir *= 20f;

        _electricity?.Stop();
        _activeParticle = false;
        StartCoroutine(Destiny());
        _chargedTime = 0;
    }

    IEnumerator Destiny()
    {
        //_launch = true;
        actualStateBananaNPC = EstadoDeBananaBotNPC.AtaqueCargado;

        float elapsedTime = 0;
        var positionA = transform.position;

        //Tiempo para llegar al destino
        while (elapsedTime < _timeToArrive)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _timeToArrive;

            transform.position = Vector3.Lerp(positionA, positionA + _dir, t);
            yield return null;
        }

        elapsedTime = 0;
        //_zoneChargedAttack.enabled = true;
        _ondaExpansiva.gameObject.SetActive(true);

        while (elapsedTime < _quietTime)
        {
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        actualStateBananaNPC = EstadoDeBananaBotNPC.RegresandoAPosicion;

        //_zoneChargedAttack.radius = 4;
        //_zoneChargedAttack.enabled = false;

        _ondaExpansiva.transform.localScale = new Vector3(3f, 3f, 3f);
        _ondaExpansiva.gameObject.SetActive(false);
    }




    #endregion

    #region Patrones de Movimiento
    public Vector3 Seek(Vector3 target)
    {
        var desired = target - transform.position;
        desired.Normalize();
        desired *= maxSpeed;

        return CalculateSteering(desired);
    }

    public Vector3 Arrive(Vector3 target)
    {
        var dist = Vector3.Distance(transform.position, target);

        if (dist > _arriveRadius)
            return Seek(target);

        var desired = target - transform.position;
        desired.Normalize();
        desired *= maxSpeed * ((dist - _viewRadius) / _arriveRadius); //Si la dist la divido por el radio, me va achicando la velocidad

        actualStateBananaNPC = EstadoDeBananaBotNPC.EnPosicion;

        return CalculateSteering(desired);
    }


    /// <summary>
    /// Calculo la fuerza con la que va a girar su direccion
    /// </summary>
    /// <param name="desired"></param>
    /// <returns></returns>
    Vector3 CalculateSteering(Vector3 desired)
    {
        var steering = desired - _velocity; //direccion = la dir. deseada - hacia donde me estoy moviendo
        steering = Vector3.ClampMagnitude(steering, maxForce / 10);

        return steering;
    }

    public Vector3 ObstacleAvoidance()
    {
        Vector3 pos = transform.position;
        Vector3 dir = transform.forward;
        float dist = _velocity.magnitude; //Que tan rapido estoy yendo

        Debug.DrawLine(pos, pos + (dir * dist));

        if (Physics.SphereCast(pos, 1, dir, out RaycastHit hit, dist, _obstacleLayer))
        {
            var obstacle = hit.transform; //Obtengo el transform del obstaculo q acaba de tocar
            Vector3 dirToObject = obstacle.position - transform.position; //La direccion del obstaculo

            float anguloEntre = Vector3.SignedAngle(transform.forward, dirToObject, Vector3.up); //(Dir. hacia donde voy, Dir. objeto, Dir. mis costados)

            Vector3 desired = anguloEntre >= 0 ? -transform.right : transform.right; //Me meuvo para derecha o izquierda dependiendo donde esta el obstaculo
            desired.Normalize();
            desired *= maxSpeed;

            return CalculateSteering(desired);
        }

        return Vector3.zero;
    }

    public void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _arriveRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _rangoRadius);
    }
#endregion


    public override void Save()
    {
        _currentState.Rec(transform.position, maxSpeed);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        transform.position = (Vector3)col.parameters[0];
        //_actualIndex = (int)col.parameters[1];
        maxSpeed = (float)col.parameters[1];
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("LaunchChargedAttack", LaunchChargedAttack);
        EventManager.Unsubscribe("ChargeAttack", ChargeAttack);     
    }
}
