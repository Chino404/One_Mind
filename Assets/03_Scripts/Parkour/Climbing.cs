using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    public ModelMonkey monkey;
    [SerializeField] float _climbingSpeed;
    [SerializeField] GameObject _climbObject;
    bool _isGrabbed;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer==11)
        {
            monkey.GetComponent<Rigidbody>().isKinematic = true;
            monkey.isRestricted = true;
            _isGrabbed = true;
        }
    }

    IEnumerator Desactive()
    {
        _climbObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        _climbObject.SetActive(true);

    }

    private void Update()
    {
        if (_isGrabbed)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            Vector3 movement = new Vector3(horizontal, 0f, 0f) * _climbingSpeed * Time.deltaTime;
            transform.Translate(movement);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Desactive());
                monkey.GetComponent<Rigidbody>().isKinematic = false;
                monkey.isRestricted = false;
                _isGrabbed = false;

            }
        }
    }
}
