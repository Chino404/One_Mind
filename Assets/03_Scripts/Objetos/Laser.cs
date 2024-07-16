using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [Header("LASER WARNING VALUES")]
    [Tooltip("Tiempo en que el laser esta desactivado")]public float timeDisableLaser = 3f;
    [Tooltip("Duracion de la advertencia del laser")]public float durationLaserWarning = 2f;
    [Range(0, 1)]public float widthLaserWarning;
    private float _timer;
    public Transform startPoint;
    public Transform endPoint;
    [SerializeField] private LayerMask _layerMask;
    private bool _activeWarning;

    private LineRenderer _laserLine;

    private void Awake()
    {
        _laserLine = GetComponent<LineRenderer>();
        if (_laserLine == null) Debug.LogError($"Falta el componente LineRenderer en {gameObject.name}");
        if (startPoint == null) Debug.LogError($"Falta el StartPoint en el {gameObject.name}");
        if (endPoint == null) Debug.LogError($"Falta el EndPoint en el {gameObject.name}");

        _laserLine.startWidth = 0;
    }

    private void Update()
    {
        if(!_activeWarning && _timer >= timeDisableLaser)
        {
            _activeWarning = true;
            StartCoroutine(ActiveWarning());
        }
        else if(_timer < timeDisableLaser) _timer += Time.deltaTime;

        _laserLine.SetPosition(0, startPoint.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.up, out hit, Mathf.Infinity, _layerMask))
        {
            endPoint.transform.position = hit.point;
            _laserLine.SetPosition(1, endPoint.transform.position);
            _laserLine.enabled = true;
        }
        else Debug.LogError("No choca con nada");

    }

    private void ActiveLaserWarning()
    {
        _laserLine.SetPosition(0, startPoint.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.up, out hit, Mathf.Infinity, _layerMask))
        {
            endPoint.transform.position = hit.point;
            _laserLine.SetPosition(1, endPoint.transform.position);
            _laserLine.enabled = true;
            StartCoroutine(InterpolateWidth());
        }
        else Debug.LogError("No choca con nada");

    }

    private void DesactiveLaserWarning()
    {
        _laserLine.enabled = false;
        _activeWarning = false;
        _timer = 0;
    }


    IEnumerator ActiveWarning()
    {
        StartCoroutine(InterpolateWidth());
        yield return new WaitForSeconds(durationLaserWarning);
        DesactiveLaserWarning();
    }

    IEnumerator InterpolateWidth()
    {
        float elapsedTime = 0;

        while (elapsedTime < durationLaserWarning)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / durationLaserWarning;
            _laserLine.startWidth = Mathf.Lerp(0, widthLaserWarning, t);
            yield return null;
        }

        _laserLine.startWidth = 0;
    }

}
