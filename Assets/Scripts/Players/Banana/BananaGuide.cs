using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaGuide : MonoBehaviour
{
    public Transform target;
    public float maxVelocity;
    public float maxForce;
    Vector3 _velocity;

    void Update()
    {

        if (transform.position != target.position)
            AddForce(Seek(target.position));

        
            transform.forward = _velocity;

        
    }

    public Vector3 Seek(Vector3 target)
    {
        var desired = target - transform.position;

        desired.Normalize();
        desired *= maxVelocity;

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
}
