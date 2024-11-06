using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixative : MonoBehaviour
{
    [SerializeField] private Camera _refCamera;
    [SerializeField] private GameObject _refGameObject;

    public RectTransform canvasRectTransform;  // El RectTransform del canvas
    public RectTransform imageRectTransform;  // El RectTransform de la imagen en el canvas

    private void Update()
    {
        //Vector3 dir = _refCamera.transform.position - transform.position;
        //dir.y = 0;

        //// Apunta el objeto hacia la dirección de la cámara
        //transform.rotation = Quaternion.LookRotation(dir);

        // Proyecta la posición del objeto en coordenadas de pantalla
        Vector3 screenPosition = _refCamera.WorldToScreenPoint(_refCamera.transform.position);

        // Convierte la posición de pantalla a coordenadas del canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, _refCamera, out Vector2 localPosition);

        // Actualiza la posición de la imagen en el canvas
        imageRectTransform.localPosition = localPosition;
    }
}
