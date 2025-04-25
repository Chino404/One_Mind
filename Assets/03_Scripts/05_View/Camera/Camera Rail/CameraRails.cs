using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraRails : MonoBehaviour
{
    [SerializeField] private Rail _myRail;
    NodesOfTheRail _currentNode;
    public Camera myCamera { get; private set;}
    [SerializeField, Range(1,5)] private int _myNumberCamera;
    public int NumberCamera {  get { return _myNumberCamera; } }

    //[SerializeField] bool _mostrarDataNodos;
    [HideInInspector] public bool isTeleporting;
    [SerializeField] private bool _isFixedCamera;

    [Header("-> Components")]
    public CharacterTarget myCharacterTarget;
    public Transform target;

    [SerializeField, Tooltip("Nodo fijo que setea la pos. y la rot. de la cámara")] public Transform fixedNode { get; private set; }

    [Space(6), Header("-> Smoothing Values")]
    public float moveSpeed = 5f;
    public PlayableDirector[] cinematics;

    //[Range(0,1)]public float rotation;
    //public Vector3 iniEuler;
    //public Vector3 endEuler;

    [Space(10), SerializeField] private bool _smoothMove = true;
    [Range(0.01f, 10f)][SerializeField] float _smoothSpeedPosition = 5f;
    private Vector3 _lastPosition;
    private Vector3 _lastRotation;

    Vector3 _targetPosition;

    [Range(0.01f, 10f)][SerializeField] float _smoothSpeedRotation = 2f;

    [Header("-> Camera Offset")]
    [Tooltip("Ajusta la posición de la cámara detrás del personaje.")] public Vector3 cameraOffset = new Vector3(0f, 0f, -9f);
    [Tooltip("Es el offset con el que empieza la cámara.")]private Vector3 _defaultOffset;

    [Space(5)]
    [SerializeField, Range(0, -50f)] private float minXRotation = -20f; // Límite inferior del eje X
    [SerializeField, Range(0, 50f)] private float maxXRotation = 35f;  // Límite superior del eje X

    [Space(5)]
    [SerializeField, Range(0, -20f)] private float minYRotation = -10f; // Límite izquierdo del eje Y
    [SerializeField, Range(0, 20f)] private float maxYRotation = 10f;  // Límite derecho del eje Y

    Vector3 _desiredPos, _smoothPos;
    Quaternion _smoothRot;

    private void Awake()
    {
        myCamera = gameObject.GetComponent<Camera>();

        _lastPosition = transform.position;
        _lastRotation = transform.rotation.eulerAngles;

        _defaultOffset = cameraOffset;

        if (myCharacterTarget == CharacterTarget.Bongo)
        {
            CamerasManager.instance.listBongosRailsCamera.Add(this);

            if (!target) target = GameManager.instance.modelBongo.transform;

            if(_myNumberCamera > 1) gameObject.SetActive(false);

            else CamerasManager.instance.currentBongoCamera = this;
        }

        else if (myCharacterTarget == CharacterTarget.Frank)
        {
            CamerasManager.instance.listFranksRailsCamera.Add(this);

            if (!target) target = GameManager.instance.modelFrank.transform;

            if (_myNumberCamera > 1) gameObject.SetActive(false);

            else CamerasManager.instance.currentFrankCamera = this;
        }

        if (target == null)
        {
            Debug.LogError($"Falta target en: {gameObject.name}");
            return;
        }

        if(_myRail) _myRail.RailTarget = target;  
    }

    /// <summary>
    /// Cambia de Rail.
    /// </summary>
    /// <param name="newRail"></param>
    public void ChangeToRail(Rail newRail)
    {
        if (newRail == _myRail) return;

        _myRail = newRail;

        if(_myRail.RailTarget == null) _myRail.RailTarget = target;

    }

    private void LateUpdate()
    {
        if (target == null) return;

        //Si la cámara va a estar fija.
        if(_isFixedCamera)
        {
            SetPositionAndRotationTarget();
            return;
        }

        _currentNode = _myRail.GetClosestNode(target.position);

        cameraOffset = _currentNode.isChangeTheCameraOffset ? _currentNode.newOffset : _defaultOffset;

        foreach (var item in cinematics)
        {
            if (item.state == PlayState.Playing) return;
        }

        // Calcular la posición deseada detrás del personaje, con el desplazamiento.
        _targetPosition = _myRail.ProjectPositionOnRail(target.position) + cameraOffset;


        // Movimiento suave
        if (_smoothMove)
        {
            _lastPosition = Vector3.Lerp(_lastPosition, _targetPosition, moveSpeed * Time.deltaTime);
            transform.position = _lastPosition;
        }
        else transform.position = _targetPosition;

        #region Puede Servir
        // Suavizar la rotación para que la cámara mire en la dirección del personaje
        //Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _smoothSpeedRotation * Time.deltaTime * 60);
        #endregion

        // Ajustar la rotación de la cámara
        //transform.rotation = AdjustCameraRotationToTheCharacter();

        //Debug.Log(Mathf.Abs(transform.eulerAngles.y));


        RotateCameraNode();
    }

    private void RotateCameraNode()
    {

        //NodesOfTheRail nextNode = currentNode.index == _myRail.nodes.Count - 1 ? currentNode : _myRail.nodes[currentNode.index + 1];
        //NodesOfTheRail previousNode = currentNode.index == 0 ? currentNode : _myRail.nodes[currentNode.index - 1];

        //if (_mostrarNodos)
        //{
        //    Debug.Log($"El nodo mas cercano es el: {currentNode}| El que le sigue es el '{nextNode}' y el anterior es {previousNode}");
        //}


        if (_currentNode.isToRotateTheCamera)
        { 
            // Convertimos _lastRotation a Quaternion
            Quaternion lastQuat = Quaternion.Euler(_lastRotation);
            Quaternion targetQuat = Quaternion.Euler(_currentNode.rotationCamera);

            // Interpolamos correctamente entre las rotaciones
            lastQuat = Quaternion.Slerp(lastQuat, targetQuat, 0.5f * Time.deltaTime); //Slerp: Interpolación esférica, ideal para rotaciones suaves y evita problemas de interpolación cíclica.

            // Aplicamos la nueva rotación
            transform.rotation = lastQuat;

            // Guardamos la rotación en Euler para la próxima iteración
            _lastRotation = lastQuat.eulerAngles;
        }
        else
        {
            transform.rotation = AdjustCameraRotationToTheCharacter();
            _lastRotation = transform.rotation.eulerAngles;
        }

        //Vector3 targetEuler = Vector3.Lerp(iniEuler, endEuler, rotation);
        //transform.rotation = Quaternion.Euler(targetEuler);
    }

    #region Camera Rail
    /// <summary>
    /// La rotación de la cámara respecto al personaje.
    /// </summary>
    private Quaternion AdjustCameraRotationToTheCharacter()
    {
        // Dirección hacia el personaje
        Vector3 direction = target.position - transform.position;

        // Obtener la rotación necesaria para mirar al personaje
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Convertir la rotación a ángulos de Euler
        Vector3 eulerRotation = targetRotation.eulerAngles;

        // Ajustar el eje X para mantenerlo dentro de los límites
        eulerRotation.x = (eulerRotation.x > 180) ? eulerRotation.x - 360 : eulerRotation.x; // Corrige valores de Euler mayores a 180°
        eulerRotation.x = Mathf.Clamp(eulerRotation.x, minXRotation, maxXRotation);

        // Ajustar el eje Y para mantenerlo dentro de los límites
        eulerRotation.y = (eulerRotation.y > 180) ? eulerRotation.y - 360 : eulerRotation.y; // Corrige valores de Euler mayores a 180°
        eulerRotation.y = Mathf.Clamp(eulerRotation.y, minYRotation, maxYRotation);

        // Aplicar la rotación con suavizado
        Quaternion finalRotation = Quaternion.Euler(eulerRotation);

        //transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, _smoothSpeedRotation * Time.deltaTime);

        return Quaternion.Slerp(transform.rotation, finalRotation, _smoothSpeedRotation * Time.deltaTime);
    }

    /// <summary>
    /// Transicionar al Rail.
    /// </summary>
    public void TransitionToRail(Transform newNode = default)
    {
        //if (!_isFixedCamera) return;

        if(_isFixedCamera) _isFixedCamera = false;
        
        if(newNode != default)
        {
            _lastPosition = newNode.position;
            _lastRotation = newNode.rotation.eulerAngles;
        }

        transform.position = _lastPosition;
        transform.rotation = Quaternion.Euler(_lastRotation);
    }
#endregion

    #region Fixed Camera
    /// <summary>
    /// Sete la Pos. y la Rot. cuando la cámara está fija.
    /// </summary>
    private void SetPositionAndRotationTarget()
    {
        if (isTeleporting)
        {
            transform.SetPositionAndRotation(fixedNode.position, fixedNode.rotation);
        }
        else
        {
            // Calcular la posición deseada relativa al punto
            _desiredPos = target.position + (fixedNode.position - target.position);

            // Suavizado en base a Time.deltaTime para asegurar suavidad continua
            _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeedPosition * Time.deltaTime * 60); //Multiplicando por 60 asegura que las velocidades del Lerp sean proporcionales y consistentes
            _smoothRot = Quaternion.Lerp(transform.rotation, fixedNode.rotation, _smoothSpeedRotation * Time.deltaTime * 60);

            //transform.SetPositionAndRotation(_smoothPos, _smoothRot);

            transform.SetPositionAndRotation(fixedNode.position, fixedNode.rotation);
        }

    }

    /// <summary>
    /// Transicionar a un nodo fijo.
    /// </summary>
    /// <param name="newNode"></param>
    public void TransitionToAFixedNode(Transform newNode)
    {
        //if (_isFixedCamera) return;

        if(!_isFixedCamera) _isFixedCamera = true;

        _lastPosition = transform.position;
        _lastRotation = transform.rotation.eulerAngles;

        fixedNode = newNode;
    }

#endregion

}

