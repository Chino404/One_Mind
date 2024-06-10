using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBanana : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject canvasOfMonkey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ModelBanana>()&&other.gameObject.GetComponent<ModelBanana>().enabled==true)
        {
            tutorialCanvas.SetActive(true);
            
        }
    }

    private void Update()
    {
        if (gameObject.GetComponent<ModelBanana>().enabled == false)
            tutorialCanvas.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<ModelBanana>() && other.gameObject.GetComponent<ModelBanana>().enabled==false)
        {
            tutorialCanvas.SetActive(false);
        }
    }

    //IEnumerator Active()
    //{
    //    tutorialCanvas.SetActive(true);
    //    yield return new WaitForSeconds(3f);
    //    tutorialCanvas.SetActive(false);
    //    gameObject.SetActive(false);

    //}
}
