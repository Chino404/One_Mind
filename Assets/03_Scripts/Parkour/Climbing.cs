using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public ModelMonkey playerModel;
    public Transform orientation;
    public Transform cam;
    private Rigidbody _rb;

    [Header("Edge Grabbing")]
    public float moveToEdgeSpeed;
    public float maxEdgeGrabDistance;
    public bool isHolding;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public LayerMask whatCanClimb;

    private Transform _lastEdge;
    private Transform _currentEdge;

    private RaycastHit _edgeHit;

    private void Update()
    {
        EdgeDetection();
        SubStateMachine();
    }

    void SubStateMachine()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        //bool anyInputPressed = horizontalInput != 0 || verticalInput != 0;

        //Agarrandose al borde
        if(isHolding)
        {
            FreezeRigidbodyOnEdge();

            if (Input.GetKeyDown(KeyCode.Space))
                ExitEdgeHold();
        }

    }

    void EdgeDetection()
    {
        bool edgeDetected = Physics.SphereCast(transform.position, sphereCastRadius, cam.forward,
            out _edgeHit, detectionLength, whatCanClimb);

        if (!edgeDetected) return;

        float distanceToEdge = Vector3.Distance(transform.position, _edgeHit.transform.position);

        if (_edgeHit.transform == _lastEdge) return;

        if (distanceToEdge < maxEdgeGrabDistance && !isHolding) EnterEdgeHold();
    }

    void EnterEdgeHold()
    {
        playerModel.isRestricted = true;

        isHolding = true;
        _currentEdge = _edgeHit.transform;
        _lastEdge = _edgeHit.transform;

        //_rb.useGravity = false;
        _rb.velocity = Vector3.zero;
    }

    void FreezeRigidbodyOnEdge()
    {
        //_rb.useGravity = false;

        Vector3 dirToEdge = _currentEdge.position - transform.position;
        float distanceToEdge = Vector3.Distance(transform.position, _currentEdge.position);

        if(distanceToEdge>1f)
        {
            if(_rb.velocity.magnitude<moveToEdgeSpeed)
            _rb.AddForce(dirToEdge.normalized * moveToEdgeSpeed * 1000f * Time.deltaTime);
        }

        else
        {
            if (_rb.velocity != Vector3.zero) _rb.velocity = Vector3.zero;
        }

        if (distanceToEdge > maxEdgeGrabDistance) ExitEdgeHold();
    }

    void ExitEdgeHold()
    {
        isHolding = false;

        playerModel.isRestricted = false;
        //_rb.useGravity = true;
    }
}
