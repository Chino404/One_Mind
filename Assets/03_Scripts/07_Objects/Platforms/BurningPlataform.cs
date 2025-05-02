using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BurningPlataform : MonoBehaviour
{
    public MeshRenderer[] renderers;
    private bool _isBurning;
    private float _currentValue = 0f;
    private float _targetValue = 0f;
    private float _speed=2f;
    private List<Material> _materials=new();

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        

    }

    private void Start()
    {
        foreach (var item in renderers)
        {
            _materials.AddRange(item.materials);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DeadInFall>())
            _targetValue = 1f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DeadInFall>())
            _targetValue = 0f;
    }

    private void Update()
    {
        if (_targetValue == 0f)
            _speed = 0.5f;
        if (_targetValue == 1f)
            _speed = 5f;
        _currentValue = Mathf.MoveTowards(_currentValue, _targetValue, _speed * Time.deltaTime);

        //for (int i = 0; i < renderers.Length; i++)
        //{
        //    renderers[i].material.SetFloat("_DisolverLava", _currentValue);
        //}


        foreach (var item in _materials)
        {
            item.SetFloat("_DisolverLava", _currentValue);
        }



    }
}
