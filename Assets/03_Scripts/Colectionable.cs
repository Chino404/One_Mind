using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Colectionable : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    private bool _isCollected;

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
        }

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
