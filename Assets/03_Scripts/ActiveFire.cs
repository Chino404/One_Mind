using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFire : MonoBehaviour
{
    public GameObject turnOnFire;
    public GameObject turnOffFire;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Characters>())
        {
            turnOffFire.SetActive(false);
            turnOnFire.SetActive(true);
        }
    }
}
