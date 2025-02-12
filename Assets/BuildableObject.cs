using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    public GameObject activableObject;
    public void Activate()
    {
        activableObject.SetActive(true);
    }
}
