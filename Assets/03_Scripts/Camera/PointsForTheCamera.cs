using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsForTheCamera : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        GameManager.instance.points = this;
    }

    private void FixedUpdate()
    {
        //player = GameManager.instance.assignedPlayer;
        transform.position = player.position;
    }
}
