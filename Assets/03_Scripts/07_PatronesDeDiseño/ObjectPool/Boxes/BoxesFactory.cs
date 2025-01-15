using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesFactory : Factory<Box>
{
    public BoxesFactory(Box p)//En un constructor se entra cuando se crea
    {
        prefab = p;
    }

    public override Box GetObj()
    {
        Debug.Log("Se creo una caja");
        return base.GetObj();
    }
}
