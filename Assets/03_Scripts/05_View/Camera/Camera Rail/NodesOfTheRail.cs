using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesOfTheRail : MonoBehaviour
{
    public CharacterTarget player;
    [SerializeField] public int index;

    [Space(7)]
    [Tooltip("Puntos siguientes")] public NodesOfTheRail[] neighborNode;

    [HideInInspector, Tooltip("Es para girar la c�mara.")] public bool isToRotateTheCamera;
    [HideInInspector, Tooltip("La rotaci�n de la c�mara cuando llegeu a este nodo.")] public Quaternion rotationCamera;

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
