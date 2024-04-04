using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera cameraA, cameraB;
    //[SerializeField] LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("entre");
            cameraA.gameObject.SetActive(false);
            cameraB.gameObject.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("sali");
            cameraA.gameObject.SetActive(true);
            cameraB.gameObject.SetActive(false);
        }

    }
}
