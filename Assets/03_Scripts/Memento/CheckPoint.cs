using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ModelMonkey>())
        {
            foreach (var item in GameManager.instance.rewinds)
            {
                item.Save();
            }
        }

    }
}
