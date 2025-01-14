using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesFactory : Factory<Boxes>
{
    public BoxesFactory(Boxes p)//En un constructor se entra cuando se crea
    {
        prefab = p;
    }
}
