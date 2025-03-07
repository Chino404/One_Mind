using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsOfTheCameraRails : MonoBehaviour
{
    public CharacterTarget player;

    [Space(7), Tooltip("Puntos anteriores")] public PointsOfTheCameraRails[] previousNodes;

    [Space(7), Tooltip("Puntos siguientes")] public PointsOfTheCameraRails[] nextNodes;

    private void OnDrawGizmosSelected()
    {
        if(nextNodes.Length != 0)
        {
            Color gizmoColorNextPoint = Color.green;

            foreach (var point in nextNodes)
            {
                Gizmos.color = gizmoColorNextPoint;
                Gizmos.DrawLine(transform.position, point.transform.position);
            }
        }

        if (previousNodes.Length != 0)
        {
            Color gizmoColorpreviousPoint = Color.yellow;

            foreach (var point in previousNodes)
            {
                Gizmos.color = gizmoColorpreviousPoint;
                Gizmos.DrawLine(transform.position, point.transform.position);
            }
        }

    }
}
