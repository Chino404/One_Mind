using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShader : MonoBehaviour
{
    [SerializeField] private Renderer _fireShader;
    [SerializeField, Tooltip("Nombre de la variable a modificar")] private string _shaderName;
    private int _idShader;

    [Space(10)]
    [SerializeField, Tooltip("Valor maximo para modificar el Shader")]private float _maxValueShader;
    [SerializeField, Tooltip("Valor maximo para modificar el Shader")]private float _minValueShader;
    [SerializeField] private float _currentValueShader;

    private void Awake()
    {
        _idShader = Shader.PropertyToID(_shaderName);
    }

    /// <summary>
    /// Interpolar el valor del shader.
    /// </summary>
    /// <param name="active"></param>
    /// <param name="interpolateValue"></param>
    public void InterpolatShaderValue(bool active,float interpolateValue)
    {
        var auxFire = _currentValueShader;

        if(active) _currentValueShader = Mathf.Lerp(auxFire, _maxValueShader, interpolateValue);
        else _currentValueShader = Mathf.Lerp(auxFire, _minValueShader, interpolateValue);

        _fireShader.material.SetFloat(_idShader, _currentValueShader);

        //if (active) _fireShader.material.SetFloat(_idShader, _maxValueShader);
        //else _fireShader.material.SetFloat(_idShader, _minValueShader);
    }

    /// <summary>
    /// Setear el valor del sahder. El valor que pases es el valor que se va a asignar de una.
    /// </summary>
    /// <param name="value"></param>
    public void SetValueShader(bool active)
    {
        if(active) _fireShader.material.SetFloat(_idShader, _maxValueShader);
        else _fireShader.material.SetFloat(_idShader, _minValueShader);
    }

}
