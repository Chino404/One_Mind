using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class BananaGuide : Rewind
{
    public Transform target;
    [SerializeField]private int _actualIndex;
    public WayPoints[] wayPoints;
    private Rigidbody _rb;
    [Tooltip("Velocidad")]public float maxSpeed = 10f;
    private float _iniSpeed;
    [Tooltip("Fuerza para girar")]public float maxForce = 6f;

    [Header("Radios")]
    [Tooltip("Radio para emepzar a frenar")]public float arriveRadius = 1.5f;
    [Tooltip("Radio de visualizacion")]public float viewRadius = 1f;

    [Header("Obstacle Acoidance / Esquivar Obstaculos")]
    [Tooltip("Capas de obstaculos")]public LayerMask obstacleLayer;
    [Tooltip("Fueza para esquivar")]public float avoidWeight; //El peso con el que esquiva las cosas, q tanto se va a mover 
    private Vector3 _velocity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _currentState = new MementoState();

        _iniSpeed = maxSpeed;

    }

    void Update()
    {

        AddForce(Seek(wayPoints[_actualIndex].transform.position));

        if (wayPoints[_actualIndex].Stop)
        {
            StopBanana();
            return;
        }

        if (Vector3.Distance(transform.position, wayPoints[_actualIndex].transform.position) <= 0.3f)
        {

            var target = wayPoints[_actualIndex].transform.position;

            _actualIndex++;
        }


        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity; //Que mire para donde se esta moviendo

        //if (transform.position != target.position)
        //{
        //    AddForce(Arrive(target.position));

        //    //AddForce(ObstacleAvoidance() * avoidWeight);

        //    //transform.position += _velocity * Time.deltaTime;
        //    //transform.forward = _velocity; //Que mire para donde se esta moviendo
        //}

        // Solo rotamos sobre el eje Y (vertical), manteniendo la posición en X y Z
        //Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        //transform.LookAt(lookPos);
    }

    private void FixedUpdate()
    {
        if (_velocity != Vector3.zero)
        {
            _rb.MovePosition(transform.position + _velocity * Time.fixedDeltaTime);
        }
    }

    void StopBanana()
    {
        if(Vector3.Distance(transform.position, wayPoints[_actualIndex].transform.position) <= 0.3f)
        {
            maxSpeed = 0;

            // Solo rotamos sobre el eje Y (vertical), manteniendo la posición en X y Z
            Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(lookPos);
        }

        if (Vector3.Distance(transform.position, target.position) <= viewRadius && !wayPoints[_actualIndex].action)
        {
            Debug.Log("Cambio Index");
            maxSpeed = _iniSpeed;
            _actualIndex++;
        }
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

        if (dist > arriveRadius)
            return Seek(target);

        var desired = target - transform.position;
        desired.Normalize();
        desired *= maxSpeed * ((dist - viewRadius) / arriveRadius); //Si la dist la divido por el radio, me va achicando la velocidad

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
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, arriveRadius);
    }
#endregion


    public override void Save()
    {
        _currentState.Rec(transform.position);
        //Debug.Log("guardo banana");
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        transform.position = (Vector3)col.parameters[0];
        
        //Debug.Log("cargo banana");
    }
}
