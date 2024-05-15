using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : Factory<BulletBanana>
{
    
    public BulletFactory(BulletBanana bullet)
    {
        prefab = bullet;
    }

   
}
