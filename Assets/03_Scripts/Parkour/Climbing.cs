using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    public ModelMonkey _monkey;
    [SerializeField]float _speedInClimbing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            _monkey.GetComponent<Rigidbody>().isKinematic = true;
            
            Debug.Log("me agarre");
            //_monkey.isRestricted = true;
            //float horizontal = Input.GetAxisRaw("Horizontal");
            
            //_monkey.CancelarTodasLasFuerzas();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _monkey.Jump();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer==11)
         _monkey.GetComponent<Rigidbody>().isKinematic = false;

    }
}
