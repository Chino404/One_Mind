using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Shadow : MonoBehaviour
{
    public GameObject shadow;
    [SerializeField] private Characters _character;

    private GameObject _circleInstance; // Instancia del círculo 2D
    [SerializeField, Tooltip("En que layer va a aparecer la sombra")] private LayerMask _layerToCastShadow;
    [SerializeField] private float _iniScale;
    [SerializeField] private float _newScale;

    private float disRayScale = 4f;
    private float timeScale = 0.5f;

    private void Start()
    {
        if(_circleInstance == null)
            _circleInstance = Instantiate(shadow);

        _circleInstance.transform.localScale = IniScale();

        _character = gameObject.GetComponentInParent<Characters>();
    }

    void Update()
    {
        RaycastShadow();

        //if (!GameManager.instance.players[0].GetComponent<ModelMonkey>().IsGrounded())
        //{
        //    RaycastScaleShadow();
        //}

        if (_character.IsGrounded(_layerToCastShadow))
        {
            RaycastScaleShadow();
        }
        else
        {
            _circleInstance.transform.localScale = IniScale();
        }
    }

    void RaycastScaleShadow()
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        Ray ray = new Ray(pos, dir);

        RaycastHit hit;

        Debug.DrawLine(pos, pos + (dir * disRayScale));

        if (Physics.Raycast(ray, out hit, disRayScale))
        {
            float distance = hit.distance;
            float t = Mathf.Clamp01(distance / disRayScale); //En funcio a la distancia del rayast
            Vector3 targetScale = Vector3.Lerp(IniScale(), new Vector3(_newScale, _newScale, _newScale), t);

            Debug.DrawLine(pos, pos + (dir * disRayScale));

            float startTime = 0;
            if (startTime < timeScale)
            {
                startTime++;
                _circleInstance.transform.localScale = Vector3.Lerp(_circleInstance.transform.localScale, targetScale, startTime / timeScale);
            }
        }
    }

    void RaycastShadow()
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        Ray ray = new Ray(pos, dir);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerToCastShadow))
        {
            _circleInstance.transform.position = hit.point + Vector3.up * 0.05f;

            //if (_timer < _transicionDuration) _timer += Time.deltaTime;
        }
        else
        {
            _circleInstance.transform.localScale = new Vector3(0,0,0);
        }

        

        //Visualizar el raycast en la escena
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 0.1f); // Dibuja el raycast en la escena para visualizarlo
    }

    private Vector3 IniScale()
    {
        return new Vector3(_iniScale, _iniScale, _iniScale);
    }
}

