using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Timeline.TimelineAsset;

public class TransparencyMaterial : MonoBehaviour
{
    private Material _material;

    private bool _action;

    private Color _iniColor;
    private Color _endColor;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;

        _iniColor = _material.color;
        _endColor.a = 0;
    }

    /// <summary>
    /// Desvanecer el material del objeto
    /// </summary>
    public void Fade(float t)
    {
        if(!_action)
        {
            _action = true;
            StartCoroutine(InterpolateFade(t));
        }
    }

    IEnumerator InterpolateFade(float durationFade)
    {
        float elapsedTime = 0;

        while (elapsedTime < durationFade)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationFade;

            // Interpolar el valor alfa
            float alpha = Mathf.Lerp(_iniColor.a, _endColor.a, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            _material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, alpha);

            yield return null;
        }

        // Asegúrate de que el color final sea el correcto
        _material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, _endColor.a);
    }


    /// <summary>
    /// Para que aparezca el material
    /// </summary>
    /// <param name="t"></param>
    public void Appear(float t)
    {
        if(_action)
        {
            _action = false;
            StartCoroutine(InterpolateVisible(t));
        }
    }

    IEnumerator InterpolateVisible(float durationFade)
    {
        float elapsedTime = 0;

        while (elapsedTime < durationFade)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationFade;

            // Interpolar el valor alfa
            float alpha = Mathf.Lerp(_endColor.a, _iniColor.a, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            _material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, alpha);

            yield return null;
        }

        // Asegúrate de que el color final sea el correcto
        _material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, _iniColor.a);
    }
}
