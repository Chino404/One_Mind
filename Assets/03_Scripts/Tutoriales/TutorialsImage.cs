using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsImage : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ModelMonkey>())
        {
            StartCoroutine(Active());
        }
    }

    IEnumerator Active()
    {
        tutorialCanvas.SetActive(true);
        yield return new WaitForSeconds(3f);
        tutorialCanvas.SetActive(false);
        gameObject.SetActive(false);

    }
}
