using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class FixativeObjectUI : MonoBehaviour
{
    //RectTransform es un componente en Unity que se utiliza para definir el tamaño, la posición y la orientación de elementos dentro de la interfaz de usuario (UI),
    //especialmente en el sistema de Canvas. Es una extensión de Transform y está específicamente diseñado para trabajar con interfaces y elementos en 2D.

    private Camera _refCamera;  // La cámara principal que renderiza la escena
    private ModelBongo _refModelBongo;
    private Vector3 _screenPosition;

    public RectTransform canvasRectTransform;  // El RectTransform del canvas
    public RectTransform targetLockImage;  // El RectTransform de la imagen en el canvas que indica el target lock

    [Space(10),SerializeField]private List<Transform> _listTargetObject;  // El objeto en la escena al que queremos hacer target lock
    [SerializeField] private int _currentIndexList; //Se muestra para chequear
    private Vector3 _direction;

    private float _scroll; //Temporal
    private bool _isTargetDetected;

    private Collider _myBoxCollider;

    private void Awake()
    {
        _myBoxCollider = GetComponent<BoxCollider>();
        _refModelBongo = GetComponent<ModelBongo>();
    }

    private void Start()
    {
        _refCamera = GameManager.instance.bongoCamera.GetComponent<Camera>();
        _currentIndexList = 0;
    }

    void Update()
    {
        // Obtiene el valor de desplazamiento de la rueda del ratón
        //_scroll = Input.GetAxisRaw("Mouse ScrollWheel");

        //if(_scroll > 0f)
        //{
        //    _currentIndexList--;

        //    if (_currentIndexList < 0) _currentIndexList = _listTargetObject.Count - 1;
        //}

        //else if (_scroll < 0f)
        //{
        //    _currentIndexList++;

        //    if (_currentIndexList >= _listTargetObject.Count) _currentIndexList = 0;
        //}

        //if (_listTargetObject.Count > 0) TargetLockA();
        //else _refModelBongo.DirSpells = Vector3.zero;

    }

    private bool ObstructionRay(Vector3 obj)
    {
        _direction = obj - transform.position;

        if (Physics.Raycast(transform.position, _direction, out RaycastHit hit, _direction.magnitude))
        {
            if (hit.transform.gameObject.layer != 10) _isTargetDetected = true;

            else _isTargetDetected = false;
        }

        return _isTargetDetected;
    }

    public void TargetLockA(float scroll)
    {
        if(_listTargetObject.Count <= 0)
        {
            _refModelBongo.DirSpells = Vector3.zero;
            return;
        }


        if (scroll > 0f)
        {
            var aux = _currentIndexList; //Variable auxiliar
            _currentIndexList--;

            if (_currentIndexList < 0) _currentIndexList = _listTargetObject.Count - 1;

            if (ObstructionRay(_listTargetObject[_currentIndexList].position)) _currentIndexList = aux;
        }

        else if (scroll < 0f)
        {
            var aux = _currentIndexList; //Variable auxiliar
            _currentIndexList++;

            if (_currentIndexList >= _listTargetObject.Count) _currentIndexList = 0;

            if (ObstructionRay(_listTargetObject[_currentIndexList].position)) _currentIndexList = aux;
        }

        //if (ObstructionRay(_listTargetObject[_currentIndexList].position)) LogicaDelMasCecano();

        // Verifica que el objeto esté dentro de la vista de la cámara
        _screenPosition = _refCamera.WorldToScreenPoint(_listTargetObject[_currentIndexList].position);

        _direction = _listTargetObject[_currentIndexList].position - transform.position;

        Debug.DrawLine(transform.position, _listTargetObject[_currentIndexList].position, Color.cyan);

        if (_screenPosition.z > 0)  // Asegura que el objeto está en frente de la cámara
        {
            // Convierte la posición de pantalla a coordenadas locales del canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, _screenPosition, _refCamera, out Vector2 localPosition);

            // Asigna la posición de la imagen de target lock en el canvas
            targetLockImage.localPosition = localPosition;

            // Asegura que la imagen esté visible
            targetLockImage.gameObject.SetActive(true);

            _refModelBongo.DirSpells = _direction;
        }
        else
        {
            // Si el objeto está detrás de la cámara, oculta la imagen
            Debug.Log("Es menor a 0");
            _refModelBongo.DirSpells = Vector3.zero;
            targetLockImage.gameObject.SetActive(false);
        }

    }

    private void LogicaDelMasCecano()
    {
        var masCercano = 10000f;
        var newIndex = 0;

        for (int i = 0; i < _listTargetObject.Count; i++)
        {
            var newDistance = Vector3.Distance(transform.position, _listTargetObject[i].position);

            if (newDistance < masCercano)
            {
                masCercano = newDistance;
                newIndex = i;
            }
        }

        _currentIndexList = newIndex;
    }

    private void RemoveObjectToList(Transform obj)
    {
        _listTargetObject.Remove(obj);

        if (_currentIndexList > 0) _currentIndexList--;
        else targetLockImage.gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        var objInteract = other.gameObject;

        if (objInteract.layer == 10)
        {
            if(!_listTargetObject.Contains(objInteract.transform)) _listTargetObject.Add(objInteract.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            RemoveObjectToList(other.gameObject.transform);
        }
    }

}
