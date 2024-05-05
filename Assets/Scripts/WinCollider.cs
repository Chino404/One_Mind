using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : MonoBehaviour
{
    [SerializeField] GameObject _winCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==3)
        {
            Time.timeScale = 0;
            _winCanvas.SetActive(true);
        }

    }
}
