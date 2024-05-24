using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public GameObject shadow;
    private GameObject _circleInstance; // Instancia del círculo 2D
    private CanvasGroup _canvasGroup; // El componente CanvasGroup para su Alpha

    private float _timer = 0;
    private float _transicionDuration = 0.25f;

    private void Start()
    {
        _circleInstance = Instantiate(shadow);

        _canvasGroup = _circleInstance.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (!GameManager.instance.players[0].GetComponent<ModelMonkey>().IsGrounded())
        {
            PerformRaycast();
        }
        else
        {
            _timer = 0;
            _circleInstance.SetActive(false);
        }
    }

    void PerformRaycast()
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        Ray ray = new Ray(pos, dir);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            _circleInstance.SetActive(true);
            _circleInstance.transform.position = hit.point + Vector3.up * 0.05f;

            if (_timer < _transicionDuration) _timer += Time.deltaTime;

            _canvasGroup.alpha = Mathf.Lerp(0 ,1 , _timer / _transicionDuration);
        }

        //Visualizar el raycast en la escena
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 0.1f); // Dibuja el raycast en la escena para visualizarlo
    }

}

