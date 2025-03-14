using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [Header("LASER WARNING VALUES")]
    [Space(5), SerializeField] private LayerMask _layerObject;
    public Transform startPoint;
    public Transform endPoint;

    [Space(10),Header("-> Timers")]
    [Tooltip("Tiempo en que el laser esta desactivado")]public float timeDisableLaser = 3f;
    [Tooltip("Duracion de la advertencia del laser")]public float durationLaserWarning = 2f;
    [Tooltip("Duracion del laser")]public float durationLaser = 3f;
    [Range(0, 1)]public float widthLaserWarning;

    private float _timer;
    private RaycastHit _hit;
    private RaycastHit _hitTarget;
    private bool _activeWarning;
    private bool _activeLaser;

    [Space(10), SerializeField] private Vector3 _boxCastSize = new Vector3(1, 1, 1); // Tamaño del cubo
    [Tooltip("Rotación del cubo")] private Quaternion _rotationBoxCast;
    [SerializeField]private LineRenderer _lineRenderer;

    private bool _isStart;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        if (_lineRenderer == null) Debug.LogError($"Falta el componente LineRenderer en {gameObject.name}");
        if (startPoint == null) Debug.LogError($"Falta el StartPoint en el {gameObject.name}");
        if (endPoint == null) Debug.LogError($"Falta el EndPoint en el {gameObject.name}");

        _lineRenderer.startWidth = 0;

        _isStart = true;
    }

    private void Update()
    {
        _rotationBoxCast = transform.rotation;

        _lineRenderer.SetPosition(0, startPoint.transform.position);

        if(!_activeWarning && _timer >= timeDisableLaser)
        {
            _activeWarning = true;
            StartCoroutine(ActiveWarning());
        }
        else if(_timer < timeDisableLaser) _timer += Time.deltaTime;

        

        //Raycast para el LineRenderer
        if (Physics.Raycast(startPoint.position, (endPoint.position - startPoint.position).normalized, out _hit, Mathf.Infinity, _layerObject))
        {
            endPoint.position = _hit.point;
            _lineRenderer.SetPosition(1, endPoint.transform.position);

            _boxCastSize = new Vector3(_boxCastSize.x, Vector3.Distance(startPoint.position, endPoint.position), _boxCastSize.z);

            _lineRenderer.enabled = true;
        }
        else Debug.LogError("No choca con nada");

        if (_activeLaser)
        {
            // Dirección del Raycast (hacia el punto final)
            Vector3 direction = (endPoint.position - startPoint.position).normalized;

            // Calcular la distancia
            float distance = Vector3.Distance(startPoint.position, endPoint.position);

            // Realizar el BoxCast desde el punto de inicio hacia la dirección
            if (Physics.BoxCast(startPoint.position - direction * (distance / 2), _boxCastSize / 2, direction, out _hitTarget, transform.rotation, distance))
            {
                var targetComponent = _hitTarget.collider.GetComponent<Characters>();

                if (targetComponent != null)
                {
                    targetComponent.TakeDamageEntity(100, transform.position);
                    Debug.Log("A llorar monito");
                }
            }
        }
    }



    IEnumerator ActiveWarning()
    {
        StartCoroutine(InterpolateWidthLaserWarning());

        yield return new WaitForSeconds(durationLaserWarning);

        _lineRenderer.enabled = false;
        _activeLaser = true;
        _lineRenderer.startWidth = 1;

        yield return new WaitForSeconds(durationLaser);

        _lineRenderer.startWidth = 0;
        _activeWarning = false;
        _activeLaser = false;
        _timer = 0;
    }

    IEnumerator InterpolateWidthLaserWarning()
    {
        float elapsedTime = 0;

        while (elapsedTime < durationLaserWarning)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationLaserWarning;
            _lineRenderer.startWidth = Mathf.Lerp(0, widthLaserWarning, t);
            yield return null;
        }

        //_lineRenderer.startWidth = 0;
    }

    void OnDrawGizmos()
    {
        if (startPoint == null || endPoint == null) return;

        Vector3 direction = (endPoint.position - startPoint.position).normalized; //Dirección del Raycast (hacia el punto final)

        float distance = Vector3.Distance(startPoint.position, endPoint.position); //Caluclo la distancia

        //CUBO
        // Dibujar el cubo en el punto de inicio con la dirección y tamaño correcto
        Gizmos.color = Color.red;
        var centerBox = startPoint.position + direction * (distance / 2);

        // Guarda la matriz actual
        if(_isStart)Gizmos.matrix = Matrix4x4.TRS(centerBox, _rotationBoxCast, _boxCastSize);

        // Dibuja el cubo en la posición y rotación especificadas
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

        // Restaura la matriz original
        Gizmos.matrix = Matrix4x4.identity;


        //LINEA
        // Dibujar una línea entre los dos puntos
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }

}
