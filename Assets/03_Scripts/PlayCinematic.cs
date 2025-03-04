using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayCinematic : MonoBehaviour
{
    public PlayableDirector[] cinematic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            foreach (var item in cinematic)
            {
                item.Play();
            }
        }
    }
}
