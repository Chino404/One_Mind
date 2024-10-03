using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyLayers : MonoBehaviour, IInteracteable
{
    [SerializeField, Tooltip("Capa en la que no se renderiza los objetos para la cámara | Poner solo una Layer")] private LayerMask _doNotRenderer;
    private int _valueMaskNotRenderer;

    [Space(10), SerializeField, Tooltip("Objetos que se van a ACTIVAR para la cámara")] private GameObject[] _activeLayers;
    private Queue<int> _listSaveActiveLayers;
    [SerializeField, Tooltip("Volver a desactivar las layer que se activaron cuando salga del Collider")] private bool _backToDeactive;

    [Space(10), SerializeField, Tooltip("Objetos que se van a DESACTIVAR para la cámara")] private GameObject[] _desactiveLayers;
    private Queue<int> _listSaveDesactiveLayers;

    private Collider _myCollider;

    private void Awake()
    {
        if (gameObject.GetComponent<Collider>()) _myCollider = GetComponent<Collider>();
        else Debug.LogError($"Falta Collider en: {gameObject.name}");

        _listSaveActiveLayers = new Queue<int>();
        _listSaveDesactiveLayers = new Queue<int>();

        _valueMaskNotRenderer = (int)Mathf.Log(_doNotRenderer.value, 2);

        for (int i = 0; i < _activeLayers.Length; i++)
        {
            _listSaveActiveLayers.Enqueue(_activeLayers[i].gameObject.layer);

            _activeLayers[i].layer = _valueMaskNotRenderer;
        }

        for (int i = 0; i < _desactiveLayers.Length; i++)
        {
            _listSaveDesactiveLayers.Enqueue(_desactiveLayers[i].gameObject.layer);
        }

        //foreach (var item in _listSaveActiveLayers)
        //{
        //    Debug.Log($"En {gameObject.name}: {item}");
        //}

    }

    public void Active()
    {                                                                                                                                                        
        for (int i = 0; i < _activeLayers.Length; i++)
        {
            _activeLayers[i].layer = _listSaveActiveLayers.Dequeue();
        }

        for (int i = 0; i < _desactiveLayers.Length; i++)
        {
            _desactiveLayers[i].layer = _valueMaskNotRenderer;
        }

        if(!_backToDeactive) _myCollider.enabled = false;
    }

    public void Deactive()
    {
        if(_backToDeactive)
        {
            for (int i = 0; i < _activeLayers.Length; i++)
            {
                _listSaveActiveLayers.Enqueue(_activeLayers[i].layer);

                _activeLayers[i].layer = _valueMaskNotRenderer;
            }
        }
    }

}
