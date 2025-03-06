using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsOfTheCameraRails : MonoBehaviour
{
    [Space(7), Tooltip("Puntos anteriores")] public PointsOfTheCameraRails[] previousPoints;

    [Space(7), Tooltip("Puntos siguientes")] public PointsOfTheCameraRails[] nextPoints;


    private void OnDrawGizmosSelected()
    {
        if(nextPoints.Length != 0)
        {
            Color gizmoColorNextPoint = Color.green;

            foreach (var point in nextPoints)
            {
                Gizmos.color = gizmoColorNextPoint;
                Gizmos.DrawLine(transform.position, point.transform.position);
            }
        }

        if (previousPoints.Length != 0)
        {
            Color gizmoColorpreviousPoint = Color.yellow;

            foreach (var point in previousPoints)
            {
                Gizmos.color = gizmoColorpreviousPoint;
                Gizmos.DrawLine(transform.position, point.transform.position);
            }
        }

    }
}
