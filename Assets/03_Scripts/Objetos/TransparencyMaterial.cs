using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyMaterial : MonoBehaviour, ITransparency
{
    private Material _material;

    private bool _action;

    private Color _iniColor;
    [SerializeField, Tooltip("Agregar los objetos en donde se van a activar junto a este la transparencia")] private TransparencyMaterial[] _gruopObjetTransparency;
    [SerializeField, Range(0,1f)] private float _valueAlpha = 0.1f;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;

        _iniColor = _material.color;
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

            for (int i = 0; i < _gruopObjetTransparency.Length; i++)
            {
                if (_gruopObjetTransparency[i])
                {
                    _gruopObjetTransparency[i].Fade(t); 
                }
            }
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
            float alpha = Mathf.Lerp(_iniColor.a, _valueAlpha, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            _material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, alpha);
           

            yield return null;
        }

        // Asegúrate de que el color final sea el correcto
        _material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, _valueAlpha);
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

            for (int i = 0; i < _gruopObjetTransparency.Length; i++)
            {
                if (_gruopObjetTransparency[i]) _gruopObjetTransparency[i].Appear(t);
            }
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
            float alpha = Mathf.Lerp(_valueAlpha, _iniColor.a, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            _material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, alpha);

            yield return null;
        }

        // Asegúrate de que el color final sea el correcto
        _material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, _iniColor.a);
    }
}
