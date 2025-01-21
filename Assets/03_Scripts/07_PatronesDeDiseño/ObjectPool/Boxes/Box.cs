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

public class Box : Platform
{
    [Tooltip("Los distintos modelos de cajas")]public BoxData[] prefabsBoxes;

    [SerializeField,Tooltip("Tiempo de vida")] private float _timeToLife;
    [Tooltip("Contador de tiempo para apagarme y guardarme en el objectPool")] private float _counter;

    [Tooltip("")] private Transform _destiny;
    [SerializeField, Tooltip("La posicion del prefab")] private Transform _posPrefabIni;

    private ObjectPool<Box> _objectPool;

    private Rigidbody _myRb;

    private void Awake()
    {
        _myRb = GetComponent<Rigidbody>();
        _myRb.velocity = Vector3.zero;

        _myRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        _isTrigger = true;
    }

    public override void Update()
    {
        base.Update();

        _counter += Time.deltaTime;

        var dist = Vector3.Distance(transform.position, _destiny.position);

        if (dist <= 0.5f || _counter >= _timeToLife) _objectPool.StockAdd(this);
    }


    public void ChangeBox(BoxType type)
    {
        foreach (BoxData currentBox in prefabsBoxes)
        {
            if (currentBox.boxType == type)
            {
                _posPrefabIni = currentBox.prefab.gameObject.GetComponentInChildren<Component>().transform; //Obtengo el transform del prefab
                _posPrefabIni.position = transform.position; //Lo igualo a la posicion del objeto

                currentBox.prefab.gameObject.SetActive(true); //Lo enciendo

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
        foreach (var currentBox in box.prefabsBoxes) //Recorro cada uno por las dudas
        {
            currentBox.prefab.SetActive(false);
        }      
        
        box._rbCharacter = null;

        box._myRb.drag = 0;
        box._myRb.velocity = Vector3.zero;

        box.gameObject.SetActive(false);
    }

    public static void TurnOn(Box box)
    {
        box._counter = 0;
        box.gameObject.SetActive(true);
    }


}

