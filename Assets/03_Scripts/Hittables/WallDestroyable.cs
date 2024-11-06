using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroyable : MonoBehaviour, IHittable
{
    public void Action()
    {
        gameObject.SetActive(false);
    }
}
