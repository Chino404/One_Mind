using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesOfTheRail : MonoBehaviour
{
    public CharacterTarget player;
    [SerializeField] public int index;

    [Space(7)]
    [Tooltip("Puntos siguientes")] public NodesOfTheRail[] neighborNode;

    [HideInInspector, Tooltip("Es para girar la cámara.")] public bool isToRotateTheCamera;
    [HideInInspector, Tooltip("La rotación de la cámara cuando llegeu a este nodo.")] public Vector3 rotationCamera;

    [HideInInspector, Tooltip("Para modificar el FOV de la cámara.")] public bool isChangeTheCameraOffset = false;
    [HideInInspector, Range(40,120), Tooltip("Nuevo valor del offset")] public Vector3 newOffset;

    private void OnDrawGizmosSelected()
    {
        if(neighborNode.Length != 0)
        {
            Color gizmoColorNextPoint = Color.green;

            foreach (var point in neighborNode)
            {
                if (!point)
                {
                    Debug.LogError("Hay espacio vacio.");
                    continue;
                }

                Gizmos.color = gizmoColorNextPoint;
                Gizmos.DrawLine(transform.position, point.transform.position);
            }
        }

    }
}
