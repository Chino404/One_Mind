using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Transform _waypoint;

    private Vector3 _velocity;
    [SerializeField] private float _speed;

    private bool _hasEaten;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_hasEaten)
            MoveTowardsWaypoint(_waypoint.position);
    }

    private void OnTriggerEnter(Collider other)
    {

        StartCoroutine(Eat());
    }

    IEnumerator Eat()
    {
        _animator.SetTrigger("Eat");
        yield return new WaitForSeconds(1f);
        _hasEaten = true; ;


    }

    void MoveTowardsWaypoint(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) >= 0.05f)
        {
            _velocity = target - transform.position;
            _velocity.Normalize();

            transform.position += _velocity * _speed * Time.deltaTime;
        }
    }
}
