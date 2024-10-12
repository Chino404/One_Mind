using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualDoor : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        
        _animator = GetComponent<Animator>();
    }

    public void OpenTheDoor()
    {
        //OldAudioManager.instance.PlaySFX(OldAudioManager.instance.doorOpen);
        _animator.SetTrigger("Open");
        AudioManager.instance.Play(SoundId.OpenDoor);

    }

    
}
