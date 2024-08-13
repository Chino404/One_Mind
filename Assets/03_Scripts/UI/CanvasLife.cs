using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLife : MonoBehaviour
{
    [SerializeField] GameObject _canvasVida;
    private void Update()
    {
        if(!GameManager.Instance.ContollerMonkey)
        {
            _canvasVida.SetActive(false);
        }
        else _canvasVida.SetActive(true);
    }
}
