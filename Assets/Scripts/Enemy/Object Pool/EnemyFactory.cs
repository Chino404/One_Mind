using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : Factory<Enemy>
{
    public EnemyFactory (Enemy enemy)
    {
        prefab = enemy;
    }
}
