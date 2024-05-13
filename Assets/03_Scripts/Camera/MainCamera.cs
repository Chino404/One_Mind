using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.camerasPlayers[0] = gameObject;
    }
}
