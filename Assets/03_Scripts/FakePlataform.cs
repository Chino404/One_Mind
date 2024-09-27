using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlataform : Rewind
{
    [SerializeField, Range(0.1f, 1)] private float _timeToDisolve = 0.25f;
    [SerializeField] private float _timeToRespawn=1.5f;
    private float _valueDisolve = 1f;
    private Renderer _disolveMaterial;
    private int _IdDisolve = Shader.PropertyToID("_Disolve");

    

    private void Start()
    {
        _disolveMaterial = GetComponent<Renderer>();
        _valueDisolve = 1f;
    }

    public override void Save()
    {
        _currentState.Rec(gameObject.activeInHierarchy, _valueDisolve);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();

        gameObject.SetActive((bool)col.parameters[0]);
        _valueDisolve = (float)col.parameters[1];

    }

    private void Update()
    {
        _disolveMaterial.material.SetFloat(_IdDisolve, _valueDisolve);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>()&&_valueDisolve==1)
        {
            StartCoroutine(ShaderDisolve());
        }
    }

    IEnumerator ShaderDisolve()
    {
        var timer = 0f;
        //var valueDisolve = 0f;

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
        while (timer < _timeToDisolve)
        {
            timer += Time.deltaTime;
            float t = timer / _timeToDisolve;
            _valueDisolve = Mathf.Lerp(0, 1, t);
            _disolveMaterial.material.SetFloat(_IdDisolve, _valueDisolve);
            yield return null;
        }


        //gameObject.SetActive(false);
    }
}
