using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    [SerializeField] private CharacterTarget _trinketCharacter;
    [SerializeField] private int _buildIndex;

    [Space(10),SerializeField] private float _rotationSpeed;
    [SerializeField] private float _imageDisplayDuration = 2f;
    public Image imageColec;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas

        //Pido el buildIndex y paso como referencia
        _buildIndex = GameManager.instance.SetCollectible(this, _trinketCharacter);
        Debug.Log($"Mi Build Index es {_buildIndex}");

        if (_trinketCharacter == CharacterTarget.Bongo)
        {
            //if (CallJson.instance.refJasonSave.GetValueCollectableDict(currentScene.buildIndex, "BongoTrinket")) gameObject.SetActive(false);

            if (CallJson.instance.refJasonSave.GetValueCollectableDict(_buildIndex, "BongoTrinket")) gameObject.SetActive(false);
        }
        else
        {
            //if (CallJson.instance.refJasonSave.GetValueCollectableDict(currentScene.buildIndex, "FrankTrinket")) gameObject.SetActive(false);
            if (CallJson.instance.refJasonSave.GetValueCollectableDict(_buildIndex, "FrankTrinket")) gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, _rotationSpeed, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            Take();
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

    private void Take()
    {
        Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas

        if (_trinketCharacter == CharacterTarget.Bongo)
        {
            CallJson.instance.refJasonSave.ModyfyValueCollectableDict(_buildIndex, "BongoTrinket", true);
            GameManager.instance.UIBongoTrincket.CollectibleTaken();

            //Debug.LogWarning("Coleccionable BONGO agarrado");
            gameObject.SetActive(false);
        }
        else
        {
            CallJson.instance.refJasonSave.ModyfyValueCollectableDict(_buildIndex, "FrankTrinket", true);
            GameManager.instance.UIFrankTrincket.CollectibleTaken();

            //Debug.LogWarning("Coleccionable FRANK agarrado");
            gameObject.SetActive(false);
        }
    }
}
