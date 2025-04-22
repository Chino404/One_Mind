using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTorch : MonoBehaviour, IInteracteable
{
    [SerializeField, Range(0,2f),Tooltip("Tiempo que tarda en encenderse el fuego")] private float _timeToSpawn = 1f;
    private float _timeToDespawn;
    private float _timer = 0;

    [Space(10),SerializeField] private Light _light;
    [SerializeField]private Color _colorFire;
    public Color ColorFire { get { return _colorFire; } }
    private int _IdFireColor = Shader.PropertyToID("_FireColor");
    private float _dissolve; 
    
    [SerializeField, Range(0,3f)] private float _rangeLight = 1.8f;
    [Tooltip("Valor actual del rango de la luz")]private float _actualRangeLight;

    private int _IdFireThreshold = Shader.PropertyToID("_FireThreshold");
    [Space(10), SerializeField, Tooltip("Material del fuego")]private Renderer _fireMaterial;

    [Tooltip("valor del tamaño del fueho")] private float _valueFireTreshold = 0f;
    public ParticleSystem myParticleSystem;
    [SerializeField] private ParticleSystem _myParticleAmber;

    [Space(10)]
    public bool iceTorch;
    private bool _isActive;

    private void Awake()
    {
        myParticleSystem = GetComponentInChildren<ParticleSystem>();

        ChangeColorFire(_colorFire);

        if (_fireMaterial == null) Debug.LogWarning($"Falta el Renderer en: {gameObject.name}");

        if (_light == null) Debug.LogWarning($"Poner una LIGHT en: {gameObject.name}");
        else _light.range = 0;

        
    }

    void Start()
    {
        myParticleSystem?.Stop();
    }
    
    public void ChangeColorFire(Color color)
    {
        _colorFire = color;

        _fireMaterial.material.SetColor(_IdFireColor, color);
        _light.color = color;

        //if(_myParticleAmber) _myParticleAmber.colorOverLifetime.color = color;
    }

    public void Active()
    {
        _timer = 0;
        
        //if(!myParticleSystem.gameObject.activeInHierarchy) myParticleSystem.gameObject.SetActive(true);

        myParticleSystem?.Play();

        StartCoroutine(SpawnFire());
    }

    void Update()
    {
        if (iceTorch && _isActive)
        {
            _dissolve += Time.deltaTime;
            _dissolve = Mathf.Clamp01(_dissolve);
            _fireMaterial.material.SetFloat("_dissolve", _dissolve);
        }
    }

    IEnumerator SpawnFire()
    {
        if (iceTorch)
        {
            _fireMaterial.material.SetFloat("_freeze", 0);
        }

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

        _fireMaterial.material.SetFloat(_IdFireThreshold, 1);
        _light.range = _rangeLight;

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

        _fireMaterial.material.SetFloat(_IdFireThreshold, 0);
        _light.range = 0;

        if (_timer >= _timeToSpawn)
        {
            _light.range = 0;
            _valueFireTreshold = 0;
        }

        myParticleSystem?.Stop();

        _timer = 0;
        _timeToDespawn = 0;
    }
}
