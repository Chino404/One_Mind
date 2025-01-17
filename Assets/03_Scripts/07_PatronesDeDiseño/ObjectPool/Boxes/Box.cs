using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BoxData
{
    public BoxType boxType;
    public GameObject prefab;
}

public enum BoxType
{
    Normal,
    Hueco,
    AltaTecho,
    Ancha
}

public class Box : MonoBehaviour
{
    [Tooltip("Los distintos modelos de cajas")]public BoxData[] prefabsBoxes;

    [SerializeField,Tooltip("Tiempo de vida")] private float _timeToLife;
    [Tooltip("Contador de tiempo para apagarme y guardarme en el objectPool")] private float _counter;

    [Tooltip("")] private Transform _destiny;
    [SerializeField, Tooltip("La posicion del prefab")] private Transform _posPrefabIni;

    private ObjectPool<Box> _objectPool;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;

        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        _counter += Time.deltaTime;

        var dist = Vector3.Distance(transform.position, _destiny.position);

        if(dist <= 0.5f  || _counter >= _timeToLife) _objectPool.StockAdd(this);
    }



    public void ChangeBox(BoxType type)
    {
        foreach (BoxData item in prefabsBoxes)
        {
            if (item.boxType == type)
            {
                _posPrefabIni = item.prefab.gameObject.GetComponentInChildren<Component>().transform; //Obtengo el transform del prefab
                _posPrefabIni.position = transform.position; //Lo igualo a la posicion del objeto

                item.prefab.gameObject.SetActive(true); //Lo enciendo

                break;
            }
        }
    }

    public void SetPos(Transform posIni, Transform posEnd)
    {
        transform.position = posIni.position;

        transform.forward = posIni.forward;

        _destiny = posEnd;
    }

    public void AddReference(ObjectPool<Box> objPool) => _objectPool = objPool;

    //Son estaticos para que no necesite pasar la referencia del script directamente!
    public static void TurnOff(Box box)
    {
        foreach (var item in box.prefabsBoxes) //Recorro cada uno por las dudas
        {
            box._rb.drag = 0;
            item.prefab.SetActive(false);
        }      

        box._rb.velocity = Vector3.zero;
        box.gameObject.SetActive(false);
    }

    public static void TurnOn(Box box)
    {
        box._counter = 0;
        box.gameObject.SetActive(true);
    }


}

