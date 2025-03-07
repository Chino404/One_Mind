using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRails : MonoBehaviour
{
    public static CameraRails Instance;

    [Header("Components")]
    public CharacterTarget myCharacterTarget;
    private Transform _target;
    public Transform Target { get { return _target; } set => _target = value; }
    [SerializeField] private Rail _myRail;

    [Space(6), Header("Smoothing Values")]
    public float moveSpeed = 5f;
    [SerializeField] private bool _smoothMove = true;

    [Range(0.01f, 10f)][SerializeField] float _smoothSpeedPosition = 5f;
    private Vector3 _lastPosition;

    [Range(0.01f, 10f)][SerializeField] float _smoothSpeedRotation = 0.075f;

    [Header("Camera Offset")]
    [Tooltip("Ajusta la posici�n de la c�mara detr�s del personaje.")] public Vector3 cameraOffset = new Vector3(0f, 0f, -9f);

    [Space(5)]
    [SerializeField, Range(0, -50f)] private float minXRotation = -20f; // L�mite inferior del eje X
    [SerializeField, Range(0, 50f)] private float maxXRotation = 35f;  // L�mite superior del eje X
    [SerializeField, Range(0, -50f)] private float minYRotation = -10f; // L�mite izquierdo del eje Y
    [SerializeField, Range(0, 50f)] private float maxYRotation = 10f;  // L�mite derecho del eje Y

    #region Puede servir
    //[Tooltip("Distancia detr�s del personaje"),SerializeField] private float _distanceBehind = 3f;
    //[Tooltip("Altura adicional"),SerializeField] private float _heightOffset = 1f;
    #endregion

    private void Awake()
    {
        Instance = this;

        if (myCharacterTarget == CharacterTarget.Bongo)
        {
            if (GameManager.instance.bongoRails) Destroy(gameObject);
            else GameManager.instance.bongoRails = this;

            if (!_target) _target = GameManager.instance.modelBongo.transform;
        }
        else if (myCharacterTarget == CharacterTarget.Frank)
        {
            if (GameManager.instance.frankRails) Destroy(gameObject);
            else GameManager.instance.frankRails = this;

            if (!_target) _target = GameManager.instance.modelFrank.transform;
        }

        if (_target == null)
        {
            Debug.LogError($"Falta target en: {gameObject.name}");
            return;
        }

        _myRail.RailTarget = _target;
        _lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        #region Puede servir
        //// Obtener la posici�n proyectada en el riel
        //Vector3 projectedPosition = _myRail.ProjectPositionOnRail(_target.position);

        //// Agregar un desplazamiento detr�s del personaje
        //Vector3 offset = -_target.forward * _distanceBehind + Vector3.up * _heightOffset;
        //Vector3 targetPosition = projectedPosition + offset;
        #endregion

        // Calcular la posici�n deseada detr�s del personaje, con el desplazamiento.
        Vector3 targetPosition = _myRail.ProjectPositionOnRail(_target.position) + cameraOffset;

        // Movimiento suave
        if (_smoothMove)
        {
            _lastPosition = Vector3.Lerp(_lastPosition, targetPosition, moveSpeed * Time.deltaTime);
            transform.position = _lastPosition;
        }
        else
        {
            transform.position = targetPosition;
        }

        #region Puede Servir
        // Suavizar la rotaci�n para que la c�mara mire en la direcci�n del personaje
        //Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _smoothSpeedRotation * Time.deltaTime * 60);
        #endregion

        // Ajustar la rotaci�n de la c�mara
        AdjustCameraRotation();
    }

    private void AdjustCameraRotation()
    {
        // Direcci�n hacia el personaje
        Vector3 direction = _target.position - transform.position;

        // Obtener la rotaci�n necesaria para mirar al personaje
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Convertir la rotaci�n a �ngulos de Euler
        Vector3 eulerRotation = targetRotation.eulerAngles;

        // Ajustar el eje X para mantenerlo dentro de los l�mites
        eulerRotation.x = (eulerRotation.x > 180) ? eulerRotation.x - 360 : eulerRotation.x; // Corrige valores de Euler mayores a 180�
        eulerRotation.x = Mathf.Clamp(eulerRotation.x, minXRotation, maxXRotation);

        // Ajustar el eje Y para mantenerlo dentro de los l�mites
        eulerRotation.y = (eulerRotation.y > 180) ? eulerRotation.y - 360 : eulerRotation.y; // Corrige valores de Euler mayores a 180�
        eulerRotation.y = Mathf.Clamp(eulerRotation.y, minYRotation, maxYRotation);

        // Aplicar la rotaci�n con suavizado
        Quaternion finalRotation = Quaternion.Euler(eulerRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, _smoothSpeedRotation * Time.deltaTime);
    }
}

