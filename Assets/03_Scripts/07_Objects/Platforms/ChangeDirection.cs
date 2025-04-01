using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    MovePlataform _movePlataform;
    public Transform[] waypoints; 

    private void Awake()
    {
        _movePlataform = gameObject.GetComponent<MovePlataform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Characters>())
        {
            _movePlataform.waypoints = waypoints;
            //collision.transform.SetParent(transform);
            //collision.gameObject.GetComponent<Rigidbody>().MovePosition(collision.gameObject.GetComponent<Rigidbody>().position + _movePlataform.Velocity*1f );

        }
    }

    
}
