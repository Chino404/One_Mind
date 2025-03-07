using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private Transform[] _nodes;
    [SerializeField] private int _nodeCount; 

    private void Awake()
    {
        _nodeCount = transform.childCount;
        _nodes = new Transform[_nodeCount];

        for (int i = 0; i < _nodeCount; i++)
        {
            _nodes[i] = transform.GetChild(i);
        }
    }


}
