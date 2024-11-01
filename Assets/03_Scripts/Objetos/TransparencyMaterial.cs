using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TransparencyMaterial : MonoBehaviour, ITransparency
{
    //private MeshRenderer _meshRendererMaterial;
    private Renderer _meshRendererMaterial;

    private bool _isActiveTrasnparency;
    [SerializeField] private bool _isThisShader;
    private int _idOpacity = Shader.PropertyToID("_Opacity");

    private Color _iniColor;
    [Space(10), SerializeField, Tooltip("Agregar los objetos en donde se van a activar junto a este la transparencia")] private TransparencyMaterial[] _gruopObjetTransparency;
    [SerializeField, Range(0,1f)] private float _valueAlpha = 0.1f;

    private void Start()
    {
        //_meshRendererMaterial = GetComponent<MeshRenderer>();
        _meshRendererMaterial = GetComponent<Renderer>();

        if(!_isThisShader) _iniColor = _meshRendererMaterial.material.color;

    }

    /// <summary>
    /// Desvanecer el material del objeto
    /// </summary>
    public void Fade(float t)
    {
        if(!_isActiveTrasnparency)
        {

            _isActiveTrasnparency = true;

            if(_isThisShader) StartCoroutine(InterpolateFadeShader(t));
            else StartCoroutine(InterpolateFadeMaterial(t));

            for (int i = 0; i < _gruopObjetTransparency.Length; i++)
            {
                if (_gruopObjetTransparency[i])
                {
                    _gruopObjetTransparency[i].Fade(t); 
                }
            }
        }
    }

    IEnumerator InterpolateFadeMaterial(float durationFade)
    {
        float elapsedTime = 0;

        while (elapsedTime < durationFade)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationFade;

            // Interpolar el valor alfa
            float alpha = Mathf.Lerp(_iniColor.a, _valueAlpha, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            _meshRendererMaterial.material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, alpha);
           

            yield return null;
        }

        // Asegúrate de que el color final sea el correcto
        _meshRendererMaterial.material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, _valueAlpha);
    }

    IEnumerator InterpolateFadeShader(float durationFade)
    {
        float elapsedTime = 0;

        while (elapsedTime < durationFade)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationFade;

            // Interpolar el valor alfa
            float alpha = Mathf.Lerp(2, _valueAlpha, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            //_meshRendererMaterial.material.SetFloat(_idOpacity, alpha);

            for (int i = 0; i < _meshRendererMaterial.materials.Length; i++)
            {
                _meshRendererMaterial.materials[i].SetFloat(_idOpacity, alpha);
            }


            yield return null;
        }

        // Asegúrate de que el color final sea el correcto

        _meshRendererMaterial.material.SetFloat(_idOpacity, _valueAlpha);
    }


    /// <summary>
    /// Para que aparezca el material
    /// </summary>
    /// <param name="t"></param>
    public void Appear(float t)
    {
        if(_isActiveTrasnparency)
        {
            _isActiveTrasnparency = false;

            if (_isThisShader) StartCoroutine(InterpolateVisibleShader(t));
            else StartCoroutine(InterpolateVisiblematerial(t));

            for (int i = 0; i < _gruopObjetTransparency.Length; i++)
            {
                if (_gruopObjetTransparency[i]) _gruopObjetTransparency[i].Appear(t);
            }
        }
    }

    IEnumerator InterpolateVisiblematerial(float durationFade)
    {
        float elapsedTime = 0;

        while (elapsedTime < durationFade)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationFade;

            // Interpolar el valor alfa
            float alpha = Mathf.Lerp(_valueAlpha, _iniColor.a, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            _meshRendererMaterial.material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, alpha);

            yield return null;
        }

        // Asegúrate de que el color final sea el correcto
        _meshRendererMaterial.material.color = new Color(_iniColor.r, _iniColor.g, _iniColor.b, _iniColor.a);
    }

    IEnumerator InterpolateVisibleShader(float durationFade)
    {
        float elapsedTime = 0;

        while (elapsedTime < durationFade)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationFade;

            // Interpolar el valor alfa
            float alpha = Mathf.Lerp(_valueAlpha, 2, t);

            // Crear un nuevo color con el alfa interpolado, manteniendo los valores RGB originales
            _meshRendererMaterial.material.SetFloat(_idOpacity, alpha);

            //for (int i = 0; i < _meshRendererMaterial.materials.Length; i++)
            //{
            //    _meshRendererMaterial.materials[i].SetFloat(_idOpacity, alpha);
            //}

            yield return null;
        }

        // Asegúrate de que el color final sea el correcto
        _meshRendererMaterial.material.SetFloat(_idOpacity, 2);
    }
}
