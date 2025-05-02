using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LivingBeingsAnim : MonoBehaviour
{

    private Animation _animation;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    private void Start()
    {
        _animation.Play();
    }

}
