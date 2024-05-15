using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject[] paredes;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ModelBanana>())
        {
            foreach (var item in paredes)
            {
                item.gameObject.SetActive(false);
                Debug.Log("Desactivado");
            }
        }
    }
}
