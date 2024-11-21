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
        var level = ColectableManager.instance.collectablesLevels;

        var json = CallJson.instance.refJasonSave.GetSaveData.levels;

        foreach (var item in level)
        {
            if(currentScene.buildIndex == item.indexLevel)
            {
                if (_trinketCharacter == CharacterTarget.Bongo)
                {
                    item.isBongoTaken = true;


                    for (int i = 0; i < json.Length; i++)
                    {
                        if (currentScene.buildIndex == json[i].indexLevelJSON)
                        {
                            //if(!json[i].collectablesJSON.ContainsKey("BongoTrinket"))
                            //{
                            //    json[i].collectablesJSON.Add("BongoTrinket", true);
                            //}
                        }
                    }
                }

                else
                {
                    item.isFrankTaken = true;

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
