using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NormalPressurePlate : Rewind, IInteracteable
{
    [Header("OBJECTS TO...")]
    [Space(5),SerializeField] private GameObject[] _active;
    [SerializeField] private GameObject[] _desactive;

    //private Animator _animator;

    private bool _pressed;
    [SerializeField, Tooltip("Se puede volver a presionar de nuevo")] private bool _isPressAgain;

    [Header("Prefab Settings")]
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private float _endAnimation;
    [SerializeField] private GameObject _button;

    public override void Awake()
    {
        //_animator = GetComponent<Animator>();
        base.Awake();
    }

    private void Start()
    {
        //for (int i = 0; i < _active.Length; i++)
        //{
        //    if (_active[i].activeInHierarchy) //Si los objetos a activar estan activos en la jerarquia, los apago
        //    {
        //        _active[i].gameObject.SetActive(false);
        //    }
        //}
    }

    private void Update()
    {
        if (_pressed)
        {
            if(_button.transform.localPosition.y>_endAnimation)
                _button.transform.localPosition -= new Vector3(0,  Time.deltaTime, 0);
        }
            
    }
    public void Active()
    {
        if(!_pressed)
        {
            if(!_isPressAgain)_pressed = true;
            //_animator?.SetTrigger("Pressed");

            for (int i = 0; i < _active.Length; i++) //Activo los objetos
            {
                //_active[i].gameObject.SetActive(true);

                if (!_active[i].GetComponent<DesactiveWall>()._isActing)
                    _active[i].gameObject.GetComponent<DesactiveWall>().Active();
                else return;

            }

            for (int i = 0; i < _desactive.Length; i++) //Desactivo los objetos
            {
                //_desactive[i].gameObject.SetActive(false);
                if (!_desactive[i].GetComponent<DesactiveWall>()._isActing)
                    _desactive[i].gameObject.GetComponent<DesactiveWall>().Desactive();
                else return;
                

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
        _currentState.Rec(_pressed,_button.transform.localPosition);
            
         
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        _pressed = (bool)col.parameters[0];
        _button.transform.localPosition = (Vector3)col.parameters[1];
        
        //if (_pressed == false)
        //    _animator?.SetTrigger("Normal");
    }
}
