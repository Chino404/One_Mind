using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualPressurePlate : Rewind, IInteracteable
{
    [Header("PARAMETERS")]
    [SerializeField] private CharacterTarget _player;
    [Space(10), SerializeField,Tooltip("Colocar la otra placa de presion en la cual va a estar vinculada")] private DualPressurePlate _otherDualPressurePlate;
    [SerializeField, Tooltip("Puerta al que se le va a ejecutar una acción")] private DualDoor _objectToInteract;
    [Space(10), SerializeField, Tooltip("Objetos que sirven para indicar que esta placa de presion fue activada")] private ActiveTorch[] _indicators;

    private bool _activePressurePlate;
    public bool ActivePressurePlate { get { return _activePressurePlate; } }

    private bool _actionCompleted = false;
    public bool ActionCompleted { set { _actionCompleted = value; } }

    [SerializeField] private Material[] _materials;


    [Space(5), SerializeField] GameObject _view;

    [Header("-> PARTICLE")]
    [SerializeField] private ParticleSystem[] _particleButton;
    
    private Renderer _renderer;
    //private Animator _animator;

    [SerializeField] private GameObject _button;
    [SerializeField] private float _endAnimation;
    [SerializeField] private float _speed;
    


    public override void Awake()
    {
        base.Awake();
        if (_otherDualPressurePlate == null) Debug.LogWarning($"Falta referencia de la otra placa de presión en: {gameObject.name}");
        if (_objectToInteract == null) Debug.LogWarning($"Falta objeto para interactuar en: {gameObject.name}");

        //_animator = GetComponent<Animator>();

        for (int i = 0; i < _indicators.Length; i++)
        {
            if (_indicators[i] == null)
            {
                Debug.LogWarning($"FALTAN INDICADORES EN: {gameObject.name}");
                continue;
            } 
        }

    }

    void Start()
    {     
        _renderer = _view.GetComponent<Renderer>();
        _renderer.enabled = true;
        _renderer.sharedMaterial = _materials[0];
    }

    void Update()
    {
        
            
        if (_activePressurePlate)
        {
            if (_button.transform.localPosition.y > 0)
            {
                _button.transform.localPosition -= new Vector3(0, 1, 0);
                //Debug.Log("aprete el boton");
            }
        }

        if (!_activePressurePlate)
        {

            if (_button.transform.localPosition.y < _endAnimation)
                _button.transform.localPosition += new Vector3(0, 1, 0);
        }
    }

    public void Active()
    {
        if (_actionCompleted) return;

        _activePressurePlate = true;

        //_animator?.SetTrigger("Pressed");

        if(_player == CharacterTarget.Bongo)
        {
            if (_materials[1] == null) Debug.LogWarning($"Falta un material de Bongo en: {gameObject.name}");
            else _renderer.sharedMaterial = _materials[1];
        }
        else
        {
            if (_materials[2] == null) Debug.LogWarning($"Falta un material de Frank en: {gameObject.name}");
            else _renderer.sharedMaterial = _materials[2];
        }

        for (int i = 0; i < _indicators.Length; i++)
        {      
            if (_indicators[i] != null) _indicators[i].Active();       
        }

        if (!_actionCompleted) AudioManager.instance.Play(SoundId.ButtonDualDoor);

        if (_otherDualPressurePlate != null && _otherDualPressurePlate.ActivePressurePlate)
        {
            _otherDualPressurePlate.ActionDualPressurePlate();

            ActionDualPressurePlate();
        }
    }

    public void ActionDualPressurePlate()
    {

        if (_objectToInteract != null)
        {
            if(_player == CharacterTarget.Bongo) _particleButton[0].Play();
            else _particleButton[1].Play();

            _objectToInteract.OpenTheDoor();

            _actionCompleted = true;
        }

    }

    public void Deactive()
    {
        if (_actionCompleted) return;

        _activePressurePlate = false;
        
        //_animator?.SetTrigger("Normal");
        _renderer.sharedMaterial = _materials[0];
        

        for (int i = 0; i < _indicators.Length; i++)
        {
            //if (_indicators[i] != null)_indicators[i].gameObject.SetActive(false);
            if (_indicators[i] != null)_indicators[i].Deactive();
        }

    }

    public override void Save()
    {
        _currentState.Rec(_actionCompleted, _button.transform.localPosition);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        _actionCompleted = (bool)col.parameters[0];
        _button.transform.localPosition = (Vector3)col.parameters[1];


    }
}
