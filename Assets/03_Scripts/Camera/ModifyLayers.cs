using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyLayers : MonoBehaviour, IInteracteable
{
    [SerializeField, Tooltip("Capa en la que no se renderiza los objetos para la cámara")] private LayerMask _doNotRenderer;

    [SerializeField, Tooltip("Objetos que se van a ACTIVAR para la cámara")] private GameObject[] _activeLayers;
    //private List<LayerMask> _listSaveActiveLayers;
    private Queue<int> _listSaveActiveLayers;

    [SerializeField, Tooltip("Objetos que se van a DESACTIVAR para la cámara")] private GameObject[] _desactiveLayers;
    //[SerializeField] private List<int> _listSaveDesactiveLayers;
    private Queue<int> _listSaveDesactiveLayers;

    private void Awake()
    {
        _listSaveActiveLayers = new Queue<int>();
        _listSaveDesactiveLayers = new Queue<int>();

        for (int i = 0; i < _activeLayers.Length; i++)
        {
            //_listSaveActiveLayers.Add(_activeLayers[i].layer);

            _listSaveActiveLayers.Enqueue(_activeLayers[i].layer);

            _activeLayers[i].layer = _doNotRenderer;
        }

        for (int i = 0; i < _desactiveLayers.Length; i++)
        {
            //_listSaveDesactiveLayers.Add(_desactiveLayers[i].gameObject.layer);
            _listSaveDesactiveLayers.Enqueue(_desactiveLayers[i].gameObject.layer);
        }

        //LayerMask mask = _doNotRenderer;
        //int layerIndex = Mathf.Log(mask.value, 2);

        //Debug.Log("El índice de la capa es: " + layerIndex);
    }

    public void Interact()
    {
        for (int i = 0; i < _activeLayers.Length; i++)
        {
            _activeLayers[i].layer = _listSaveActiveLayers.Peek();
        }

        for (int i = 0; i < _desactiveLayers.Length; i++)
        {
            _desactiveLayers[i].layer = _doNotRenderer.value;
            //_desactiveLayers[i].layer = Mathf.Log(_doNotRenderer.value, 2); // Cambiar a la capa que no renderiza
        }
    }

    public void Disconnect()
    {
        
    }

}
