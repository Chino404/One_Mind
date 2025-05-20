using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsForTheCamera : MonoBehaviour
{
    public CharacterTarget characterTarget;
    [HideInInspector]public Transform player;

    private void Awake()
    {
        GameManager.instance.pointsNormalCamera.Add(this);   
    }

    private void Start()
    {
        if (characterTarget == CharacterTarget.Bongo) player = GameManager.instance.modelBongo.transform;
        else if (characterTarget == CharacterTarget.Frank) player = GameManager.instance.modelFrank.transform;
    }

    private void FixedUpdate()
    {
        transform.position = player.position;
    }
}
