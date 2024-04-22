using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [Header("Swinging")]
    [SerializeField]private float _maxSwiningDistance = 25f;
    private Vector3 _swingPoint;
    [SerializeField]private SpringJoint _joint; //Articulacion
    [SerializeField] private Transform _hookPoint;


    [Header("Cooldown")]


    //[SerializeField] private float _dist;
    private LineRenderer _lr;

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        //_hJ = GetComponent<HingeJoint>();
        //_hJ = GetComponentInChildren<HingeJoint>();
    }

    private void Start()
    {
        EventManager.Subscribe("Hook", StartSwing);
        EventManager.Subscribe("StopHook", StopSwing);
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Llamar cada vez que se quiera iniciar el gancho
    /// </summary>
    public void StartSwing(params object[] parameters)
    {
        _lr.enabled = true;
        var player = (Transform)parameters[0];

        Vector3 pos = _hookPoint.position;
        Vector3 dir = (Vector3)parameters[1];
        Debug.DrawLine(pos, dir);

        _swingPoint = dir;

        if(!player.gameObject.GetComponent<SpringJoint>())
            _joint = player.gameObject.AddComponent<SpringJoint>();

        _joint.autoConfigureConnectedAnchor = false; //Debería Unity calcular la posición del punto de anclaje conectado automáticamente?
        _joint.connectedAnchor = _swingPoint; //El punto en el espacio local del objeto el cual la articulación está adjunta.

        float distanceFormPoint = Vector3.Distance( player.position, _swingPoint);

        ////La distancia del "agarre" intentara mantenerse alejada del "punto de agarre"
        //_joint.maxDistance = distanceFormPoint * 0.8f; //Limite superior del rango de distancia en la cual el resorte no aplica cualquier fuerza.
        //_joint.minDistance = distanceFormPoint * 0.25f; //Límite inferior del rango de distancia en la que el resorte no aplicará alguna fuerza.

        _joint.maxDistance = 0f; //Limite superior del rango de distancia en la cual el resorte no aplica cualquier fuerza.
        _joint.minDistance = 0f; //Límite inferior del rango de distancia en la que el resorte no aplicará alguna fuerza.

        //Customizar variables a gusto
        _joint.spring = 10f; //Fuerza del resorte.
        _joint.damper = 1000f; //Cantidad a la cual el resorte es reducido cuando está activa.
        _joint.massScale = 4.5F;

        _lr.positionCount = 2;
        _currentGrapplePosition = _hookPoint.position;
        

    }

    public void ExecuteSwing()
    {

    }

    /// <summary>
    /// Llamar cuando se quiera cortar el gancho
    /// </summary>
    public void StopSwing(params object[] parameters)
    {
        //_lr.enabled = false;
        _lr.positionCount = 0;
        Destroy(_joint);
    }

    private Vector3 _currentGrapplePosition;

    private void DrawRope()
    {
        if (!_joint) return;

        _currentGrapplePosition = Vector3.Lerp(_currentGrapplePosition, _swingPoint, Time.deltaTime * 8F);

        _lr.SetPosition(0, _hookPoint.position); //Origen
        _lr.SetPosition(1, _swingPoint); //Destino
    }
}
