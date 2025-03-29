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
                nodes.Add(node);
                node.index = i;
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
        //ACA ES PARA ROTAR LA C�MARA EN EL NODO QUE IRIA (CREO)

        NodesOfTheRail closestNode = GetClosestNode(player); // Obtengo el nodo m�s cercano.

        //Si el nodo m�s cercano es el primero o el �ltimo de la lista, proyecta player sobre el segmento que conecta con el siguiente o anterior nodo, respectivamente.
        if (closestNode.index == 0) //Primer nodo
        {
            //nodes[0].transform.position,
            return (ProjectOnSegment(closestNode.transform.position, nodes[1].transform.position, _target.position));
        }
        if (closestNode.index == nodes.Count - 1) //�ltimo nodo
        {
                                    //nodes[nodes.Count - 1].transform.position,
            return (ProjectOnSegment(closestNode.transform.position, nodes[nodes.Count - 2].transform.position, _target.position));
        }

        //LO DEJO POR LAS DUDAS, PERO AS� ESTABA ANTES
        //Vector3 nextNode = ProjectOnSegment(nodes[closestNode.index + 1].transform.position, closestNode.transform.position, player);
        //Vector3 backNode = ProjectOnSegment(nodes[closestNode.index - 1].transform.position, closestNode.transform.position, player);

        Vector3 nextNode = ProjectOnSegment(closestNode.transform.position, nodes[closestNode.index + 1].transform.position, player);
        Vector3 previousNode = ProjectOnSegment(closestNode.transform.position, nodes[closestNode.index - 1].transform.position, player);

        Debug.DrawLine(player, nodes[closestNode.index + 1].transform.position, Color.blue);
        Debug.DrawLine(player, nodes[closestNode.index - 1].transform.position, Color.yellow);

        //Debug.DrawLine(player, nextNode, Color.blue);
        //Debug.DrawLine(player, previousNode, Color.yellow);

        //Si me encuentro m�s cerca de un nodo o del otro.
        _targetPos = (player - previousNode).sqrMagnitude <= (player - nextNode).sqrMagnitude ? previousNode : nextNode;

        
        return _targetPos;
    }

    /// <summary>
    /// Determina qu� nodo est� m�s cerca de la c�mara o del jugador para luego encontrar en qu� segmento del "riel" deber�a estar.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public NodesOfTheRail GetClosestNode(Vector3 target)
    {
        NodesOfTheRail auxNode = default;

        float shortestDistance = Mathf.Infinity; //Distancia m�s corta

        for (int i = 0; i < nodes.Count; i++) //Recorro cada nodo.
        {
            //Calculo la distancia entre el nodo actual y el par�metro.
            float newDistance = (nodes[i].transform.position - target).sqrMagnitude;

            //Si la distancia es mas corta que la anterior.
            if (newDistance < shortestDistance)
            {
                //Actualizo la distancia m�s corta.
                shortestDistance = newDistance;

                auxNode = nodes[i];
            }
        }

        return auxNode;
    }


    /// <summary>
    /// Permite encontrar el punto m�s cercano a pos dentro del segmento [Node A, Node B], asegurando que el objeto se mantenga en el riel.
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    private Vector3 ProjectOnSegment(Vector3 nodeA, Vector3 nodeB, Vector3 player)
    {
        Vector3 segmentDirection = (nodeB - nodeA).normalized; //la direcci�n.
        float distanceFromNodeA = Vector3.Dot(segmentDirection, player - nodeA); //Dot.() Ser�a el producto punto entre dos vectores A y B. 

        if (distanceFromNodeA < 0f) return nodeA; //Si es menor que 0, devuelve nodeA (fuera del segmento por el primer nodo). Si estoy al principio del rail.

        if (distanceFromNodeA * distanceFromNodeA > (nodeB - nodeA).sqrMagnitude) return nodeB; //Si es mayor que la longitud del segmento, devuelve nodeB (fuera del segmento por el �ltimo nodo). Si estoy al final de rail.

        return nodeA + segmentDirection * distanceFromNodeA; //Si est� dentro del segmento, devuelve la posici�n proyectada.
    }
}
