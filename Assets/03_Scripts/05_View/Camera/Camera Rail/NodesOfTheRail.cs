using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesOfTheRail : MonoBehaviour
{
    public CharacterTarget player;

    [Space(7), Tooltip("Puntos siguientes")] public NodesOfTheRail[] neighborNode;

    private void OnDrawGizmosSelected()
    {
        if(neighborNode.Length != 0)
        {
            Color gizmoColorNextPoint = Color.green;

            foreach (var point in neighborNode)
            {
                Gizmos.color = gizmoColorNextPoint;
                Gizmos.DrawLine(transform.position, point.transform.position);
            }
        }

    }
}
