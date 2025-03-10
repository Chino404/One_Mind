using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] private Transform _target;
    public Transform RailTarget { get => _target; set => _target = value; }

    public List<Transform> nodes;

    private int _nodeCount;

    private void Awake()
    {
        _nodeCount = transform.childCount;
        //nodes = new Transform[_nodeCount];

        for (int i = 0; i < _nodeCount; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<NodesOfTheRail>())
            {
                //nodes[i] = transform.GetChild(i);
                nodes.Add(transform.GetChild(i));
            }

        }
    }

    /// <summary>
    /// Asegura que la cámara (o lo que estés moviendo) siga el riel correctamente, alineándola con el segmento más cercano.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public Vector3 ProjectPositionOnRail(Vector3 player)
    {
        int closestNodeIndex = GetClosestNode(player); // Obtengo el nodo más cercano.


        //Si el nodo más cercano es el primero o el último de la lista, proyecta player sobre el segmento que conecta con el siguiente o anterior nodo, respectivamente.
        if (closestNodeIndex == 0) //Primer nodo
        {
            return ProjectOnSegment(nodes[0].position, nodes[1].position, _target.position);
        }

        if (closestNodeIndex == nodes.Count - 1) //Último nodo
        {
            return ProjectOnSegment(nodes[nodes.Count - 1].position, nodes[nodes.Count - 2].position, _target.position);
        }

        Vector3 leftSeg = ProjectOnSegment(nodes[closestNodeIndex - 1].position, nodes[closestNodeIndex].position, player);
        Vector3 rightSeg = ProjectOnSegment(nodes[closestNodeIndex + 1].position, nodes[closestNodeIndex].position, player);

        Debug.DrawLine(player, leftSeg, Color.red);
        Debug.DrawLine(player, rightSeg, Color.blue);

        return (player - leftSeg).sqrMagnitude <= (player - rightSeg).sqrMagnitude ? leftSeg : rightSeg;
    }

    /// <summary>
    /// Determina qué nodo está más cerca de la cámara o del jugador para luego encontrar en qué segmento del "riel" debería estar.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetClosestNode(Vector3 player)
    {
        int closestNodeIndex = -1; //Nodo más cercano

        float shortestDistance = Mathf.Infinity; //Distancia más corta

        for (int i = 0; i < nodes.Count; i++) //Recorro cada nodo.
        {
            //Calculo la distancia entre el nodo actual y el parámetro.
            float newDistance = (nodes[i].position - player).sqrMagnitude;

            //Si la distancia es mas corta que la anterior.
            if (newDistance < shortestDistance)
            {
                //Actualizo la distancia más corta.
                shortestDistance = newDistance;

                closestNodeIndex = i;
            }
        }

        return closestNodeIndex;
    }


    /// <summary>
    /// Permite encontrar el punto más cercano a pos dentro del segmento [v1, v2], asegurando que el objeto se mantenga en el riel.
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    private Vector3 ProjectOnSegment(Vector3 nodeA, Vector3 nodeB, Vector3 player)
    {
        Vector3 segmentDirection = (nodeB - nodeA).normalized; //la dirección.
        float distanceFromNodeA = Vector3.Dot(segmentDirection, player - nodeA);

        if (distanceFromNodeA < 0f) return nodeA; //Si es menor que 0, devuelve nodeA (fuera del segmento por la izquierda). Si estoy al principio del rail.

        if (distanceFromNodeA * distanceFromNodeA > (nodeB - nodeA).sqrMagnitude) return nodeB; //Si es mayor que la longitud del segmento, devuelve nodeB (fuera del segmento por la derecha). Si estoy al final de rail.

        return nodeA + segmentDirection * distanceFromNodeA; //Si está dentro del segmento, devuelve la posición proyectada.
    }
}
