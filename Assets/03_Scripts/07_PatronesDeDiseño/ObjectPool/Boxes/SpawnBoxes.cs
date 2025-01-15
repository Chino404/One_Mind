using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoxes : MonoBehaviour
{
    public BoxType boxType;
    [Tooltip("Cooldown para pedir el objeto en el objectpool")][SerializeField] float _coolDown;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            var box = OP_Boxes.objectPool.GetByEnum(boxType);
            //var box = OP_Boxes.objectPool.Get();
            box.AddReference(OP_Boxes.objectPool);
            box.transform.position = transform.position;
            box.transform.forward = transform.forward;
        }
    }
}
