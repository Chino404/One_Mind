using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyLayers : MonoBehaviour, IInteracteable
{
    [SerializeField, Tooltip("Capa en la que no se renderiza los objetos para la cámara | Poner solo una Layer")] private LayerMask _doNotRenderer;
    private int _valueMask;

    [SerializeField, Tooltip("Objetos que se van a ACTIVAR para la cámara")] private GameObject[] _activeLayers;
    private Queue<int> _listSaveActiveLayers;

    [SerializeField, Tooltip("Objetos que se van a DESACTIVAR para la cámara")] private GameObject[] _desactiveLayers;
    private Queue<int> _listSaveDesactiveLayers;

    private void Awake()
    {
        _listSaveActiveLayers = new Queue<int>();
        _listSaveDesactiveLayers = new Queue<int>();

        _valueMask = (int)Mathf.Log(_doNotRenderer.value, 2);

        for (int i = 0; i < _activeLayers.Length; i++)
        {
            _listSaveActiveLayers.Enqueue(_activeLayers[i].layer);

            _activeLayers[i].layer = _doNotRenderer;
        }

        for (int i = 0; i < _desactiveLayers.Length; i++)
        {
            _listSaveDesactiveLayers.Enqueue(_desactiveLayers[i].gameObject.layer);
        }

    }

    public void Active()
    {
        for (int i = 0; i < _activeLayers.Length; i++)
        {
            _activeLayers[i].layer = _listSaveActiveLayers.Peek();
        }

        for (int i = 0; i < _desactiveLayers.Length; i++)
        {
            _desactiveLayers[i].layer = _valueMask;
        }
    }

    public void Deactive()
    {
        
    }

}
