using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : Characters
{
    public ModelMonkey monkey;
    [SerializeField] private float _climbingSpeed;
    GameObject _climbObject;//objeto que estoy escalando
    GameObject _climbedObject;//objeto que escale
    [Range(0, 1)]
    [SerializeField] private float _difPos;
    private bool _isGrabbed;
    [SerializeField]private float _jumpForce;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer==11)
        {
            monkey.GetComponent<Rigidbody>().isKinematic = true;
            monkey.isRestricted = true;
            _isGrabbed = true;
            _climbObject = collision.gameObject;
        }
    }

    IEnumerator Desactive()
    {
        _isGrabbed = false;
        _climbedObject = _climbObject;
        _climbedObject.SetActive(false);
        monkey.GetComponent<Rigidbody>().isKinematic = false;
        monkey.isRestricted = false;
        monkey.GetComponent<Rigidbody>().velocity = Vector3.up * _jumpForce;
        yield return new WaitForSeconds(0.5f);
        _climbedObject.SetActive(true);

    }

    private void Update()
    {
        if (_isGrabbed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Desactive());
            }
            else
            {
                transform.position = new Vector3(transform.position.x, _climbObject.transform.position.y - _difPos, transform.position.z);
                float horizontal = Input.GetAxisRaw("Horizontal");
                Vector3 movement = new Vector3(horizontal, 0f, 0f) * _climbingSpeed * Time.deltaTime;
                transform.Translate(movement);
            }
            
            
        }
    }
}
