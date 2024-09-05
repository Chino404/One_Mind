using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [HideInInspector]public Transform spawnPoint;
    
    private void Awake()
    {
        
        spawnPoint = transform.GetChild(0);
    }
    


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Characters>())
        {
            other.GetComponent<Characters>().actualCheckpoint = this;
            foreach (var item in GameManager.instance.rewinds)
            {
                item.Save();
            }
        }

    }
}
