using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Colectionable : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _imageDisplayDuration = 2f;
    private bool _isCollected;
    public Image imageColec;

    private void Update()
    {
        transform.Rotate(new Vector3(0, _rotationSpeed, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            Debug.Log("agarre coleccionable");

            _isCollected = true;
            gameObject.SetActive(false);
            //imageColec.gameObject?.SetActive(true);
            //StartCoroutine(ShowImageAndDisable());
        }

    }

    //new
    private IEnumerator ShowImageAndDisable()
    {
        float elapsedTime = 0f;
        Color imageColor = imageColec.color;
        imageColor.a = 0.5f; 
        imageColec.color = imageColor;

        
        while (elapsedTime < _imageDisplayDuration)
        {
            elapsedTime += Time.deltaTime;
            imageColor.a = Mathf.Clamp01(elapsedTime / _imageDisplayDuration); 
            imageColec.color = imageColor;
            yield return null;
        }

        
        yield return new WaitForSeconds(1f); 

        
        imageColor.a = 0; 
        imageColec.color = imageColor;
        imageColec.gameObject?.SetActive(false);
    }

    private void OnDisable()
    {
        if (_isCollected == false) return;
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 2)
        {
            for (int i = 0; i < ColectableManager.instance.collectablesCollectedLvl1.Length; i++)
            {
                if (ColectableManager.instance.collectablesCollectedLvl1[i] == false)
                {
                    ColectableManager.instance.collectablesCollectedLvl1[i] = true;
                    break;
                }
            }
        }

        else if (currentScene.buildIndex == 3)
        {
            for (int i = 0; i < ColectableManager.instance.collectablesCollectedLvl2.Length; i++)
            {
                if (ColectableManager.instance.collectablesCollectedLvl2[i] == false)
                {
                    ColectableManager.instance.collectablesCollectedLvl2[i] = true;
                    break;
                }
            }

        }
            
    }
}
