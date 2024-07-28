using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaGuide : Rewind
{
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

    [Header("Obstacle Acoidance / Esquivar Obstaculos")]
    [Tooltip("Capas de obstaculos")]public LayerMask obstacleLayer;
    [Tooltip("Fueza para esquivar")]public float avoidWeight; //El peso con el que esquiva las cosas, q tanto se va a mover 
    private Vector3 _velocity;
    [SerializeField] private Vector3 _dir;

    private bool _launch;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _myCollider = GetComponent<Collider>();
        _currentState = new MementoState();

        EventManager.Subscribe("ChargedAttack", ChargedAttack);

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
        if (_launch) return;

        AddForce(Arrive(target.position));
        Rotate(target.forward);

        //AddForce(ObstacleAvoidance() * avoidWeight);

        if (_velocity != Vector3.zero)
        {
            _rb.MovePosition(transform.position + _velocity * Time.fixedDeltaTime);
        }
        
    }

    private void Rotate(Vector3 dirForward)
    {
        transform.forward = dirForward;

    }

    public void ChargedAttack(params object[] parameters)
    {
        _dir = (Vector3)parameters[0];
        _dir *= 20f;
        //_dir *= 30f;

        //StartCoroutine(Launch());
        StartCoroutine(Destiny());


        //_rb.velocity = new Vector3(transform.position.x, transform.position.y, transform.position.z * 20f);
        //transform.forward *= 20f; 

        //_rb.MovePosition(transform.position + (dir.normalized * 20f) * Time.fixedDeltaTime);
    }

    IEnumerator Destiny()
    {
        _launch = true;
        float elapsedTime = 0;
        var positionA = transform.position;

        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 0.5f;

            transform.position = Vector3.Lerp(positionA, positionA + _dir, t);
            yield return null;
        }
        _launch = false;
    }

    IEnumerator Launch()
    {
        _launch = true;
        yield return new WaitForSeconds(2);
        _launch = false;
    }

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

        if (Physics.SphereCast(pos, 1, dir, out RaycastHit hit, dist, obstacleLayer))
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
}
