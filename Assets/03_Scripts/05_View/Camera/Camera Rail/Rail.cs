using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] private Transform _target;
    public Transform RailTarget { get => _target; set => _target = value; }

    [Tooltip("Nodos del rail.")] public List<NodesOfTheRail> nodes;
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private Quaternion _targetRot;

    [Tooltip("Cantidad de Nodos")] private int _nodeCount;

    private void Awake()
    {
        _nodeCount = transform.childCount;
        //nodes = new Transform[_nodeCount];

        for (int i = 0; i < _nodeCount; i++)
        {
            var node = transform.GetChild(i).gameObject.GetComponent<NodesOfTheRail>();

            if (node)
            {
                //nodes[i] = transform.GetChild(i);

                //nodes.Add(transform.GetChild(i).GetComponent<NodesOfTheRail>());

                nodes.Add(node);
                node.index = i;
            }

        }
    }

    /// <summary>
    /// Asegura que la cámara (o lo que estés moviendo) siga el riel correctamente, alineándola con el segmento más cercano.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public (Vector3, Quaternion) ProjectPositionOnRail(Vector3 player)
    {
        //ACA ES PARA ROTAR LA CÁMARA EN EL NODO QUE IRIA (CREO)

        NodesOfTheRail closestNode = GetClosestNode(player); // Obtengo el nodo más cercano.

        if (closestNode.isToRotateTheCamera) _targetRot = closestNode.rotationCamera;
        else _targetRot = Quaternion.identity;

        //Si el nodo más cercano es el primero o el último de la lista, proyecta player sobre el segmento que conecta con el siguiente o anterior nodo, respectivamente.
        if (closestNode.index == 0) //Primer nodo
        {
            return (ProjectOnSegment(nodes[0].transform.position, nodes[1].transform.position, _target.position), _targetRot);
        }

        else if (closestNode.index == nodes.Count - 1) //Último nodo
        {
            return (ProjectOnSegment(nodes[nodes.Count - 1].transform.position, nodes[nodes.Count - 2].transform.position, _target.position), _targetRot);
        }

        Vector3 nextNode = ProjectOnSegment(nodes[closestNode.index + 1].transform.position, closestNode.transform.position, player);
        Vector3 backNode = ProjectOnSegment(nodes[closestNode.index - 1].transform.position, closestNode.transform.position, player);

        Debug.DrawLine(player, nextNode, Color.blue);
        Debug.DrawLine(player, backNode, Color.yellow);

        _targetPos = (player - backNode).sqrMagnitude <= (player - nextNode).sqrMagnitude ? backNode : nextNode;

        

        return (_targetPos, _targetRot);
    }

    //public Vector3 ProjectPositionOnRail(Vector3 player)
    //{
    //    //int closestNodeIndex = GetClosestNode(player); // Obtengo el nodo más cercano.
    //    NodesOfTheRail closestNode = GetClosestNode(player); // Obtengo el nodo más cercano.

    //    //Si el nodo más cercano es el primero o el último de la lista, proyecta player sobre el segmento que conecta con el siguiente o anterior nodo, respectivamente.
    //    if (closestNode.index == 0) //Primer nodo
    //    {
    //        return ProjectOnSegment(nodes[0].transform.position, nodes[1].transform.position, _target.position);
    //    }

    //    else if (closestNode.index == nodes.Count - 1) //Último nodo
    //    {
    //        return ProjectOnSegment(nodes[nodes.Count - 1].transform.position, nodes[nodes.Count - 2].transform.position, _target.position);
    //    }

    //    Vector3 nextNode = ProjectOnSegment(nodes[closestNode.index + 1].transform.position, closestNode.transform.position, player);
    //    Vector3 backNode = ProjectOnSegment(nodes[closestNode.index - 1].transform.position, closestNode.transform.position, player);

    //    Debug.DrawLine(player, nextNode, Color.blue);
    //    Debug.DrawLine(player, backNode, Color.yellow);

    //    return (player - backNode).sqrMagnitude <= (player - nextNode).sqrMagnitude ? backNode : nextNode;
    //}

    /// <summary>
    /// Determina qué nodo está más cerca de la cámara o del jugador para luego encontrar en qué segmento del "riel" debería estar.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private NodesOfTheRail GetClosestNode(Vector3 player)
    {
        NodesOfTheRail auxNode = default;

        float shortestDistance = Mathf.Infinity; //Distancia más corta

        for (int i = 0; i < nodes.Count; i++) //Recorro cada nodo.
        {
            //Calculo la distancia entre el nodo actual y el parámetro.
            float newDistance = (nodes[i].transform.position - player).sqrMagnitude;

            //Si la distancia es mas corta que la anterior.
            if (newDistance < shortestDistance)
            {
                //Actualizo la distancia más corta.
                shortestDistance = newDistance;

                auxNode = nodes[i];
            }
        }

        return auxNode;
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
