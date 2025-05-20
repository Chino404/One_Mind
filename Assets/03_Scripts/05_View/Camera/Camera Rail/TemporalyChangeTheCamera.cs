using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalyChangeTheCamera : MonoBehaviour
{
    [SerializeField] private CharacterTarget _targetPLayer;

    [Space(5),SerializeField] private GameObject _fiexdCamera;
    private Camera _camera;
    [SerializeField] private LayerMask _objToInteract;

    [Space(5), SerializeField, Range(0, 10f)] private float _timeActive;

    private void Awake()
    {
        if (_fiexdCamera)
        {
            if (!_fiexdCamera.gameObject.GetComponent<Camera>()) Debug.LogError($"El objeto puesto en <color=yellow>{gameObject.name}</color> no tiene le componente cemara.");
            else _camera = _fiexdCamera.gameObject.gameObject.GetComponent<Camera>();
        }
        else Debug.Log($"Falta asigna una cáamra en <color=yellow>{gameObject.name}</color>");
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((1 << obj.layer) & layerMask.value) != 0;

        #region Eplicacion código
        /*Mueve los bits de un número hacia la izquierda, agregando ceros por la derecha. Por cada posición que se mueve, se multiplica el número por 2.

            int x = 1;          // En binario: 0001
            int y = x << 2;     // Se convierte en: 0100 (o sea 4 en decimal)

          Es decir:

          1 << 0 = 1

          1 << 1 = 2

          1 << 2 = 4

          1 << 3 = 8

          1 << 4 = 16
          ...y así sucesivamente.
         
          ¿Por qué se usa con LayerMask?
          Cada Layer en Unity está representado por un bit en una máscara de 32 bits.Por ejemplo:

          Layer 0 = 1 << 0 → 0000 0001

          Layer 1 = 1 << 1 → 0000 0010

          Layer 2 = 1 << 2 → 0000 0100
        
          Entonces, si hacés:

          | (1 << obj.layer) & layerMask.value |

          Estás preguntando si el bit correspondiente al layer del objeto está activado en el LayerMask.
          Si el resultado no es cero, significa que ese layer está incluido.*/
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if(IsInLayerMask(other.gameObject, _objToInteract))
        {
            Debug.Log("ES UN BLOQUE DE HIELO");
            StartCoroutine(ChangeCamera());
        }
    }

    IEnumerator ChangeCamera()
    {
        if(_targetPLayer == CharacterTarget.Bongo) CamerasManager.instance.currentBongoCamera.GetComponent<Camera>().enabled = false;
        else if (_targetPLayer == CharacterTarget.Frank) CamerasManager.instance.currentFrankCamera.GetComponent<Camera>().enabled = false;

        _camera.enabled = true;
        GameManager.instance.isNotMoveThePlayer = true;

        yield return new WaitForSeconds(_timeActive);

        if (_targetPLayer == CharacterTarget.Bongo) CamerasManager.instance.currentBongoCamera.GetComponent<Camera>().enabled = true;
        else if (_targetPLayer == CharacterTarget.Frank) CamerasManager.instance.currentFrankCamera.GetComponent<Camera>().enabled = true;

        _camera.enabled = false;
        GameManager.instance.isNotMoveThePlayer = false;
    }
}
