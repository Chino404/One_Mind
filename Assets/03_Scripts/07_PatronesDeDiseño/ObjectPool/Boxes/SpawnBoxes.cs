using UnityEngine;

public enum WayOfSpawning //Modo de spawn
{
    Seguidos,
    Random,
    Seleccionados
}

public class SpawnBoxes : MonoBehaviour
{
    [Space(5), Tooltip("Modo en el que se van a spawnear")] public WayOfSpawning way;
    [Tooltip("Colocar el nombre del enum que deseas")]public string[] selectBoxes;
    private int _indexString;

    [Space(5), SerializeField] private Transform _iniPos;
    [SerializeField] private Transform _endPos;

    /*[Space(5), SerializeField]*/ private BoxType _myBoxType;

    [Space(10), Tooltip("Cooldown para pedir el objeto en el objectpool")][SerializeField] float _coolDown;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if (!_iniPos)
            {
                Debug.LogError($"Falta la posicion en la que se va a instanciar la caja en : {gameObject.name}");
                return;
            }

            if(!_endPos)
            {
                Debug.LogError($"Falta la posicion en la que se va a desactivar la caja en : {gameObject.name}");
                return;
            }

            var box = OP_Boxes.objectPool.Get();
            ChangeBoxAndEnum(box);
            box.AddReference(OP_Boxes.objectPool);
            box.SetPos(_iniPos, _endPos);
            
            //box.transform.position = transform.position;
            //box.transform.forward = transform.forward;
       
        }
    }

    private void ChangeBoxAndEnum(Box box)
    {
        switch (way)
        {
            case WayOfSpawning.Seguidos:
                {
                    box.ChangeBox(_myBoxType);

                    // Convierte el estado actual a entero, avanza al siguiente valor
                    int estadoSiguiente = ((int)_myBoxType + 1) % System.Enum.GetValues(typeof(BoxType)).Length;
                    _myBoxType = (BoxType)estadoSiguiente;
                }
                break;

            case WayOfSpawning.Random:
                {
                    box.ChangeBox(_myBoxType);

                    // Obtén todos los valores del enum y selecciona uno aleatorio
                    System.Array valores = System.Enum.GetValues(typeof(BoxType));
                    _myBoxType = (BoxType)valores.GetValue(Random.Range(0, valores.Length));
                }
                break;

            case WayOfSpawning.Seleccionados:
                {
                    if (System.Enum.TryParse(selectBoxes[_indexString], out BoxType type))
                    {
                        _myBoxType = type;

                        box.ChangeBox(_myBoxType);

                        _indexString++;
                        if (_indexString >= selectBoxes.Length) _indexString = 0;
                    }
                    else
                    {
                        Debug.Log($"El string {selectBoxes[_indexString]} no corresponde a ningún valor del enum.");
                    }
                }
                break;

            default:
                break;
        }
    }
}
