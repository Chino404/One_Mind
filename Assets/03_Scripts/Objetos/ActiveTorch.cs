using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTorch : MonoBehaviour, IInteracteable
{
    [SerializeField, Range(0,2f),Tooltip("Tiempo que tarda en encenderse el fuego")] private float _timeToSpawn = 1f;
    private float _timeToDespawn;
    private float _timer = 0;

    [Space(10),SerializeField] private Light _light;
    [SerializeField, Range(0,3f)] private float _rangeLight = 1.8f;
    private float _actualRangeLight;

    private int _IdFireThreshold = Shader.PropertyToID("_FireThreshold");
    [Space(10), SerializeField]private Renderer _fireMaterial;

    private float _valueFireTreshold = 0f;
    private ParticleSystem _myParticleSystem;

    private bool _isActive;

    private void Awake()
    {
        _myParticleSystem = GetComponentInChildren<ParticleSystem>();

        if (_fireMaterial == null) Debug.LogWarning($"Falta el Renderer en: {gameObject.name}");

        if (_light == null) Debug.LogWarning($"Poner una LIGHT en: {gameObject.name}");
        else _light.range = 0;
    }

    public void Active()
    {
        _timer = 0;
        _myParticleSystem?.Play();
        StartCoroutine(SpawnFire());
    }

    IEnumerator SpawnFire()
    {
        _isActive = true;

        var auxFire = _valueFireTreshold;
        var auxRange = _actualRangeLight;

        while (_timer < _timeToSpawn)
        {
            if (!_isActive) break;

            _timer += Time.deltaTime;

            _timeToDespawn = _timer; 
            float t = _timer / _timeToSpawn;

            _valueFireTreshold = Mathf.Lerp(auxFire, 1, t);
            _fireMaterial.material.SetFloat(_IdFireThreshold, _valueFireTreshold);

            _actualRangeLight = Mathf.Lerp(auxRange, _rangeLight,t);
            _light.range = _actualRangeLight;

            yield return null;
        }

        if (_timer >= _timeToSpawn)
        {
            _light.range = _rangeLight;
            _valueFireTreshold = 1;
        }

        _timer = 0;
    }

    public void Deactive()
    {
        _timer = 0;
        StartCoroutine(DespawnFire());
    }

    IEnumerator DespawnFire()
    {
        _isActive = false;

        var auxFire = _valueFireTreshold;
        var auxRange = _actualRangeLight;

        while (_timer < _timeToDespawn)
        {
            if (_isActive) break;

            _timer += Time.deltaTime;
            float t = _timer / _timeToDespawn;

            _valueFireTreshold = Mathf.Lerp(auxFire, 0, t);
            _fireMaterial.material.SetFloat(_IdFireThreshold, _valueFireTreshold);

            _actualRangeLight = Mathf.Lerp(auxRange, 0, t);
            _light.range = _actualRangeLight;

            yield return null;
        }

        if (_timer >= _timeToSpawn)
        {
            _light.range = 0;
            _valueFireTreshold = 0;
        }

        _myParticleSystem?.Stop();

        _timer = 0;
        _timeToDespawn = 0;
    }
}
