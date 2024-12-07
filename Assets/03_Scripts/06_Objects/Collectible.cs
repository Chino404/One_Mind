using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public CharacterTarget trinketCharacter;
    private int _myBuildIndex;

    [Space(10),SerializeField] private float _rotationSpeed;


    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas

        //Pido el buildIndex y me paso como referencia
        _myBuildIndex = GameManager.instance.SetCollectibleManager(this);
        //_myBuildIndex = GameManager.instance.SetCollectible(this, trinketCharacter);

        //Debug.Log($"Mi Build Index es {_buildIndex}");

        if (trinketCharacter == CharacterTarget.Bongo)
        {
            //Si ya el coleccioanble esta agarrado, apago el objeto
            if (CallJson.instance.refJasonSave.GetValueCollectableDict(_myBuildIndex, "BongoTrinket")) gameObject.SetActive(false);
        }
        else
        {
            //Si ya el coleccioanble esta agarrado, apago el objeto
            if (CallJson.instance.refJasonSave.GetValueCollectableDict(_myBuildIndex, "FrankTrinket")) gameObject.SetActive(false);
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


    /// <summary>
    /// Cuando el jugador toma el coleccionable
    /// </summary>
    private void Take()
    {
        Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas

        if (trinketCharacter == CharacterTarget.Bongo)
        {
            //Cambio el booleano del dicccionario por verdadero
            CallJson.instance.refJasonSave.ModyfyValueCollectableDict(_myBuildIndex, "BongoTrinket", true);

            //Aviso a la UI que tome el coleccionable
            GameManager.instance.UIBongoTrincket.UICollectibleTaken();

            //Debug.LogWarning("Coleccionable BONGO agarrado");
            gameObject.SetActive(false);
        }
        else
        {
            //Cambio el booleano del dicccionario por verdadero
            CallJson.instance.refJasonSave.ModyfyValueCollectableDict(_myBuildIndex, "FrankTrinket", true);

            //Aviso a la UI que tome el coleccionable
            GameManager.instance.UIFrankTrincket.UICollectibleTaken();

            //Debug.LogWarning("Coleccionable FRANK agarrado");
            gameObject.SetActive(false);
        }
    }
}
