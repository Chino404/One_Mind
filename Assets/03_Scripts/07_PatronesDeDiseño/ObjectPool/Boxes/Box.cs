using System;
using Unity.VisualScripting;
using UnityEngine;


public enum BoxType
{
    Normal,
    Hueco,
    AltaTecho,
    Ancha
}

public class Box : Platform
{
    public BoxType type;

    [SerializeField,Tooltip("Tiempo de vida")] private float _timeToLife;
    [Tooltip("Contador de tiempo para apagarme y guardarme en el objectPool")] private float _counter;

    [Tooltip("")] private Transform _destiny;
    [Tooltip("La posicion del prefab")] private Transform _posPrefabIni;

    private ObjectPool<Box> _objectPool;
    private Rigidbody _myRb;

    private void Awake()
    {
        _myRb = GetComponent<Rigidbody>();
        _myRb.velocity = Vector3.zero;

        _myRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        _isTrigger = true;

        //_posPrefabIni = gameObject.GetComponentInChildren<Component>().transform; //Obtengo el transform del prefab
    }

    public override void Update()
    {
        base.Update();

        _counter += Time.deltaTime;

        var dist = Vector3.Distance(transform.position, _destiny.position);

        if (dist <= 0.5f || _counter >= _timeToLife) _objectPool.StockAdd(this);
    }

    /// <summary>
    /// Sete la posicion en donde va a spawnear y en donde va a desaparecer la box
    /// </summary>
    /// <param name="posIni"></param>
    /// <param name="posEnd"></param>
    public void SetPos(Transform posIni, Transform posEnd)
    {
        transform.position = posIni.position;

        transform.forward = posIni.forward;

        _destiny = posEnd;
    }

    public void AddReference(ObjectPool<Box> objPool) => _objectPool = objPool;

    /// <summary>
    /// Cuando se apaga
    /// </summary>
    /// <param name="box"></param>
    public static void TurnOff(Box box)
    {
        box._rbCharacter = null;

        box._myRb.drag = 0;
        box._myRb.velocity = Vector3.zero;

        box.gameObject.SetActive(false);
    }

    /// <summary>
    /// Cuando se prenda
    /// </summary>
    /// <param name="box"></param>
    public static void TurnOn(Box box)
    {
        box._counter = 0;
        box.gameObject.SetActive(true);
    }


}

