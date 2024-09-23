using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlataform : Rewind
{
    [SerializeField, Range(0.1f, 1)] private float _timeToDisolve = 0.25f;
    private Renderer _disolveMaterial;
    private int _IdDisolve = Shader.PropertyToID("_Disolve");

    private void Start()
    {
        _disolveMaterial = GetComponent<Renderer>();
    }

    public override void Save()
    {
        _currentState.Rec(gameObject.activeInHierarchy);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();

        gameObject.SetActive((bool)col.parameters[0]);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            StartCoroutine(ShaderDisolve());
        }
    }

    IEnumerator ShaderDisolve()
    {
        var timer = 0f;
        var valueDisolve = 0f;

        while (timer < _timeToDisolve)
        {
            timer += Time.deltaTime;
            float t = timer / _timeToDisolve;
            valueDisolve = Mathf.Lerp(1, 0, t);

            _disolveMaterial.material.SetFloat(_IdDisolve, valueDisolve);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
