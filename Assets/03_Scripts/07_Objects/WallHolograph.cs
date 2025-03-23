using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHolograph : DesactiveWall
{
    private Renderer _opacityMaterial;
    [SerializeField, Range(0, 1f)] private float _valueOpacity = 1;
    private int _IdOpacity = Shader.PropertyToID("_Opacity");

    private Animator _animator;

    public override void Awake()
    {
        _animator = GetComponent<Animator>();
        _opacityMaterial = GetComponent<Renderer>();
        base.Awake();
    }

    private void Update()
    {
        _opacityMaterial.material.SetFloat(_IdOpacity, _valueOpacity);
    }

    public override void Active()
    {
        StartCoroutine(timeToActive());
    }

    IEnumerator timeToActive()
    {
        //gameObject.SetActive(true);
        _animator.SetTrigger("Desactive");
        yield return new WaitForSeconds(0.25f);
        _animator.SetTrigger("Active");

    }

    public override void Desactive()
    {
        StartCoroutine(timeToDesactive());
        AudioManager.instance.Play(SoundId.DesactiveWallHolograph);
        //OldAudioManager.instance.PlaySFX(OldAudioManager.instance.wallHoloraphActive);
    }

    IEnumerator timeToDesactive()
    {

        var timer = 0f;

        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.25f;
            _valueOpacity = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        //_animator.SetTrigger("Desactive");
        //yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
    }

    public override void Save()
    {
        
        _currentState.Rec(gameObject.activeInHierarchy, _valueOpacity);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;
        var col = _currentState.Remember();
        gameObject.SetActive((bool)col.parameters[0]);
        _valueOpacity = (float)col.parameters[1];
    }
}
