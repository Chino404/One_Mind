using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour, IInteracteable
{
    [SerializeField] private Mechanism[] mechanism;

    [Space(10), Header("-> Feedback")]
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _deactiveColor;

    [Space(5), SerializeField] private GameObject _lamp;
    [SerializeField] private Light _feedBackLight;
    [SerializeField, Range(0.1f, 1f) ,Tooltip("Tiempo apágado del parpadeo")] private float _flashingTime;

    private void Start()
    {
        if (!_feedBackLight) Debug.LogWarning($"Falta una luz en: {gameObject.name}");
        else
        {
            _feedBackLight.color = _deactiveColor;

            StartCoroutine(FlashingLight());
        }


        if (!_lamp) Debug.LogWarning($"Falta una lampara en: {gameObject.name}");
        else _lamp.GetComponent<MeshRenderer>().material.color = _deactiveColor;
    }

    public void Active()
    {
        for (int i = 0; i < mechanism.Length; i++)
        {
            StopAllCoroutines();

            mechanism[i].ActiveMechanism();

            if(_feedBackLight) _feedBackLight.color = _activeColor;
            if(_lamp) _lamp.GetComponent<MeshRenderer>().material.color = _activeColor;
        }
    }

    public void Deactive()
    {

    }

    IEnumerator FlashingLight()
    {
        while (true)
        {
            _feedBackLight.gameObject.SetActive(false);

            yield return new WaitForSeconds(_flashingTime);

            _feedBackLight.gameObject.SetActive(true);

            yield return new WaitForSeconds(_flashingTime * 2);

        }
    }

}
