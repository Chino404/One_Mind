using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private LineRenderer _lr;
    private Vector3 _grapperPoint;
    //public Transform stickTip;
    [SerializeField]private SpringJoint _joint; //Articulacion

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        EventManager.Subscribe("Hook", StartGrapple);
    }

    /// <summary>
    /// Llamar cada vez que se quiera iniciar el gancho
    /// </summary>
    public void StartGrapple(params object[] parameters)
    {
        var player = (Transform)parameters[0];

        _joint = player.gameObject.AddComponent<SpringJoint>();
        _joint.autoConfigureConnectedAnchor = true;
        _joint.connectedAnchor = _grapperPoint;

        float distanceFormPoint = Vector3.Distance( player.position, _grapperPoint);

        //La distancia del "agarre" intentara mantenerse alejada del "punto de agarre"
        _joint.maxDistance = distanceFormPoint * 0.8f;
        _joint.minDistance = distanceFormPoint * 0.25f;

        _joint.spring = 4.5f; //Resorte
        _joint.damper = 7f; //Amortiguacion
        _joint.massScale = 4.5F;
    }

    /// <summary>
    /// Llamar cuando se quiera cortar el gancho
    /// </summary>
    public void StopGrapple()
    {

    }
}
