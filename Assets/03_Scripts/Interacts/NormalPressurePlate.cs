using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPressurePlate : Rewind, IInteracteable
{
    [Header("OBJECTS TO...")]
    [Space(5),SerializeField] private GameObject[] _active;
    [SerializeField] private GameObject[] _desactive;

    private Animator _animator;

    private bool _pressed;

    [Header("Prefab Settings")]
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPoint;

    public override void Awake()
    {
        _animator = GetComponent<Animator>();
        base.Awake();
    }

    private void Start()
    {
        for (int i = 0; i < _active.Length; i++)
        {
            if (_active[i].activeInHierarchy) //Si los objetos a activar estan activos en la jerarquia, los apago
            {
                _active[i].gameObject.SetActive(false);
            }
        }
    }

    public void Active()
    {
        if(!_pressed)
        {
            _pressed = true;
            _animator?.SetTrigger("Pressed");

            for (int i = 0; i < _active.Length; i++) //Activo los objetos
            {
                _active[i].gameObject.SetActive(true);

                _active[i].gameObject.GetComponent<WallHolograph>().Active();
            }

            for (int i = 0; i < _desactive.Length; i++) //Desactivo los objetos
            {
                //_deactive[i].gameObject.SetActive(false);

                _desactive[i].gameObject.GetComponent<WallHolograph>().Desactive();

            }

          

            if (_prefab != null && _spawnPoint != null)
            {
                Instantiate(_prefab, _spawnPoint.position, _spawnPoint.rotation); // Instancia el prefab
            }
            
        }
    }

    public void Deactive()
    {

    }

    public override void Save()
    {
        _currentState.Rec(_pressed);
            
         
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        _pressed = (bool)col.parameters[0];
    }
}
