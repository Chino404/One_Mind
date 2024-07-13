using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappeable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<ModelMonkey>();

        if (player)
        {
            EventManager.Unsubscribe("ActualMovement", player.NormalMovement);
            EventManager.Subscribe("ActualMovement", player.HandleMovement);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<ModelMonkey>();

        if (player)
        {
            EventManager.Unsubscribe("ActualMovement", player.HandleMovement);
            EventManager.Subscribe("ActualMovement", player.NormalMovement);
        }
    }
}
