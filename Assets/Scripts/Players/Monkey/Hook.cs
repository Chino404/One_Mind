using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private LineRenderer _lr;
    private Vector3 _grapperPoint;
    [SerializeField] private float _dist;
    //public Transform stickTip;
    [SerializeField]private SpringJoint _joint; //Articulacion
    [SerializeField] private HingeJoint _hJ;

    private void Awake()
    {
        //_lr = GetComponent<LineRenderer>();
        _hJ = GetComponent<HingeJoint>();
    }

    private void Start()
    {
        EventManager.Subscribe("Hook", StartGrapple);
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Llamar cada vez que se quiera iniciar el gancho
    /// </summary>
    public void StartGrapple(params object[] parameters)
    {
        Vector3 pos = transform.position;
        Vector3 dir = (Vector3)parameters[1];


        //Debug.DrawLine(pos, pos + (dir * _dist));
        Debug.DrawLine(pos, dir);


        Debug.Log("En rango");
        var player = (Transform)parameters[0];

        //_joint = player.gameObject.AddComponent<SpringJoint>();
        //_joint.autoConfigureConnectedAnchor = true;
        //_joint.connectedAnchor = _grapperPoint;

        //float distanceFormPoint = Vector3.Distance( player.position, _grapperPoint);

        ////La distancia del "agarre" intentara mantenerse alejada del "punto de agarre"
        //_joint.maxDistance = distanceFormPoint * 0.8f;
        //_joint.minDistance = distanceFormPoint * 0.25f;

        //_joint.spring = 4.5f; //Resorte
        //_joint.damper = 7f; //Amortiguacion
        //_joint.massScale = 4.5F;

    }

    private void DrawRope()
    {
        //_lr.SetPosition(0, transform.position);
        //_lr.SetPosition(1, _grapperPoint);
    }

    /// <summary>
    /// Llamar cuando se quiera cortar el gancho
    /// </summary>
    public void StopGrapple()
    {

    }
}
