using System;
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
    Alta,
    Ancha
}

public class Box : MonoBehaviour
{
    public BoxData[] _prefabsBoxes;

    [SerializeField,Tooltip("Tiempo de vida")] private float _timeToLife;
    [Tooltip("Contador de tiempo para apagarme y guardarme en el objectPool")] private float _counter;

    [SerializeField] private Transform _destiny;

    private ObjectPool<Box> _objectPool;

    private Rigidbody _rb;

    public Vector3 myVelocity;

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

        //if (dist <= 0.5f) Debug.Log("LLEGO A LA META");

        if(dist <= 0.5f  || _counter >= _timeToLife) _objectPool.StockAdd(this);

        myVelocity = _rb.velocity;
    }



    public void ChangeBox(BoxType type)
    {
        foreach (var item in _prefabsBoxes)
        {
            if (item.boxType == type)
            {
                item.prefab.gameObject.SetActive(true);
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
        foreach (var item in box._prefabsBoxes)
        {
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

