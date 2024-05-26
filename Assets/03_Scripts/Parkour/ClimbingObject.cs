using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingObject : MonoBehaviour
{
    ModelMonkey _monkey;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject==_monkey.gameObject)
        {
            _monkey.isRestricted = true;
            if (Input.GetKeyDown(KeyCode.Space))
                _monkey.isRestricted = false;
        }
    }
}
