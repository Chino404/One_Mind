using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlataform : Rewind
{
    [SerializeField, Range(0.1f, 1), Tooltip("Tiempo de la transición del Disolve")] private float _timeToDisolve = 0.25f;
    [SerializeField, Tooltip("Tiempo que tarda en respawnear cuando se DESACTIVO")] private float _timeToRespawn = 1.5f;
    [SerializeField]private float _valueDisolve;
    private Renderer _disolveMaterial;
    private int _IdDisolve = Shader.PropertyToID("_Disolve");

    public override void Awake()
    {
        base.Awake();
        _disolveMaterial = GetComponent<Renderer>();
        _valueDisolve = 0f;     
    } 

    private void OnEnable()
    {
        StartCoroutine(ShaderDisolve(false));
    }

    

    private void Update()
    {
        _disolveMaterial.material.SetFloat(_IdDisolve, _valueDisolve);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>() && _valueDisolve==1)
        {
            StartCoroutine(ShaderDisolve(true));
        }
    }

    public IEnumerator ShaderDisolve(bool desactive)
    {
        var timer = 0f;

        if(desactive)
        {
            while (timer < _timeToDisolve)
            {
                timer += Time.deltaTime;
                float t = timer / _timeToDisolve;
                _valueDisolve = Mathf.Lerp(1, 0, t);

                _disolveMaterial.material.SetFloat(_IdDisolve, _valueDisolve);
            
                yield return null;
            }

            timer = 0f;

            yield return new WaitForSeconds(_timeToRespawn);
        }


        while (timer < _timeToDisolve)
        {
            timer += Time.deltaTime;
            float t = timer / _timeToDisolve;
            _valueDisolve = Mathf.Lerp(0, 1, t);
            _disolveMaterial.material.SetFloat(_IdDisolve, _valueDisolve);
            yield return null;
        }
    }

    public override void Save()
    {
        _currentState.Rec(gameObject.activeInHierarchy, _valueDisolve);
        Debug.Log($"{gameObject.name} guardo plataforma");

    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();

        gameObject.SetActive((bool)col.parameters[0]);
        _valueDisolve = (float)col.parameters[1];
        StopAllCoroutines();
        Debug.Log($"{gameObject.name} cargue plataforma");

    }
}
