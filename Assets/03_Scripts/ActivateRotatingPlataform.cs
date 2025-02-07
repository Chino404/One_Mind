using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRotatingPlataform : MonoBehaviour
{
    public GameObject rotatingPlataform;

    private void Start()
    {
        rotatingPlataform.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Characters>())
            rotatingPlataform.SetActive(true);
    }
}
