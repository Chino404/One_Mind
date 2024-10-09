using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyLayers : MonoBehaviour, IInteracteable
{
    [SerializeField, Tooltip("Desactivar las layers de _activeLayers ni bien arranca el juego")] private bool _deactiveInAwake = true;
    [SerializeField, Tooltip("Capa en la que no se renderiza los objetos para la cámara | Poner solo una Layer")] private LayerMask _doNotRenderer;
    private int _valueMaskNotRenderer;
    [SerializeField, Tooltip("Desactivar el collider cuando se haya ejecutado el códigco")] private bool _deactiveMyColider = true;


    [Space(10), SerializeField, Tooltip("Objetos que se van a ACTIVAR para la cámara")] private GameObject[] _activeLayers;
    //private Queue<int> _listSaveActiveLayers;
    [SerializeField]private List<int> _listSaveActiveLayers = new List<int>();

    [SerializeField, Tooltip("Volver a DESACTIVAR las layer que se activaron cuando salga del Collider")] private bool _backToDeactive;

    [Space(10), SerializeField, Tooltip("Objetos que se van a DESACTIVAR para la cámara")] private GameObject[] _desactiveLayers;
    //private Queue<int> _listSaveDesactiveLayers;
    private List<int> _listSaveDesactiveLayers = new List<int>();

    [SerializeField, Tooltip("Volver a ACTIVAR las layer que se desactivaron cuando salga del Collider")] private bool _backToActive;

    private Collider _myCollider;

    private void Awake()
    {
        if (gameObject.GetComponent<Collider>()) _myCollider = GetComponent<Collider>();
        else Debug.LogError($"Falta Collider en: {gameObject.name}");

        //_listSaveActiveLayers = new Queue<int>();
        //_listSaveDesactiveLayers = new Queue<int>();

        _valueMaskNotRenderer = (int)Mathf.Log(_doNotRenderer.value, 2);

        for (int i = 0; i < _activeLayers.Length; i++)
        {
            //Me guardo la capa del objeto
            //_listSaveActiveLayers.Enqueue(_activeLayers[i].gameObject.layer);

            if (!_activeLayers[i]) return;

            _listSaveActiveLayers.Add(_activeLayers[i].layer);

            if (_listSaveActiveLayers[i] == 9)
            {
                Debug.LogWarning("Una capa no se guardo bien, se cambiara por la LAYER 8");
                _listSaveActiveLayers[i] = 8;
            }

            if (_deactiveInAwake)
            {
                _activeLayers[i].layer = _valueMaskNotRenderer; //Desactivar las layers

                //ChangeLayersInNotRenderer(_activeLayers);
            }
        }

        for (int i = 0; i < _desactiveLayers.Length; i++)
        {
            //_listSaveDesactiveLayers.Enqueue(_desactiveLayers[i].gameObject.layer);
            if (_desactiveLayers[i])_listSaveDesactiveLayers.Add(_desactiveLayers[i].layer);
        }

    }

    public void Active()
    {                                                                                                                                                        
        for (int i = 0; i < _activeLayers.Length; i++)
        {
            //_activeLayers[i].layer = _listSaveActiveLayers.Dequeue();
            if (_activeLayers[i]) _activeLayers[i].layer = _listSaveActiveLayers[i];
        }

        for (int i = 0; i < _desactiveLayers.Length; i++)
        {
            if (_desactiveLayers[i]) _desactiveLayers[i].layer = _valueMaskNotRenderer;
        }

        if (_deactiveMyColider) _myCollider.enabled = false;
    }

    public void Deactive()
    {
        if(_backToDeactive)
        {
            //Las desactivos
            for (int i = 0; i < _activeLayers.Length; i++)
            {
                //_listSaveActiveLayers.Enqueue(_activeLayers[i].layer);
                if (_activeLayers[i])
                {
                    _listSaveActiveLayers.Add(_activeLayers[i].layer);

                    _activeLayers[i].layer = _valueMaskNotRenderer;
                }
            }
        }

        if(_backToActive)
        {
            //Las vuelvo a activar
            for (int i = 0; i < _desactiveLayers.Length; i++)
            {
                //_desactiveLayers[i].layer = _listSaveDesactiveLayers.Dequeue();
                if (_desactiveLayers[i])_desactiveLayers[i].layer = _listSaveDesactiveLayers[i];
            }

            //Me las vuelvo a guardar
            for (int i = 0; i < _desactiveLayers.Length; i++)
            {
                //_listSaveDesactiveLayers.Enqueue(_desactiveLayers[i].gameObject.layer);
                if (_desactiveLayers[i])_listSaveDesactiveLayers.Add(_desactiveLayers[i].layer);
            }
        }
    }

    /// <summary>
    /// Cambia las Layer de los Obj del array por la layer que no renderiza la cámara
    /// </summary>
    /// <param name="obj"></param>
    void ChangeLayersInNotRenderer(GameObject[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            if (obj[i]) obj[i].layer = _valueMaskNotRenderer; //Cambio a la layer q no renderiza la cámara
        }
    }

}
