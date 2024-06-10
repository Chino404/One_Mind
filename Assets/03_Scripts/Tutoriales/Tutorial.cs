using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ModelMonkey>())
            _canvas.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<ModelMonkey>())
            _canvas.gameObject.SetActive(false);
    }
}
