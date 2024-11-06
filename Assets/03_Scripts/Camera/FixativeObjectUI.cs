using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixativeObjectUI : MonoBehaviour
{
    //RectTransform es un componente en Unity que se utiliza para definir el tamaño, la posición y la orientación de elementos dentro de la interfaz de usuario (UI),
    //especialmente en el sistema de Canvas. Es una extensión de Transform y está específicamente diseñado para trabajar con interfaces y elementos en 2D.

    private Camera _refCamera;  // La cámara principal que renderiza la escena
    private Vector3 _screenPosition;
    public RectTransform canvasRectTransform;  // El RectTransform del canvas
    public RectTransform targetLockImage;  // El RectTransform de la imagen en el canvas que indica el target lock

    private Transform _targetObject;  // El objeto en la escena al que queremos hacer target lock
    private Collider _myBoxCollider;

    private void Awake()
    {
        _myBoxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _refCamera = GameManager.instance.bongoCamera.GetComponent<Camera>();
    }

    void Update()
    {
        

        
    }

    private void TargetLokc()
    {
        // Verifica que el objeto esté dentro de la vista de la cámara
        _screenPosition = _refCamera.WorldToScreenPoint(_targetObject.position);

        if (_screenPosition.z > 0)  // Asegura que el objeto está en frente de la cámara
        {
            // Convierte la posición de pantalla a coordenadas locales del canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, _screenPosition, _refCamera, out Vector2 localPosition);

            // Asigna la posición de la imagen de target lock en el canvas
            targetLockImage.localPosition = localPosition;

            // Asegura que la imagen esté visible
            targetLockImage.gameObject.SetActive(true);
        }
        else
        {
            // Si el objeto está detrás de la cámara, oculta la imagen
            targetLockImage.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
             _targetObject = other.gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
             _targetObject = null;
        }
    }

}
