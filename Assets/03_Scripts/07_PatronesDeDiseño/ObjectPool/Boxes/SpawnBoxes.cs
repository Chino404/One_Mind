using UnityEngine;
using UnityEngine.UIElements;

public enum WayOfSpawning //Modo de spawn
{
    Seguidos,
    Random,
    Seleccionados
}

public class SpawnBoxes : MonoBehaviour
{
    [Space(5), Tooltip("Modo en el que se van a spawnear")] public WayOfSpawning way;
    [Tooltip("Colocar la caja que queres que salga")]public BoxType[] selectBoxesToSpawn;
    private int _indexBoxesToSpawn;

    [Space(5), SerializeField] private Transform _iniPos;
    [SerializeField] private Transform _endPos;

    private BoxType _myBoxType;

    [Space(10), Tooltip("Cooldown para pedir el objeto en el objectpool")][SerializeField] float _coolDown = 1.7f;

    private void Start()
    {
        InvokeRepeating("SpawnBox", 0f, _coolDown);     
    }

    private void SpawnBox()
    {
        if (!_iniPos)
        {
            Debug.LogError($"Falta la posicion en la que se va a instanciar la caja en : {gameObject.name}");
            return;
        }

        if (!_endPos)
        {
            Debug.LogError($"Falta la posicion en la que se va a desactivar la caja en : {gameObject.name}");
            return;
        }

        //var box = OPSBoxesManager.objectPool.Get();
        //ChangeBoxAndEnum(box);
        //box.AddReference(OPSBoxesManager.objectPool);
        //box.SetPos(_iniPos, _endPos);

        ChangeWay(way);

    }

    private void ChangeWay(WayOfSpawning type)
    {
        switch (type)
        {
            case WayOfSpawning.Seguidos:
                {
                    var box = OPSBoxesManager.instance.GetBox(_myBoxType);
                    box.SetPos(_iniPos, _endPos);

                    // Convierte el estado actual a entero, avanza al siguiente valor
                    int estadoSiguiente = ((int)_myBoxType + 1) % System.Enum.GetValues(typeof(BoxType)).Length;
                    _myBoxType = (BoxType)estadoSiguiente;
                }
                break;

            case WayOfSpawning.Random:
                {
                    var box = OPSBoxesManager.instance.GetBox(_myBoxType);
                    box.SetPos(_iniPos, _endPos);

                    // Obtén todos los valores del enum y selecciona uno aleatorio
                    System.Array valores = System.Enum.GetValues(typeof(BoxType));
                    _myBoxType = (BoxType)valores.GetValue(Random.Range(0, valores.Length));
                }
                break;

            case WayOfSpawning.Seleccionados:
                {
                    _myBoxType = selectBoxesToSpawn[_indexBoxesToSpawn];

                    var box = OPSBoxesManager.instance.GetBox(_myBoxType);
                    box.SetPos(_iniPos, _endPos);

                    _indexBoxesToSpawn++;
                    if (_indexBoxesToSpawn >= selectBoxesToSpawn.Length) _indexBoxesToSpawn = 0;

                }
                break;

            default:
                break;
        }
    }

}
