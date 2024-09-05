using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHolograph : MonoBehaviour
{
    private Renderer _opacityMaterial;
    [SerializeField, Range(0, 1f)] private float _valueOpacity = 1;
    private int _IdOpacity = Shader.PropertyToID("_Opacity");

    [SerializeField] private Animator _animator;

    public bool isActive;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _opacityMaterial = GetComponent<Renderer>();
    }

    private void Update()
    {
        _opacityMaterial.material.SetFloat(_IdOpacity, _valueOpacity);
    }

    public void Active()
    {
        StartCoroutine(timeToActive());
    }

    IEnumerator timeToActive()
    {
        //gameObject.SetActive(true);
        _animator.SetTrigger("Desactive");
        yield return new WaitForSeconds(1.0f);
        _animator.SetTrigger("Active");

    }

    public void Desactive()
    {
        StartCoroutine(timeToDesactive());
    }

    IEnumerator timeToDesactive()
    {
        _animator.SetTrigger("Desactive");
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}
