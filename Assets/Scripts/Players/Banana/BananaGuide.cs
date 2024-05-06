using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaGuide : MonoBehaviour
{
    public Transform target;
    public float maxVelocity;
    public float maxForce;
    Vector3 _velocity;
    public float radiusArrive;

    void Update()
    {
        if (_velocity != Vector3.zero)
            transform.position += _velocity * Time.deltaTime;

        if (transform.position != target.position)
            AddForce(Arrive(target.position));

        transform.LookAt(target.forward);
    }

    public Vector3 Seek(Vector3 target)
    {
        var desired = target - transform.position;

        desired.Normalize();
        desired *= maxVelocity;

        return CalculateSteering(desired);
    }

    public Vector3 Arrive(Vector3 target)
    {
        var dist = Vector3.Distance(transform.position, target);

        if (dist > radiusArrive)
            return Seek(target);

        var desired = target - transform.position;
        desired.Normalize();
        desired *= maxVelocity * (dist / radiusArrive);



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
