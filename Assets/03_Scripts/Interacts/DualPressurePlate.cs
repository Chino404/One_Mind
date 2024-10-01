using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualPressurePlate : MonoBehaviour, IInteracteable
{
    [Header("PARAMETERS")]
    [SerializeField] private CharacterTarget _player;
    [Space(10), SerializeField,Tooltip("Colocar la otra placa de presion en la cual va a estar vinculada")] private DualPressurePlate _otherDualPressurePlate;
    [SerializeField, Tooltip("Puerta al que se le va a ejecutar una acción")] private DualDoor _objectToInteract;
    [Space(10), SerializeField, Tooltip("Objetos que sirven para indicar que esta placa de presion fue activada")] private Light[] _indicators;

    private bool _active;
    public bool Active { get { return _active; } }

    private bool _actionCompleted = false;

    [SerializeField] private Material[] _materials;


    [Space(5), SerializeField] GameObject _view;

    [Header("-> PARTICLE")]
    [SerializeField] private ParticleSystem[] _particleButton;
    
    private Renderer _renderer;
    private Animator _animator;

    private void Awake()
    {
        if (_otherDualPressurePlate == null) Debug.LogWarning($"Falta referencia de la otra placa de presión en: {gameObject.name}");
        if (_objectToInteract == null) Debug.LogWarning($"Falta objeto para interactuar en: {gameObject.name}");

        _animator = GetComponent<Animator>();

        for (int i = 0; i < _indicators.Length; i++)
        {
            if (_indicators[i] == null)
            {
                Debug.LogWarning($"FALTAN INDICADORES EN: {gameObject.name}");
                break;
            } 
            _indicators[i].gameObject.SetActive(false);
        }

    }

    void Start()
    {     
        _renderer = _view.GetComponent<Renderer>();
        _renderer.enabled = true;
        _renderer.sharedMaterial = _materials[0];
    }

    public void Interact()
    {
        _active = true;

        _animator?.SetTrigger("Pressed");

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
            if (_indicators[i] != null) _indicators[i].gameObject.SetActive(true);       
        }

        if (!_actionCompleted) AudioManager.instance.Play(SoundId.ButtonDualDoor);

        if (_otherDualPressurePlate != null && _otherDualPressurePlate.Active)
        {
            _otherDualPressurePlate.ActionDualPressurePlate();

            ActionDualPressurePlate();
        }
    }

    public void ActionDualPressurePlate()
    {

        if (_objectToInteract != null && !_actionCompleted)
        {
            if(_player == CharacterTarget.Bongo) _particleButton[0].Play();
            else _particleButton[1].Play();

            _objectToInteract.OpenTheDoor();
            AudioManager.instance.Play(SoundId.Open_Door);

            _actionCompleted = true;
        }

    }

    public void Disconnect()
    {
        if (_actionCompleted) return;

        _active = false;
        
        _animator?.SetTrigger("Normal");
        _renderer.sharedMaterial = _materials[0];
        

        for (int i = 0; i < _indicators.Length; i++)
        {
            if (_indicators[i] != null)_indicators[i].gameObject.SetActive(false);
        }

    }
}
