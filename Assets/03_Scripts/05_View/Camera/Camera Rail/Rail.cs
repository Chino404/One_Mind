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
    /// Asegura que la c�mara (o lo que est�s moviendo) siga el riel correctamente, aline�ndola con el segmento m�s cercano.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public Vector3 ProjectPositionOnRail(Vector3 player)
    {
        int closestNodeIndex = GetClosestNode(player); // Obtengo el nodo m�s cercano.


        //Si el nodo m�s cercano es el primero o el �ltimo de la lista, proyecta player sobre el segmento que conecta con el siguiente o anterior nodo, respectivamente.
        if (closestNodeIndex == 0) //Primer nodo
        {
            return ProjectOnSegment(nodes[0].position, nodes[1].position, _target.position);
        }

        if (closestNodeIndex == nodes.Count - 1) //�ltimo nodo
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
    /// Determina qu� nodo est� m�s cerca de la c�mara o del jugador para luego encontrar en qu� segmento del "riel" deber�a estar.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetClosestNode(Vector3 player)
    {
        int closestNodeIndex = -1; //Nodo m�s cercano

        float shortestDistance = Mathf.Infinity; //Distancia m�s corta

        for (int i = 0; i < nodes.Count; i++) //Recorro cada nodo.
        {
            //Calculo la distancia entre el nodo actual y el par�metro.
            float newDistance = (nodes[i].position - player).sqrMagnitude;

            //Si la distancia es mas corta que la anterior.
            if (newDistance < shortestDistance)
            {
                //Actualizo la distancia m�s corta.
                shortestDistance = newDistance;

                closestNodeIndex = i;
            }
        }

        return closestNodeIndex;
    }


    /// <summary>
    /// Permite encontrar el punto m�s cercano a pos dentro del segmento [v1, v2], asegurando que el objeto se mantenga en el riel.
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    private Vector3 ProjectOnSegment(Vector3 nodeA, Vector3 nodeB, Vector3 player)
    {
        Vector3 segmentDirection = (nodeB - nodeA).normalized; //la direcci�n.
        float distanceFromNodeA = Vector3.Dot(segmentDirection, player - nodeA);

        if (distanceFromNodeA < 0f) return nodeA; //Si es menor que 0, devuelve nodeA (fuera del segmento por la izquierda). Si estoy al principio del rail.

        if (distanceFromNodeA * distanceFromNodeA > (nodeB - nodeA).sqrMagnitude) return nodeB; //Si es mayor que la longitud del segmento, devuelve nodeB (fuera del segmento por la derecha). Si estoy al final de rail.

        return nodeA + segmentDirection * distanceFromNodeA; //Si est� dentro del segmento, devuelve la posici�n proyectada.
    }
}
