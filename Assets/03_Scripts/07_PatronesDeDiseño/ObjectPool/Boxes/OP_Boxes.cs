using UnityEngine;

public class OP_Boxes : MonoBehaviour
{
    [SerializeField] private Box _boxesModel;
    [Tooltip("cantidad de cajas que instancio al principio")][SerializeField] int _boxesQuantity;

    private Factory<Box> _factory;

    public static ObjectPool<Box> objectPool;

    private void Awake()
    {
        _factory = new BoxesFactory(_boxesModel);
        objectPool = new ObjectPool<Box>(_factory.GetObj, Box.TurnOff, Box.TurnOn, _boxesQuantity);
    }
}
