using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesFactory : Factory<Box>
{
    private Box[] m_Boxes;

    public BoxesFactory(Box p)//En un constructor se entra cuando se crea
    {
        prefab = p;

        //m_Boxes = p;
    }

    /// <summary>
    /// Obtengo cajas
    /// </summary>
    /// <returns></returns>
    //public override Box GetObj()
    //{
    //    Debug.LogWarning("Cree caja");

    //    return m_Boxes;

    //    //return base.GetObj();
    //}

    //public Box[] GetObj()
    //{
    //    return m_Boxes;
    //}

}
