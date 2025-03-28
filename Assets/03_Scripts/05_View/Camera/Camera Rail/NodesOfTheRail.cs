using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesOfTheRail : MonoBehaviour
{
    public CharacterTarget player;
    [SerializeField] public int index;

    [Space(7)]
    [Tooltip("Puntos siguientes")] public NodesOfTheRail[] neighborNode;

    [Space(5)]
    [SerializeField, Tooltip("Es para girar la cámara.")] public bool isToRotateTheCamera;
    [SerializeField, Tooltip("La rotación de la cámara cuando llegeu a este nodo.")] public Quaternion rotationCamera;

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
