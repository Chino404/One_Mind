using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRails : MonoBehaviour
{
    public static CameraRails Instance;
    [SerializeField] private Rail _myRail;

    public bool isTeleporting;
    private bool _isFixedCamera;

    [Header("Components")]
    public CharacterTarget myCharacterTarget;
    private Transform _target;
    public Transform Target { get { return _target; } set => _target = value; }

    [SerializeField, Tooltip("Nodo fijo que setea la pos. y la rot. de la c�mara")] public Transform fixedNode { get; private set; }

    [Space(6), Header("Smoothing Values")]
    public float moveSpeed = 5f;

    [Space(10), SerializeField] private bool _smoothMove = true;
    [Range(0.01f, 10f)][SerializeField] float _smoothSpeedPosition = 5f;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;

    [Range(0.01f, 10f)][SerializeField] float _smoothSpeedRotation = 0.075f;

    Vector3 _desiredPos, _smoothPos;
    Quaternion _smoothRot;



    [Header("Camera Offset")]
    [Tooltip("Ajusta la posici�n de la c�mara detr�s del personaje.")] public Vector3 cameraOffset = new Vector3(0f, 0f, -9f);

    [Space(5)]
    [SerializeField, Range(0, -50f)] private float minXRotation = -20f; // L�mite inferior del eje X
    [SerializeField, Range(0, 50f)] private float maxXRotation = 35f;  // L�mite superior del eje X
    [SerializeField, Range(0, -20f)] private float minYRotation = -10f; // L�mite izquierdo del eje Y
    [SerializeField, Range(0, 20f)] private float maxYRotation = 10f;  // L�mite derecho del eje Y


    private void Awake()
    {
        Instance = this;

        if (myCharacterTarget == CharacterTarget.Bongo)
        {
            if (GameManager.instance.bongoRailsCamera) Destroy(gameObject);
            else GameManager.instance.bongoRailsCamera = this;

            if (!_target) _target = GameManager.instance.modelBongo.transform;
        }
        else if (myCharacterTarget == CharacterTarget.Frank)
        {
            if (GameManager.instance.frankRailsCamera) Destroy(gameObject);
            else GameManager.instance.frankRailsCamera = this;

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

        if(_isFixedCamera)
        {
            SetPositionAndRotationTarget();
            return;
        }

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

    private void SetPositionAndRotationTarget()
    {
        if (isTeleporting)
        {
            transform.SetPositionAndRotation(fixedNode.position, fixedNode.rotation);
        }
        else
        {
            // Calcular la posici�n deseada relativa al punto
            _desiredPos = _target.position + (fixedNode.position - _target.position);

            // Suavizado en base a Time.deltaTime para asegurar suavidad continua
            _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeedPosition * Time.deltaTime * 60); //Multiplicando por 60 asegura que las velocidades del Lerp sean proporcionales y consistentes
            _smoothRot = Quaternion.Lerp(transform.rotation, fixedNode.rotation, _smoothSpeedRotation * Time.deltaTime * 60);

            transform.SetPositionAndRotation(_smoothPos, _smoothRot);
        }

    }

    public void TransitionToAFixedNode(Transform newNode)
    {
        //if (_isFixedCamera) return;

        if(!_isFixedCamera) _isFixedCamera = true;

        _lastPosition = transform.position;
        _lastRotation = transform.rotation;

        fixedNode = newNode;
    }

    public void TransitionToRail()
    {
        if (!_isFixedCamera) return;

        _isFixedCamera = false;

        transform.position = _lastPosition;
        transform.rotation = _lastRotation;
    }
}

