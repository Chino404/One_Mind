using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Colectionable : MonoBehaviour
{
    [SerializeField] private CharacterTarget _trinketCharacter;

    [Space(10),SerializeField] private float _rotationSpeed;
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

        Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas
        var collectableLevels = ColectableManager.instance.collectablesLevels;

        var level = CallJson.instance.refJasonSave.GetSaveData.levels;

        foreach (var col in collectableLevels)
        {
            if(currentScene.buildIndex == col.indexLevel) //Si la escena en la que estoy concuerda con un indexLevel
            {
                if (_trinketCharacter == CharacterTarget.Bongo) //Si el que agarre es el de Bongo
                {
                    col.isBongoTaken = true;

                    for (int i = 0; i < level.Length; i++)
                    {
                        if (currentScene.buildIndex == level[i].indexLevelJSON)
                        {
                            level[i].isBongoTakenTrinket = col.isBongoTaken;

                            level[i].collectablesJSON["BongoTrinket"] = col.isBongoTaken;

                            Debug.Log($"{col.isBongoTaken}");

                            break;
                        }
                    }
                }

                else //Si es de Frank
                {
                    col.isFrankTaken = true;

                    for (int i = 0; i < level.Length; i++)
                    {
                        if (currentScene.buildIndex == level[i].indexLevelJSON)
                        {
                            level[i].isFrankTakenTrinket = col.isFrankTaken;

                            level[i].collectablesJSON["FrankTrinket"] = col.isFrankTaken;

                            Debug.Log($"{col.isFrankTaken}");

                            break;
                        }
                    }

                }
            }
        }


        //if (currentScene.buildIndex == 2)
        //{
        //    for (int i = 0; i < ColectableManager.instance.collectablesCollectedLvl1.Length; i++)
        //    {
        //        if (ColectableManager.instance.collectablesCollectedLvl1[i] == false)
        //        {
        //            ColectableManager.instance.collectablesCollectedLvl1[i] = true;
        //            //if (CallJson.instance.refJasonSave.GetSaveData.levels[currentScene.buildIndex].collectablesJSON.ContainsKey(""))

        //            break;
        //        }
        //    }
        //}

        //else if (currentScene.buildIndex == 3)
        //{
        //    for (int i = 0; i < ColectableManager.instance.collectablesCollectedLvl2.Length; i++)
        //    {
        //        if (ColectableManager.instance.collectablesCollectedLvl2[i] == false)
        //        {
        //            ColectableManager.instance.collectablesCollectedLvl2[i] = true;
        //            break;
        //        }
        //    }

        //}
            
    }
}
